using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.Infrastructure.Abstractions;

namespace ElkaUWP.Infrastructure
{

    public static class Secrets
    {
        [BuildTimeEnvironmentVariable(environmentVariable: "SYNCFUSION_CONTROLS_SECRET")]
        public const string SYNCFUSION_CONTROLS_SECRET = "SYNCFUSION_CONTROLS_SECRET";

        [BuildTimeEnvironmentVariable(environmentVariable: "USOS_CONSUMER_KEY")]
        public const string USOS_CONSUMER_KEY = "USOS_CONSUMER_KEY";

        [BuildTimeEnvironmentVariable(environmentVariable: "USOS_CONSUMER_SECRET")]
        public const string USOS_CONSUMER_SECRET = "USOS_CONSUMER_SECRET";
    }
}
