using NLog;
using Prism.Ioc;

namespace ElkaUWP.DataLayer.Usos.Abstractions.Bases
{
    public class UsosServiceBase
    {
        protected ILogger Logger { get; private set; }
        protected IContainerExtension Container { get; private set; }

        public UsosServiceBase(ILogger logger, IContainerExtension containerExtension)
        {
            Logger = logger;
            Container = containerExtension;
        }
    }
}
