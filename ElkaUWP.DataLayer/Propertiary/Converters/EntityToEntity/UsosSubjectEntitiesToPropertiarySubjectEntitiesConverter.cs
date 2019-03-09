using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;

namespace ElkaUWP.DataLayer.Propertiary.Converters.EntityToEntity
{
    public class UsosSubjectEntitiesToPropertiarySubjectEntitiesConverter
    {
        public List<Subject> InProgressSubjects { get; private set; }
        public List<Subject> FailedSubjects { get; private set; }
        public List<Subject> PassedSubjects { get; private set; }

        public void Convert(Dictionary<string, Dictionary<string, List<GradedSemester>>> gradedSemestersDictionary,
            Dictionary<string, List<CourseEdition>> coursesPerSemesterDictionary)
        {

        }
    }
}
