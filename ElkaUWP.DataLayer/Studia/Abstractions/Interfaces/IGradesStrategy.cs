using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Entities;

namespace ElkaUWP.DataLayer.Studia.Abstractions.Interfaces
{
    public interface IGradesStrategy
    {
        /// <summary>
        /// Asynchronously retrieves JSON containing partial grades for given subject.
        /// </summary>
        /// <remarks>Interface implementation shall ensure that
        /// strategy is in state that allows to retrieve data.</remarks>
        Task<HttpResponseMessage> GetAsync(string subjectId, string semesterLiteral);
    }
}
