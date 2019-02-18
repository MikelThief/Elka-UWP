using System;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Helpers;
using ElkaUWP.Infrastructure.Extensions;
using Syncfusion.UI.Xaml.Schedule;

namespace ElkaUWP.DataLayer.Usos.Extensions
{
    public static class ClassGroup2Extensions
    {
        public static CalendarEvent AsScheduleAppointment(this ClassGroup2 usosScheduleItem)
        {
            var appointment = new CalendarEvent()
            {
                Subject = usosScheduleItem.CourseId.Substring(startIndex: usosScheduleItem.CourseId.LastIndexOf('-') + 1),
                StartTime = usosScheduleItem.StartTime,
                EndTime = usosScheduleItem.EndTime,
            };

            var eventTypeToParse = usosScheduleItem.ClasstypeName.En;

            if(eventTypeToParse.EndsWith('s'))
                eventTypeToParse = eventTypeToParse.Remove(startIndex: eventTypeToParse.LastIndexOf('s'));

            appointment.Notes = eventTypeToParse.CapitalizeFirstCharacter();

            appointment.Background =
                CalendarEventBackgroundHelper.GetBackgroundFromEventType(type: (CalendarEventType) Enum.Parse(enumType: typeof(CalendarEventType), value: eventTypeToParse, ignoreCase: true));

            appointment.IsRecursive = true;
            var recurrenceProperties = new RecurrenceProperties();
            recurrenceProperties.IsDailyEveryNDays = true;
            recurrenceProperties.DailyNDays = 14;
            recurrenceProperties.RecurrenceRule = ScheduleHelper.RRuleGenerator(RecProp: recurrenceProperties, AppStartTime: appointment.StartTime, AppEndTime: appointment.EndTime);
            appointment.RecurrenceRule = recurrenceProperties.RecurrenceRule;

            return appointment;
        }
    }
}
