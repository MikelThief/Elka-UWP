using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Windows.Media.Audio;
using Windows.UI.Xaml.Media.Animation;
using Anotar.NLog;
using CSharpFunctionalExtensions;
using ElkaUWP.DataLayer.Propertiary.Abstractions.Bases;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Studia.Services;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Extensions;
using ElkaUWP.DataLayer.Usos.Requests;
using ElkaUWP.DataLayer.Usos.Services;
using ElkaUWP.Infrastructure;

namespace ElkaUWP.DataLayer.Propertiary.Services
{
    public class PartialGradesService
    {
        /// <summary>
        /// Keeps ids of nodes collected while running <see cref="GetAsync"/>
        /// </summary>
        private List<PartialGradeNode> CollectedReturningNodes;

        private readonly CrstestsService _crstestsService;
        private readonly GradeService _studiaGradesService;

        public PartialGradesService(CrstestsService crstestsService, GradeService studiaGradesService)
        {
            _crstestsService = crstestsService;
            _studiaGradesService = studiaGradesService;
            CollectedReturningNodes =  new List<PartialGradeNode>();
        }

        public async Task<PartialGradesModel> GetAsync(string semesterLiteral, string subjectId)
        {
            var studiaGradesTask = _studiaGradesService.GetAsync(semesterLiteral: semesterLiteral, subjectId: subjectId);
            var usosGradesTask = GetUsosGradesAsync(semesterLiteral: semesterLiteral, subjectId: subjectId);

            var model = new PartialGradesModel
            {
                SemesterLiteral = semesterLiteral,
                SubjectId = subjectId
            };

            var usosGradesResult = await usosGradesTask.ConfigureAwait(continueOnCapturedContext: true);

            if (usosGradesResult.IsSuccess && usosGradesResult.Value.HasValue)
            {
                model.GradeNodes = usosGradesResult.Value.Value;
            }

            var studiaGradesResult = await studiaGradesTask.ConfigureAwait(continueOnCapturedContext: false);
            if (studiaGradesResult.IsSuccess && studiaGradesResult.Value.HasValue)
            {
                model.GradeList = studiaGradesResult.Value.Value;
            }

            return model;
        }

        private async Task<Result<Maybe<List<PartialGradeNode>>>> GetUsosGradesAsync(string semesterLiteral, string subjectId)
        {
            // pseudo state-less behaviour to achieve greater processing speed
            CollectedReturningNodes.Clear();

            // Step 1 - get all user's tests
            var participantResult = await _crstestsService.ParticipantAsync().ConfigureAwait(continueOnCapturedContext: false);

            if (participantResult.IsFailure)
                return Result.Fail<Maybe<List<PartialGradeNode>>>(error: participantResult.Error);

            if (participantResult.Value.HasNoValue || !participantResult.Value.Value.ContainsKey(key : semesterLiteral))
            {
                // User has no tests at all or in given semester
                return Result.Ok(value: Maybe<List<PartialGradeNode>>.None);
            }

            var testsForGivenSemester = participantResult.Value.Value;

            // Step 2 - generate list of root nodes for given subject
            var rootNodes = new List<Node>();

            rootNodes.AddRange(collection: testsForGivenSemester[key: semesterLiteral].Keys
                    .Where(nodeKey => testsForGivenSemester[key: semesterLiteral][key: nodeKey].CourseEdition.CourseId == subjectId)
                    .Select(nodeKey => testsForGivenSemester[key: semesterLiteral][key: nodeKey]));

            if (rootNodes.Count < 1)
            {
                // There are no tests root nodes for given subject
                return Result.Ok(value: Maybe<List<PartialGradeNode>>.None);
            }

            // Step 3 - download trees for all root nodes for given subject
            var rootTrees = new List<Node>();
            var returningRootTrees = new List<PartialGradeNode>();

            foreach (var rootNode in rootNodes)
            {
                var rootTreeResult = await _crstestsService.NodeAsync(nodeId: rootNode.NodeId);

                if(rootTreeResult.IsFailure)
                    continue;

                rootTrees.Add(item: rootTreeResult.Value);
            }

            // Step 4a - create proprietary data structures to return data
            foreach (var rootTree in rootTrees)
            {
                var returningRootTree = GetPartialGradeNode(usosNode: rootTree);
                returningRootTrees.Add(item: returningRootTree);
            }

            // Step 4b - fill proprietary data structures with points

            /* Parallel filling - some iterations that should be successful fail for unknown reason
            Parallel.ForEach(source: CollectedReturningNodes, new ParallelOptions()
            { MaxDegreeOfParallelism = 2 }, async collectedReturningNode =>
            {
                var pointsResult = await _crstestsService.StudentPointAsync(nodeId: collectedReturningNode.Id);

                if (pointsResult.IsSuccess && pointsResult.Value.HasValue)
                {
                    collectedReturningNode.Points = pointsResult.Value.Value.Points;
                }
            });
            */

            foreach (var collectedReturningNode in CollectedReturningNodes)
            {
                var pointsResult = await _crstestsService.StudentPointAsync(nodeId: collectedReturningNode.Id);

                if (pointsResult.IsSuccess && pointsResult.Value.HasValue)
                {
                    collectedReturningNode.Points = pointsResult.Value.Value.Points;
                }
            }

            return Result.Ok(value: Maybe<List<PartialGradeNode>>.From(obj: returningRootTrees));
        }

        private PartialGradeNode GetPartialGradeNode(Node usosNode)
        {
            var localSubNodes = new List<PartialGradeNode>();

            if (usosNode.SubNodes != null)
            {
                foreach (var subNode in usosNode.SubNodes)
                {
                    var localNode = GetPartialGradeNode(usosNode: subNode);
                    localSubNodes.Add(item: localNode);
                }
            }

            var returningNode = new PartialGradeNode(
                nodes: localSubNodes, header: usosNode.Name.GetValueForCurrentCulture(fallbackValue: "-", appendDescriptions: false), points: null, type: usosNode.Type, id: usosNode.NodeId, order: usosNode.Order);

            if (usosNode.VisibleForStudents)
            {
                CollectedReturningNodes.Add(item: returningNode);
            }

            return returningNode;
        }
    }
}
