using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.Infrastructure.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            DayOfWeek firstDay = defaultCultureInfo.DateTimeFormat.FirstDayOfWeek;
            return GetFirstDateOfWeek(dayInWeek, firstDay);
        }

        public static DateTime GetFirstDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }
    }
}
