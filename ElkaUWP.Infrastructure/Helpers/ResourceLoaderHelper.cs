using System;
using Windows.ApplicationModel.Resources;

namespace ElkaUWP.Infrastructure.Helpers
{
    public static class ResourceLoaderHelper
    {
        public static ResourceLoader GetResourceLoaderForView(Type loginViewType)
        {
            return ResourceLoader.GetForCurrentView(name: loginViewType.Namespace + "/Resources");
        }
    }
}
