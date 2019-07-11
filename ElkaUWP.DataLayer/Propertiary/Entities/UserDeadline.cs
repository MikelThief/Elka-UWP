using System;
using LiteDB;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class UserDeadline
    {
        [BsonId]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public string Header { get; set; }

        public string Description { get; set; }

        public UserDeadline(DateTime date, string header, string description)
        {
            Date = date;
            Header = header;
            Description = description;
        }

        public UserDeadline()
        {
            
        }
    }
}
