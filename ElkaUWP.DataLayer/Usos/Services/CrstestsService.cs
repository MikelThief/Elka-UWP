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
using ElkaUWP.Infrastructure;
using Newtonsoft.Json;
using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Usos.Services
{

    /// <summary>
    /// Wrapper for <see cref="https://apps.usos.pw.edu.pl/developers/api/services/crstests/"></see>
    /// </summary>
    public class CrstestsService
    {
        private readonly CrtestsStudentPointRequestWrapper _crtestsStudentPointRequestWrapper;
        private readonly CrstestsParticipantRequestWrapper _crstestsParticipantRequestWrapper;
        private readonly CrstestsNodeRequestWrapper _crstestsNodeRequestWrapper;
        private readonly CrstestsUserPointsRequestWrapper _crstestsUserPointsRequestWrapper;

        /// <inheritdoc />
        public CrstestsService(
            CrstestsParticipantRequestWrapper crstestsParticipantRequestWrapper,
            CrstestsNodeRequestWrapper crstestsNodeRequestWrapper,
            CrstestsUserPointsRequestWrapper crstestsUserPointsRequestWrapper,
            CrtestsStudentPointRequestWrapper crtestsStudentPointRequestWrapper)
        {
            _crstestsParticipantRequestWrapper = crstestsParticipantRequestWrapper;
            _crstestsNodeRequestWrapper = crstestsNodeRequestWrapper;
            _crstestsUserPointsRequestWrapper = crstestsUserPointsRequestWrapper;
            _crtestsStudentPointRequestWrapper = crtestsStudentPointRequestWrapper;
        }

        /// <summary>
        /// Wraps a call to <see cref="https://apps.usos.pw.edu.pl/developers/api/services/crstests/#participant"/>
        /// </summary>
        /// <returns><see cref="Maybe{T}"/> struct</returns>
        public async Task<Result<Maybe<Dictionary<string, Dictionary<int, Node>>>>> ParticipantAsync()
        {
            var requestString = _crstestsParticipantRequestWrapper.GetRequestString();
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);
                var result = JsonConvert.DeserializeObject<UserTests>(value: json);

                if (result.RootNodesPerSemester.Count > 0)
                {
                    // intentional, as API returns a json with just a single element 'tests'
                    return Result.Ok(value: Maybe<Dictionary<string, Dictionary<int, Node>>>
                        .From(obj: result.RootNodesPerSemester));
                }
                return Result.Ok(value: Maybe<Dictionary<string, Dictionary<int, Node>>>.None);
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to perform handshake with USOS.");
                return Result.Fail<Maybe<Dictionary<string, Dictionary<int, Node>>>>(error: ErrorCodes.USOS_HANDSHAKE_FAILED);
            }
            catch (JsonException jexc)
            {
                LogTo.WarnException(exception: jexc, message: "Unable to deserialize incoming data from USOS.");
                return Result.Fail<Maybe<Dictionary<string, Dictionary<int, Node>>>>(error: ErrorCodes.USOS_BAD_DATA_RECEIVED);
            }
            finally
            {
                webClient.Dispose();
                webClient = null;
            }
        }

        /// <summary>
        /// Wraps a call to <see cref="https://apps.usos.pw.edu.pl/developers/api/services/crstests/#node"/>
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public async Task<Result<Node>> NodeAsync(int nodeId)
        {
            var requestString = _crstestsNodeRequestWrapper.GetRequestString(nodeId: nodeId);
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                var result = JsonConvert.DeserializeObject<Node>(value: json);

                return Result.Ok(value: result);
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to perform handshake with USOS.");
                return Result.Fail<Node>(error: ErrorCodes.USOS_HANDSHAKE_FAILED);
            }
            catch (JsonException jexc)
            {
                LogTo.WarnException(exception: jexc, message: "Unable to deserialize incoming data from USOS.");
                return Result.Fail<Node>(error: ErrorCodes.USOS_BAD_DATA_RECEIVED);
            }
            finally
            {
                webClient.Dispose();
                webClient = null;
            }
        }

        public async Task<Result<List<TestPoint>>> UserPointsAsync(IEnumerable<int> nodeIds)
        {
            var requestString = _crstestsUserPointsRequestWrapper.GetRequestString(nodeIds: nodeIds.ToList());
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                var result = JsonConvert.DeserializeObject<List<TestPoint>>(value: json);

                return Result.Ok(value: result);
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to perform handshake with USOS.");
                return Result.Fail<List<TestPoint>>(error: ErrorCodes.USOS_HANDSHAKE_FAILED);
            }
            catch (JsonException jexc)
            {
                LogTo.WarnException(exception: jexc, message: "Unable to deserialize incoming data from USOS.");
                return Result.Fail<List<TestPoint>>(error: ErrorCodes.USOS_BAD_DATA_RECEIVED);
            }
            finally
            {
                webClient.Dispose();
                webClient = null;
            }
        }

        public async Task<Result<Maybe<StudentPoint>>> StudentPointAsync(int nodeId)
        {
            var requestString = _crtestsStudentPointRequestWrapper.GetRequestString(nodeId: nodeId);
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                var result = JsonConvert.DeserializeObject<StudentPoint>(value: json);

                return Result.Ok(value: result.Points == null
                    ? Maybe<StudentPoint>.None
                    : Maybe<StudentPoint>.From(obj: result));
            }
            catch (WebException wexc)
            {
                LogTo.FatalException(exception: wexc, message: "Unable to perform handshake with USOS.");
                return Result.Fail<Maybe<StudentPoint>>(error: ErrorCodes.USOS_HANDSHAKE_FAILED);
            }
            catch (JsonException jexc)
            {
                LogTo.WarnException(exception: jexc, message: "Unable to deserialize incoming data from USOS.");
                return Result.Fail<Maybe<StudentPoint>>(error: ErrorCodes.USOS_BAD_DATA_RECEIVED);
            }
            finally
            {
                webClient.Dispose();
                webClient = null;
            }
        }
    }
}
