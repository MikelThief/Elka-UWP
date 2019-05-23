using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserDataAccounts.Provider;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Services;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Helpers;
using Studia = ElkaUWP.DataLayer.Studia;

namespace ElkaUWP.DataLayer.Propertiary.Services
{
    public class PartialGradesService
    {
        // keeps ids of nodes collected while running Get()
        private HashSet<int> CollectedNodeIds = new HashSet<int>();

        private CrstestsService _crstestsService;
        private Studia.Services.SubjectsPartialGradesService _studiaSubjectsPartialGradesService;

        public PartialGradesService(CrstestsService crstestsService, Studia.Services.SubjectsPartialGradesService studiaSubjectsPartialGradesService)
        {
            _crstestsService = crstestsService;
            _studiaSubjectsPartialGradesService = studiaSubjectsPartialGradesService;
        }

        public async Task<PartialGradesContainer> GetAsync(string semesterLiteral, string subjectId)
        {
            var res = _studiaSubjectsPartialGradesService.CheckIfCorrectLogin();

            var nodes = GetUsosTreeAsync(semesterLiteral: semesterLiteral, subjectId: subjectId);

            return new PartialGradesContainer()
            {
                SemesterLiteral = semesterLiteral,
                SubjectId = subjectId,
                Nodes = await nodes
            };
        }

        private async Task<List<PartialGradeNode>> GetUsosTreeAsync(string semesterLiteral, string subjectId)
        {
            // pseudo state-less behaviour to achieve greater processing speed
            CollectedNodeIds.Clear();

            var nodes = new List<PartialGradeNode>();

            // Step 1 - get all user's tests
            var testStubs = await _crstestsService.ParticipantAsync().ConfigureAwait(continueOnCapturedContext: false);

            var subjectRootNodes = new List<Node>();

            // Step 1a - add tests if there are any for given pair <semester, subject>
            if(testStubs.ContainsKey(key: semesterLiteral))
            {
                foreach (var nodeKey in testStubs[key: semesterLiteral].Keys)
                {
                    if(testStubs[key: semesterLiteral][key: nodeKey].CourseEdition.CourseId == subjectId)
                        subjectRootNodes.Add(item: testStubs[key: semesterLiteral][key: nodeKey]);
                }
            }

            // Step 2 - download and convert data to proprietary structures
            foreach (var subjectRootNode in subjectRootNodes)
            {
                var subTreeTask = _crstestsService.NodeAsync(nodeId: subjectRootNode.NodeId);

                var partialGradeRootNode = new PartialGradeNode();

                void CopyUsosNodeToPartialGradeNodeRecursive(Node usosNode, PartialGradeNode partialGradeNode)
                {
                    CollectedNodeIds.Add(item: usosNode.NodeId);

                    partialGradeNode.Type = usosNode.Type;
                    partialGradeNode.Order = usosNode.Order;
                    partialGradeNode.Id = usosNode.NodeId;
                    partialGradeNode.Points = null;

                    string name = default;
                    string desciption = default;

                    if (usosNode.Type == NodeType.Root)
                    {
                        desciption = usosNode.Description?.En;


                        if (string.Compare(strA: CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, "pl",
                                comparisonType: StringComparison.OrdinalIgnoreCase) == 0
                            || string.IsNullOrEmpty(value: desciption))
                            desciption = usosNode.Description?.Pl;
                    }

                    name = usosNode.Name?.En;
                    if (string.Compare(strA: CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, "pl",
                            comparisonType: StringComparison.OrdinalIgnoreCase) == 0
                        || string.IsNullOrEmpty(value: name))
                        name = usosNode.Name?.Pl;

                    // Merge description and name
                    if (string.IsNullOrEmpty(value: name) && !string.IsNullOrEmpty(value: desciption))
                    {
                        partialGradeNode.Description = desciption;
                    }
                    else if (!string.IsNullOrEmpty(value: name) && string.IsNullOrEmpty(value: desciption))
                    {
                        partialGradeNode.Description = name;
                    }
                    else if(string.IsNullOrEmpty(value: name) && string.IsNullOrEmpty(value: desciption))
                    {
                        partialGradeNode.Description = "";
                    }
                    else
                    {
                        partialGradeNode.Description = string.Join(separator: " | ", name, desciption);
                    }

                    if (usosNode.SubNodes != null || usosNode.SubNodes.Count == 0)
                    {
                        if (partialGradeNode.Nodes == null)
                            partialGradeNode.Nodes = new List<PartialGradeNode>();

                        foreach (var node in usosNode.SubNodes)
                        {
                            var partialGradeSubNode = new PartialGradeNode();
                            partialGradeNode.Nodes.Add(item: partialGradeSubNode);
                            CopyUsosNodeToPartialGradeNodeRecursive(usosNode: node,
                                partialGradeNode: partialGradeSubNode);
                        }
                    }
                }

                CopyUsosNodeToPartialGradeNodeRecursive(usosNode: await subTreeTask.ConfigureAwait(continueOnCapturedContext: false), partialGradeNode: partialGradeRootNode);

                nodes.Add(item: partialGradeRootNode);
            }

            // Step 3 - download points for collected nodes points to nodes
            var pointsList = await _crstestsService.UserPointsAsync(nodeIds: CollectedNodeIds).ConfigureAwait(continueOnCapturedContext: false);

            void AssignPointsToPartialGradeNodeRecursive(PartialGradeNode partialRootGradeNode,
                List<TestPoint> points)
            {
                if (points.Exists(point => point.NodeId == partialRootGradeNode.Id))
                    partialRootGradeNode.Points =
                        points.Single(point => point.NodeId == partialRootGradeNode.Id).Points;


                if (partialRootGradeNode.Nodes != null && partialRootGradeNode.Nodes.Count > 0)
                    foreach (var node in partialRootGradeNode.Nodes)
                        AssignPointsToPartialGradeNodeRecursive(partialRootGradeNode: node, points: points);
            }

            foreach (var partialGradeNode in nodes)
                AssignPointsToPartialGradeNodeRecursive(partialRootGradeNode: partialGradeNode, points: pointsList);

            // Step 4 - remove redundant nodes
            var nodesToRemove = new List<PartialGradeNode>();

            foreach (var rootNode in nodes)
                if(rootNode.Nodes != null)
                    foreach (var node in rootNode.Nodes)
                        if (!node.Points.HasValue && (node.Nodes == null || node.Nodes.Capacity == 0))
                            nodesToRemove.Add(item: node);

            foreach (var node in nodesToRemove)
            {
                foreach (var partialGradeNode in nodes)
                {
                    if (partialGradeNode.Nodes.Contains(item: node))
                        partialGradeNode.Nodes.Remove(item: node);
                }
            }

            // for all of that above - USOS: FUCK YOU!
            return nodes;
        }
    }
}
