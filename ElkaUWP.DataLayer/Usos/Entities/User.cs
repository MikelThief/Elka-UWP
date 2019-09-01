using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Converters.Json;
using Newtonsoft.Json;

namespace ElkaUWP.DataLayer.Usos.Entities
{
    public class User
    {
        [JsonProperty(propertyName: "phone_numbers")]
        public List<string> PhoneNumbers { get; set; }

        [JsonProperty(propertyName: "pesel")]
        public string Pesel { get; set; }

        [JsonProperty(propertyName: "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(propertyName: "birth_date")]
        public DateTimeOffset? BirthDate { get; set; }

        [JsonProperty(propertyName: "photo_urls")]
        public PhotoUrls PhotoUrls { get; set; }

        [JsonProperty(propertyName: "email")]
        public string Email { get; set; }

        [JsonProperty(propertyName: "email_access")]
        public string EmailAccess { get; set; }

        [JsonProperty(propertyName: "has_email")]
        public bool HasEmail { get; set; }

        [JsonProperty(propertyName: "id")]
        public int Id { get; set; }

        [JsonProperty(propertyName: "student_status")]
        [JsonConverter(converterType: typeof(StudentStatusConverter))]
        public StudentStatus StudentStatus { get; set; }

        [JsonProperty(propertyName: "student_number")]
        public int StudentNumber { get; set; }

        [JsonProperty(propertyName: "middle_names")]
        public string MiddleNames { get; set; }

        [JsonProperty(propertyName: "profile_url")]
        public Uri ProfileUrl { get; set; }

        [JsonProperty(propertyName: "postal_addresses")]
        public List<object> PostalAddresses { get; set; }

        [JsonProperty(propertyName: "titles")]
        public Titles Titles { get; set; }

        [JsonProperty(propertyName: "staff_status")]
        [JsonConverter(converterType: typeof(StaffStatusConverter))]
        public StaffStatus StaffStatus { get; set; }

        [JsonProperty(propertyName: "last_name")]
        public string LastName { get; set; }

        [JsonProperty(propertyName: "homepage_url")]
        public string HomepageUrl { get; set; }

        [JsonProperty(propertyName: "room")]
        public object Room { get; set; }

        [JsonProperty(propertyName: "mobile_numbers")]
        public List<string> MobileNumbers { get; set; }

        [JsonProperty(propertyName: "student_programmes")]
        public List<StudentProgramme> StudentProgrammes { get; set; }

        [JsonProperty(propertyName: "employment_positions")]
        public List<EmploymentPosition> EmploymentPositions { get; set; }

        [JsonProperty(propertyName: "office_hours")]
        public LangDict OfficeHours { get; set; }
    }

    public enum StudentStatus : sbyte
    {
        None = -1, // Not allowed to access the student status of this user
        NotAStudent = 0, // The user is not, and never was, a student on any of the study programmes
        InactiveStudent = 1, // The user is an "inactive student".
                             // That is, they are not studying, but they were an active student in the past.
        ActiveStudent = 2 // The user is an active student
    }

    public enum StaffStatus : sbyte
    {
        InactiveOrNotAStaff = 0, // The user is currently not a staff member. They might have been a staff member in the past.
        ActiveStaff = 1, // The user is an employed staff member, but he is not an academic teacher.
        ActiveTeacher = 2 // The user is an active academic teacher.
    }
}
