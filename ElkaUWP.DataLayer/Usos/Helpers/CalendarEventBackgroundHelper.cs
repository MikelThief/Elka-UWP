using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using ElkaUWP.DataLayer.Usos.Entities;

namespace ElkaUWP.DataLayer.Usos.Helpers
{
    public static class CalendarEventBackgroundHelper
    {
        public static SolidColorBrush GetBackgroundFromEventType(CalendarEventType type)
        {
            switch (type)
            {
                case CalendarEventType.Lecture:
                    return new SolidColorBrush(color: Colors.Green);
                    break;
                case CalendarEventType.Laboratory:
                    return new SolidColorBrush(color: Colors.Red);
                    break;
                case CalendarEventType.Tutorial:
                    return new SolidColorBrush(color: Colors.Yellow);
                    break;
                case CalendarEventType.Seminar:
                    return new SolidColorBrush(color: Colors.Purple);
                    break;
                case CalendarEventType.Project:
                    return new SolidColorBrush(color: Colors.Green);
                    break;
                case CalendarEventType.Conversatory:
                    return new SolidColorBrush(color: Colors.Green);
                    break;
                case CalendarEventType.Other:
                    return new SolidColorBrush(color: Colors.LightBlue);
                    break;
                case CalendarEventType.Unspecified:
                    return new SolidColorBrush(color: Colors.LightGray);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
