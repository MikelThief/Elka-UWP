using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserDataAccounts.Provider;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Services;
using ElkaUWP.Infrastructure;
using ElkaUWP.Infrastructure.Helpers;

namespace ElkaUWP.DataLayer.Propertiary.Services
{
    public class PartialGradesService
    {
        // keeps ids of nodes collected while running Get()
        private HashSet<int> CollectedNodeIds = new HashSet<int>();

        private CrstestsService _crstestsService;

        public PartialGradesService(CrstestsService crstestsService)
        {
            _crstestsService = crstestsService;
        }

        public async Task<PartialGradesTree> GetAsync(string semesterLiteral, string subjectId)
        {
            // pseudo state-less behaviour to achieve greater processing speed
            CollectedNodeIds.Clear();

            // Step 1 - get all user's tests
            var testStubs = await _crstestsService.ParticipantAsync().ConfigureAwait(continueOnCapturedContext: false);

            var partialGradesTree = new PartialGradesTree();
            partialGradesTree.Nodes = new List<PartialGradeNode>();
            partialGradesTree.SemesterLiteral = semesterLiteral;
            partialGradesTree.SubjectId = subjectId;

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

                CopyUsosNodeToPartialGradeNodeRecursive(usosNode: await subTreeTask.ConfigureAwait(continueOnCapturedContext: false), partialGradeNode: partialGradeRootNode);

                partialGradesTree.Nodes.Add(item: partialGradeRootNode);
            }

            // Step 3 - download points for collected nodes points to nodes
            var pointsList = await _crstestsService.UserPointsAsync(nodeIds: CollectedNodeIds).ConfigureAwait(continueOnCapturedContext: false);

            foreach (var partialGradeNode in partialGradesTree.Nodes)
                AssignPointsToPartialGradeNodeRecursive(partialGradeNode, points: pointsList);

            // Step 4 - remove redundant nodes
            var nodesToRemove = new List<PartialGradeNode>();

            foreach (var rootNode in partialGradesTree.Nodes)
                if(rootNode.Nodes != null)
                    foreach (var node in rootNode.Nodes)
                        if (!node.Points.HasValue && (node.Nodes == null || node.Nodes.Capacity == 0))
                            nodesToRemove.Add(item: node);

            foreach (var node in nodesToRemove)
            {
                foreach (var partialGradeNode in partialGradesTree.Nodes)
                {
                    if (partialGradeNode.Nodes.Contains(item: node))
                        partialGradeNode.Nodes.Remove(item: node);
                }
            }

            // for all of that above - USOS: FUCK YOU!
            return partialGradesTree;
        }     
        private void CopyUsosNodeToPartialGradeNodeRecursive(Node usosNode, PartialGradeNode partialGradeNode)
        {
            CollectedNodeIds.Add(usosNode.NodeId);

            partialGradeNode.Type = usosNode.Type;
            partialGradeNode.Order = usosNode.Order;
            partialGradeNode.Id = usosNode.NodeId;
            partialGradeNode.Points = null;


            if (usosNode.Type == NodeType.Root)
            {
                partialGradeNode.Desciption = usosNode.Description?.En;


                if (string.Compare(strA: CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, strB: "pl", comparisonType: StringComparison.OrdinalIgnoreCase) == 0
                    || string.IsNullOrEmpty(value: partialGradeNode.Desciption))
                    partialGradeNode.Desciption = usosNode.Description?.Pl;
            }

            partialGradeNode.Name = usosNode.Name?.En;
            if (string.Compare(strA: CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, strB: "pl", comparisonType: StringComparison.OrdinalIgnoreCase) == 0
                || string.IsNullOrEmpty(value: partialGradeNode.Name))
                partialGradeNode.Desciption = usosNode.Name?.Pl;

            if (usosNode.SubNodes != null || usosNode.SubNodes.Count == 0)
            {
                if (partialGradeNode.Nodes == null)
                    partialGradeNode.Nodes = new List<PartialGradeNode>();

                foreach (var node in usosNode.SubNodes)
                {
                    var partialGradeSubNode = new PartialGradeNode();
                    partialGradeNode.Nodes.Add(item: partialGradeSubNode);
                    CopyUsosNodeToPartialGradeNodeRecursive(usosNode: node, partialGradeNode: partialGradeSubNode);
                }
            }

        }

        private void AssignPointsToPartialGradeNodeRecursive(PartialGradeNode partialRootGradeNode, List<TestPoint> points)
        {
            if (points.Exists(point => point.NodeId == partialRootGradeNode.Id))
                partialRootGradeNode.Points = points.Single(point => point.NodeId == partialRootGradeNode.Id).Points;


            if (partialRootGradeNode.Nodes != null && partialRootGradeNode.Nodes.Count > 0)
                foreach (var node in partialRootGradeNode.Nodes)
                    AssignPointsToPartialGradeNodeRecursive(partialRootGradeNode: node, points: points);
        }



    }
}
