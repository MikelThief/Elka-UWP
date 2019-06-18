using System;
using Windows.ApplicationModel.Resources;

namespace ElkaUWP.Infrastructure.Helpers
{
    public static class ResourceLoaderHelper
    {
        public static ResourceLoader GetResourceLoaderForView(Type viewType)
            => ResourceLoader.GetForCurrentView(name: viewType.Namespace + "/Resources");
    }
}
