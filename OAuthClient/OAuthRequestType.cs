using System;
using System.Collections.Generic;
using System.Text;

namespace OAuthClient
{
    /// <summary>
    /// The types of OAuth requests possible in a typical workflow.
    /// Used for validation purposes and to build static helpers.
    /// </summary>
    public enum OAuthRequestType
    {
        RequestToken,
        AccessToken,
        ProtectedResource,
    }
}
