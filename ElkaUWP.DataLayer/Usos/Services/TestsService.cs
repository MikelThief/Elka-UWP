using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using Newtonsoft.Json;
using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class TestsService : UsosServiceBase
    {
        /// <inheritdoc />
        public TestsService(ILogger logger, IContainerExtension containerExtension) : base(logger, containerExtension)
        {

        }

        public async Task<Dictionary<string, Dictionary<int, Node>>> GetUserTestsPerSemester()
        {
            var request = (Container.Resolve<CrtestsParticipantRequestWrapper>());
            var requestString = request.GetRequestString();
            UserTests result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<UserTests>(value: json);
            }
            catch (WebException wexc)
            {
                Logger.Fatal(exception: wexc, "Unable to start OAuth handshake");
                return null;
            }
            catch (JsonException jexc)
            {
                Logger.Warn(exception: jexc, "Unable to deserialize incoming data");
                return null;
            }

            // intentional, as API returns a json with just a single element 'tests'
            return result.RootNodesPerSemester;
        }

        public async Task<Node> GetSubjectTestTree(int nodeId)
        {
            var request = (Container.Resolve<CrtestsNodeRequestWrapper>());
            var requestString = request.GetRequestString(nodeId: nodeId);
            Node result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<Node>(value: json);
            }
            catch (WebException wexc)
            {
                Logger.Fatal(exception: wexc, "Unable to start OAuth handshake");
                return null;
            }
            catch (JsonException jexc)
            {
                Logger.Warn(exception: jexc, "Unable to deserialize incoming data");
                return null;
            }

            return result;
        }

        public async Task<List<UserPoint>> GetUserPoints(params int[] nodeIds)
        {
            var request = (Container.Resolve<CrtestsUserPointsRequestWrapper>());
            var requestString = request.GetRequestString(nodeIds: nodeIds);
            List<UserPoint> result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<List<UserPoint>>(value: json);
            }
            catch (WebException wexc)
            {
                Logger.Fatal(exception: wexc, "Unable to start OAuth handshake");
                return null;
            }
            catch (JsonException jexc)
            {
                Logger.Warn(exception: jexc, "Unable to deserialize incoming data");
                return null;
            }

            return result;
        }


        public async Task<List<UserPoint>> GetUserPoints(List<int> nodeIds)
        {
            var request = (Container.Resolve<CrtestsUserPointsRequestWrapper>());
            var requestString = request.GetRequestString(nodeIds: nodeIds);
            List<UserPoint> result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<List<UserPoint>>(value: json);
            }
            catch (WebException wexc)
            {
                Logger.Fatal(exception: wexc, "Unable to start OAuth handshake");
                return null;
            }
            catch (JsonException jexc)
            {
                Logger.Warn(exception: jexc, "Unable to deserialize incoming data");
                return null;
            }

            return result;
        }


    }
}
