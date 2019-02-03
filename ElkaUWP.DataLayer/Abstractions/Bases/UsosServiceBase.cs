using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Infrastructure.Services;
using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Abstractions.Bases
{
    public class UsosServiceBase
    {
        protected ILogger Logger { get; private set; }
        protected IContainerProvider Container { get; private set; }

        protected UsosServiceBase(ILogger logger, IContainerProvider container)
        { 
            Logger = logger;
            Container = container;
        }
    }
}
