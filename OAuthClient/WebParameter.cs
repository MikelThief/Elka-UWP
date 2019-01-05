using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace OAuthClient
{
#if !Smartphone
    [DebuggerDisplay("{Name}:{Value}")]
#endif
#if !SILVERLIGHT && !WINRT
    [Serializable]
#endif
    public class WebParameter
    {
        public WebParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Value { get; set; }
        public string Name { get; private set; }
    }
}
