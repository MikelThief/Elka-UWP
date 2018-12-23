using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Unity.Builder;
using Unity.Extension;
using Unity.Policy;

namespace ElkaUWP.Infrastructure.Extensions
{
    public class NLogExtension : UnityContainerExtension, IBuildPlanPolicy
    {
        private static readonly Func<Type, string, string> _defaultGetName = (t, n) => t.FullName;

        public Func<Type, string, string> GetName { get; set; }

        protected override void Initialize() => Context.Policies.Set(type: typeof(ILogger), null, policyInterface: typeof(IBuildPlanPolicy), policy: this);

        public void BuildUp(IBuilderContext context)
        {
            Func<Type, string, string> method = GetName ?? _defaultGetName;
            context.Existing = LogManager.GetLogger(name: method(arg1: context.ParentContext?.BuildKey.Type,
                arg2: context.ParentContext?.BuildKey.Name));
            context.BuildComplete = true;
        }
    }
}
