using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using ElkaUWP.DataLayer.Propertiary.Helpers;
using LiteDB;
using Syncfusion.UI.Xaml.Schedule;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class CalendarEvent
    {
        [BsonId]
        public Guid Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Subject { get; set; }

        public string Location { get; set; }

        [BsonIgnore]
        public Brush Background => CalendarEventBackgroundHelper.GetBackground(type: CalendarEventType);

        public CalendarEventType CalendarEventType { get; set; }

        public string Notes { get; set; }

        public Origin Origin { get; set; }

        public CalendarEvent(DateTime startTime, DateTime endTime, string subject, string location,
            CalendarEventType calendarEventType, string notes, Origin origin)
        {
            StartTime = startTime;
            EndTime = endTime;
            Subject = subject;
            Location = location;
            CalendarEventType = calendarEventType;
            Notes = notes;
            Origin = origin;
        }

        public CalendarEvent()
        {
            
        }
    }

    public enum Origin : short
    {
        Usos = 0,
        UserCreated = 1
    }

    public enum CalendarEventType
    {
        Lecture,
        Laboratory,
        Tutorial,
        Seminar,
        Project,
        Conversatory,
        Other,
        Unspecified
    }
}
