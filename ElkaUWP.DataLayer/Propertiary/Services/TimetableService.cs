using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.Infrastructure.Extensions;

namespace ElkaUWP.DataLayer.Propertiary.Services
{
    public class TimetableService
    {
        private readonly Usos.Services.TimetableService _timetableService;

        public TimetableService(Usos.Services.TimetableService timetableService)
        {
            _timetableService = timetableService;
        }

        public async Task<Uri> GetWebCalFeedAsync()
        {
            var result = await _timetableService.UpcomingShareAsync().ConfigureAwait(continueOnCapturedContext: false);

            return result != null ? new Uri(uriString: result.WebCalUrl) : null;
        }

        public async Task<List<CalendarEvent>> GetScheduleFromUsos(DateTime date)
        {
            var resultTask = _timetableService.StudentAsync(date: date);

            var activities = new List<CalendarEvent>();

            var result = await resultTask;


            if (result == null) return null;

            foreach (var classGroup2 in result)
            {
                var eventTypeToParse = classGroup2.ClasstypeName.En;
                if (eventTypeToParse.EndsWith('s'))
                    eventTypeToParse = eventTypeToParse.Remove(startIndex: eventTypeToParse.LastIndexOf('s'));

                var appointment = new CalendarEvent()
                {
                    Subject = classGroup2.CourseId.Substring(
                        startIndex: classGroup2.CourseId.LastIndexOf('-') + 1),
                    StartTime = classGroup2.StartTime,
                    EndTime = classGroup2.EndTime,
                    Location = classGroup2.RoomNumber,
                    Notes = eventTypeToParse.CapitalizeFirstCharacter(),
                    Type = (CalendarEventType) Enum.Parse(enumType: typeof(CalendarEventType), value: eventTypeToParse, ignoreCase: true),
                    Origin = Origin.Usos
                };

                activities.Add(item: appointment);
            }

            return activities;

        }
    }
}
