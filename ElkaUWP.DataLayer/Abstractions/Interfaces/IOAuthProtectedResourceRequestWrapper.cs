using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Abstractions.Interfaces
{
    public interface IOAuthProtectedResourceRequestWrapper
    {
        string GetRequestString();
    }
}
