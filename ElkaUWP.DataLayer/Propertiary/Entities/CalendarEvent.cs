using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using ElkaUWP.DataLayer.Usos.Entities;
using Syncfusion.UI.Xaml.Schedule;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class CalendarEvent
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public Brush Background { get; set; }
        public CalendarEventType Type { get; set; }
        public string Notes { get; set; }
    }
}
