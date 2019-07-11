using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;
using Type = ElkaUWP.DataLayer.Usos.Entities;

namespace ElkaUWP.DataLayer.Propertiary.Helpers
{
    public static class CalendarEventBackgroundHelper
    {
        public static SolidColorBrush GetBackground(CalendarEventType type)
        {
            switch (type)
            {
                case CalendarEventType.Lecture:
                    return new SolidColorBrush(color: Colors.Green);
                case CalendarEventType.Laboratory:
                    return new SolidColorBrush(color: Colors.Crimson);
                case CalendarEventType.Tutorial:
                    return new SolidColorBrush(color: Colors.Goldenrod);
                case CalendarEventType.Seminar:
                    return new SolidColorBrush(color: Colors.LightBlue);
                case CalendarEventType.Project:
                    return new SolidColorBrush(color: Colors.Green);
                case CalendarEventType.Conversatory:
                    return new SolidColorBrush(color: Colors.Green);
                case CalendarEventType.Other:
                    return new SolidColorBrush(color: Colors.Purple);
                case CalendarEventType.Unspecified:
                    return new SolidColorBrush(color: Colors.LightGray);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
