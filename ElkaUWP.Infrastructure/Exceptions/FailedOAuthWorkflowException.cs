using System;

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
