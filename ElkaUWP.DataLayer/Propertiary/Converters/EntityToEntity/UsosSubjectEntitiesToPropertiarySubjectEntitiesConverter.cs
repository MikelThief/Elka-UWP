using System.Collections.Generic;
using Windows.UI;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;

namespace ElkaUWP.DataLayer.Propertiary.Converters.EntityToEntity
{
    public class UsosSubjectEntitiesToPropertiarySubjectEntitiesConverter
    {
        public List<SubjectApproach> InProgressSubjects { get; private set; }
        public List<SubjectApproach> FinishedSubjects { get; private set; }

        public UsosSubjectEntitiesToPropertiarySubjectEntitiesConverter()
        {
            InProgressSubjects = new List<SubjectApproach>();
            FinishedSubjects = new List<SubjectApproach>();
        }

        public void Convert(Dictionary<string, Dictionary<string, GradesGradedSubject>> gradedSubjectsPerSemesterDictionary,
            Dictionary<string, List<CourseEdition>> coursesPerSemesterDictionary)
        {
            var semesterLiteralAndShortConverter = new SemesterLiteralAndShortConverter();

            var highestSemester = FindHighestSemester(coursesPerSemesterDictionary: coursesPerSemesterDictionary);
            var highestSemesterAsString =
                semesterLiteralAndShortConverter.ShortToString(semesterShort: highestSemester);

            foreach (var coursesPerSemesterDictionaryKey in coursesPerSemesterDictionary.Keys)
            {
                var coursesPerSemester = coursesPerSemesterDictionary[key: coursesPerSemesterDictionaryKey];

                foreach (var courseEdition in coursesPerSemester)
                {
                    string gradeLiteral = default;
                    bool isPassed = false;

                    if (gradedSubjectsPerSemesterDictionary.ContainsKey(key: coursesPerSemesterDictionaryKey))
                    {
                        var gradedSemester = gradedSubjectsPerSemesterDictionary[key: coursesPerSemesterDictionaryKey];

                        if (gradedSemester.ContainsKey(key: courseEdition.CourseId))
                        {
                            gradeLiteral = gradedSubjectsPerSemesterDictionary[key: coursesPerSemesterDictionaryKey]
                                [key: courseEdition.CourseId]
                                .CourseGrades[0].Sub1?.ValueSymbol;

                            if (gradedSubjectsPerSemesterDictionary[key: coursesPerSemesterDictionaryKey]
                                    [key: courseEdition.CourseId].CourseGrades[0].Sub1?.Passes != null)
                                isPassed =
                                    gradedSubjectsPerSemesterDictionary[key: coursesPerSemesterDictionaryKey]
                                        [key: courseEdition.CourseId].CourseGrades[0].Sub1.Passes;
                        }
                    }

                    var staffHashSet = new HashSet<string>();

                    foreach (var coordinator in courseEdition.Coordinators)
                    {
                        staffHashSet.Add(item: coordinator.FirstName + " " + coordinator.LastName); 
                    }

                    foreach (var lecturer in courseEdition.Lecturers)
                    {
                        staffHashSet.Add(item: lecturer.FirstName + " " + lecturer.LastName);
                    }

                    var tempSubject = new SubjectApproach()
                    {
                        SemesterLiteral = courseEdition.TermId,
                        Id = courseEdition.CourseId,
                        GradeLiteral = gradeLiteral,
                        IsPassed = isPassed,
                        // English subject's doesn't have polish translation (so far...) so Polish is fine...
                        Name = courseEdition.CourseName.Pl,
                        StaffHashSet = staffHashSet
                    };

                    if(courseEdition.TermId == highestSemesterAsString)
                        InProgressSubjects.Add(item: tempSubject);
                    else 
                        FinishedSubjects.Add(item: tempSubject);

                }
            }
            
        }

        private short FindHighestSemester(Dictionary<string, List<CourseEdition>> coursesPerSemesterDictionary)
        {
            var semesterLiteralAndShortConverter = new SemesterLiteralAndShortConverter();
            short highestSemester = default;
            foreach (var semester in coursesPerSemesterDictionary.Keys)
            {
                var literalAsShort = semesterLiteralAndShortConverter.LiteralToShort(semesterLiteral: semester);
                if (literalAsShort > highestSemester)
                    highestSemester = literalAsShort;
            }

            return highestSemester;
        }

        public void Flush()
        {
            InProgressSubjects.Clear();
            FinishedSubjects.Clear();
        }
    }
}
