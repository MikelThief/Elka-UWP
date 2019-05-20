using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Studia.ResolverParameters;

namespace ElkaUWP.DataLayer.Studia.Exceptions
{
    [Serializable]
    public sealed class LogonStrategyResolutionFailedException : Exception
    {
        private readonly LogonStrategyParametersContainer _parametersContainer;

        public LogonStrategyParametersContainer ParametersContainer => _parametersContainer;

        public LogonStrategyResolutionFailedException(string message, LogonStrategyParametersContainer parametersContainer)
            : base(message: message)
        {
            _parametersContainer = parametersContainer;
        }
        // Ensure exception is serializable
        [SecurityPermission(action: SecurityAction.Demand, SerializationFormatter = true)]
        protected LogonStrategyResolutionFailedException(SerializationInfo info, StreamingContext context)
            : base(info: info, context: context)
        {
            _parametersContainer = (LogonStrategyParametersContainer) info.GetValue(name: "LogonStrategyResolutionFailedException.ParametersContainer",
                type: typeof(LogonStrategyParametersContainer));
        }

        public LogonStrategyResolutionFailedException() : base()
        { }

        public LogonStrategyResolutionFailedException(string message) : base(message: message)
        { }

        public LogonStrategyResolutionFailedException(string message, Exception innerException) : base(message: message, innerException: innerException)
        { }

        [SecurityPermissionAttribute(action: SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info: info, context: context);

            info.AddValue(name: "LogonStrategyResolutionFailedException.ParametersContainer", value: ParametersContainer);
        }
    }
}
