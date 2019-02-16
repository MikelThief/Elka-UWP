using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    [Equals]
    public class UserDeadline
    {
        public DateTime Date { get; private set; }

        public string Header { get; private set; }

        public string Description { get; private set; }

        [IgnoreDuringEquals]
        public Guid Id { get; private set; }

        public UserDeadline(DateTime date, string header, string description)
        {
            Date = date;
            Header = header;
            Description = description;
            Id = new Guid();
        }
    }
}
