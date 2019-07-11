using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;


namespace ElkaUWP.Infrastructure.Services
{
    public class AppFirstLunchService
    {
        private volatile bool executed = false;

        /// <summary>
        /// Does not guarantee that te schedules task will be executed in case of crash.
        /// </summary>
        /// <param name="action">Action to perform.</param>
        public void Execute(Action action)
        {
            if (SystemInformation.IsFirstRun && !executed)
            {
                executed = true;
                action();
            }
        }

        public void Execute(IEnumerable<Action> actions)
        {
            if (SystemInformation.IsFirstRun && !executed)
            {
                executed = true;
                foreach (var action in actions)
                {
                    action();
                }
            }
        }
    }
}
