using System.Net;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Entities;

namespace ElkaUWP.DataLayer.Studia.Abstractions.Interfaces
{
    public interface IPartialGradesEngine
    {
        /// <summary>
        /// Asynchronously retrieves partial grades for given subject
        /// </summary>
        /// <remarks>Interface implementation shall ensure that engine is in state that allows to retrieve data</remarks>
        Task<Subject> GetPartialGradesAsync(string subjectId);
    }
}
