using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.Enums;
using ElkaUWP.DataLayer.Studia.ResolverParameters;

namespace ElkaUWP.DataLayer.Studia.Abstractions.Interfaces
{
    public interface ILogonStrategyResolver
    {
        ILogonStrategy Resolve(LogonStrategyParametersContainer container);
    }
}
