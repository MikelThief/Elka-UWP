using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class UserMatch
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public string EmploymentPosition { get; set; }

        public int Id { get; set; }

        public string HtmlMatchedName { get; set; }

        public UserMatch(string firstName, string lastName, string title, string employmentPosition, int id, string htmlMatchedName)
        {
            FirstName = firstName;
            LastName = lastName;
            Title = title;
            EmploymentPosition = employmentPosition;
            Id = id;
            HtmlMatchedName = htmlMatchedName;
        }
    }
}
