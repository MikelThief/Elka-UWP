using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Entities;

namespace ElkaUWP.DataLayer.Studia.Abstractions.Interfaces
{
    public interface IGradesProxy
    {
        /// <summary>
        /// Asynchronously retrieves JSON containing partial grades for given subject.
        /// </summary>
        /// <remarks>Interface implementation shall ensure that
        /// proxy is in state that allows to retrieve data.</remarks>
        Task<HttpResponseMessage> GetAsync(string subjectId, string semesterLiteral);

        /// <summary>
        /// Performs authentication of the proxy using supplied credential data in the system.
        /// </summary>
        Task Authenticate();

        /// <summary>
        /// Allows to check whether proxy is currently able to make requests
        /// without re-authentication.
        /// </summary>
        /// <returns>Yes if authenticated. No otherwise.</returns>
        bool IsAuthenticated();
    }
}
