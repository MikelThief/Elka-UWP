using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ElkaUWP.Infrastructure.Helpers
{
    public static class DatabaseConnectionStringHelper
    {
        public static string GetCachedDatabaseConnectionString()
        {
            return Path.Combine(path1: ApplicationData.Current.LocalCacheFolder.Path,
                path2: Constants.DATABASE_CACHETYPE_FILE_NAME);
        }
    }
}
