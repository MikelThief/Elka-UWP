using System;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class UserDeadline
    {
        public DateTime Date { get; private set; }

        public string Header { get; private set; }

        public string Description { get; private set; }

        public UserDeadline(DateTime date, string header, string description)
        {
            Date = date;
            Header = header;
            Description = description;
        }
    }
}
