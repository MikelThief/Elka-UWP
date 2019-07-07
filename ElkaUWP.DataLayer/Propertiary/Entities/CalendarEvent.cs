using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using ElkaUWP.DataLayer.Propertiary.Helpers;
using ElkaUWP.DataLayer.Usos.Entities;
using LiteDB;
using Syncfusion.UI.Xaml.Schedule;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class CalendarEvent
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Subject { get; set; }

        public string Location { get; set; }

        [BsonIgnore]
        public Brush Background => CalendarEventBackgroundHelper.GetBackground(type: Type);

        public CalendarEventType Type { get; set; }

        public string Notes { get; set; }

        public Origin Origin { get; set; }
    }

    public enum Origin : short
    {
        Usos = 0,
        UserCreated = 1
    }
}
