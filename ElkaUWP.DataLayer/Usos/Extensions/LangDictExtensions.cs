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
        private static readonly string _currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        public static string GetValueForCurrentCulture(this LangDict langDict, string fallbackValue)
        {
            switch (_currentCulture)
            {
                case "pl" when !string.IsNullOrEmpty(value: langDict.Pl):
                    return langDict.Pl;
                case "en" when !string.IsNullOrEmpty(value: langDict.En):
                    return langDict.En;
                case "pl" when string.IsNullOrEmpty(value: langDict.Pl)
                               && !string.IsNullOrEmpty(value: langDict.En):
                    return langDict.En + " (po angielsku)";
                case "en" when string.IsNullOrEmpty(value: langDict.En)
                               && !string.IsNullOrEmpty(value: langDict.Pl):
                    return langDict.Pl + " (in polish)";
                default:
                    return fallbackValue;
            }
        }
    }
}
