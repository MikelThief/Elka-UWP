using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Anotar.NLog;
using CSharpFunctionalExtensions;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using Newtonsoft.Json;
using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class CrstestsService
    {
        private readonly CrstestsParticipantRequestWrapper _crstestsParticipantRequestWrapper;
        private readonly CrstestsNodeRequestWrapper _crstestsNodeRequestWrapper;
        private readonly CrstestsUserPointsRequestWrapper _crstestsUserPointsRequestWrapper;

        /// <inheritdoc />
        public CrstestsService(
            CrstestsParticipantRequestWrapper crstestsParticipantRequestWrapper,
            CrstestsNodeRequestWrapper crstestsNodeRequestWrapper,
            CrstestsUserPointsRequestWrapper crstestsUserPointsRequestWrapper)
        {
            _crstestsParticipantRequestWrapper = crstestsParticipantRequestWrapper;
            _crstestsNodeRequestWrapper = crstestsNodeRequestWrapper;
            _crstestsUserPointsRequestWrapper = crstestsUserPointsRequestWrapper;
        }

        public async Task<Dictionary<string, Dictionary<int, Node>>> ParticipantAsync()
        {
            var requestString = _crstestsParticipantRequestWrapper.GetRequestString();
            UserTests result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<UserTests>(value: json);
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to perform OAuth data exchange.");
                throw;
            }
            catch (JsonException jexc)
            {
                LogTo.WarnException(exception: jexc, message: "Unable to deserialize incoming data.");
                throw;
            }

            // intentional, as API returns a json with just a single element 'tests'
            return result.RootNodesPerSemester;
        }

        public async Task<Node> NodeAsync(int nodeId)
        {
            var requestString = _crstestsNodeRequestWrapper.GetRequestString(nodeId: nodeId);
            Node result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<Node>(value: json);
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to perform OAuth data exchange.");
                return null;
            }
            catch (JsonException jexc)
            {
                LogTo.WarnException(exception: jexc, message: "Unable to deserialize incoming data.");
                return null;
            }

            return result;
        }

        public async Task<List<TestPoint>> UserPointsAsync(params int[] nodeIds)
        {
            var requestString = _crstestsUserPointsRequestWrapper.GetRequestString(nodeIds: nodeIds);
            List<TestPoint> result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<List<TestPoint>>(value: json);
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to perform OAuth data exchange.");
                return null;
            }
            catch (JsonException jexc)
            {
                LogTo.WarnException(exception: jexc, message: "Unable to deserialize incoming data.");
                return null;
            }

            return result;
        }

        public async Task<List<TestPoint>> UserPointsAsync(IEnumerable<int> nodeIds)
        {
            var requestString = _crstestsUserPointsRequestWrapper.GetRequestString(nodeIds: nodeIds.ToList());
            List<TestPoint> result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<List<TestPoint>>(value: json);
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to perform OAuth data exchange.");
                return null;
            }
            catch (JsonException jexc)
            {
                LogTo.WarnException(exception: jexc, message: "Unable to deserialize incoming data.");
                return null;
            }

            return result;
        }
    }
}
