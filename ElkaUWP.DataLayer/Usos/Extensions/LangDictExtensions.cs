using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Entities;

namespace ElkaUWP.DataLayer.Usos.Extensions
{
    public static class LangDictExtensions
    {
        private static string _currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        public static string GetValueForCurrentCulture(this LangDict langDict)
        {
            switch (_currentCulture)
            {
                case "pl" when !string.IsNullOrEmpty(value: langDict.Pl):
                    return langDict.Pl;
                case "en" when !string.IsNullOrEmpty(value: langDict.En):
                    return langDict.En;
                case "pl" when string.IsNullOrEmpty(value: langDict.Pl)
                               && !string.IsNullOrEmpty(value: langDict.En):
                    return langDict.En;
                case "en" when string.IsNullOrEmpty(value: langDict.En)
                               && !string.IsNullOrEmpty(value: langDict.Pl):
                    return langDict.Pl;
                default:
                    return string.Empty;
            }
        }
    }
}
