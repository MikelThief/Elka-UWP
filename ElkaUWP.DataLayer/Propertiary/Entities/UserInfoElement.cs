using System;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    [Equals]
    public class UserInfoElement
    {
       

        public string Header { get; private set; }

        public string Description { get; private set; }

        public UserInfoElement(DateTime date, string header, string description)
        {
           
            Header = header;
            Description = description;
        }
    }
}