using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.Infrastructure.Exceptions
{
    [Serializable]
    public class FailedOAuthWorkflowException : Exception
    {
        public string ExpectedToken { get; private set; }
        public string ActualToken { get; private set; }

        public FailedOAuthWorkflowException(string expectedToken, string actualToken)
        {
            ExpectedToken = expectedToken;
            ActualToken = actualToken;
        }
    }
}
