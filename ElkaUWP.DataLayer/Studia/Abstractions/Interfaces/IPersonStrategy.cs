using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Entities;

namespace ElkaUWP.DataLayer.Studia.Abstractions.Interfaces
{
    public interface IPersonStrategy
    {
        Task<HttpResponseMessage> GetAsync();
    }
}
