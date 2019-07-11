using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Anotar.NLog;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Converters.Json;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Requests;
using Newtonsoft.Json;
using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Usos.Services
{
    public class CoursesService
    {
        private readonly CoursesUserRequestWrapper _coursesUserRequestWrapper;
        /// <inheritdoc />
        public CoursesService(CoursesUserRequestWrapper coursesUserRequestWrapper)
        {
            _coursesUserRequestWrapper = coursesUserRequestWrapper;
        }

        public async Task<Dictionary<string, List<CourseEdition>>> UserAsync()
        {
            var requestString = _coursesUserRequestWrapper.GetRequestString();
            UserCoursesPerSemester result;
            var webClient = new WebClient();

            try
            {
                var json = await webClient.DownloadStringTaskAsync(address: requestString);

                result = JsonConvert.DeserializeObject<UserCoursesPerSemester>
                    (value: json, converters: new JsonPassTypeBoolConverter());
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

            // intentional, as API returns a json with just a single element 'course_editions'
            return result.CourseEditions;
        }
    }
}
