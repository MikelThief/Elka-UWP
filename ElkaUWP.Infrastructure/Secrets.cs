using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.Infrastructure
{

    // TODO: Place secrets in-app at compile-time. Currently secrets are taken from environment variables.
    public static class Secrets
    {
        public static string SYNCFUSION_UWP_SECRET { get; private set; }

        static Secrets()
        {
            SYNCFUSION_UWP_SECRET = Environment.GetEnvironmentVariable(variable: nameof(SYNCFUSION_UWP_SECRET));
        }
    }
}
