using System.Collections.Generic;
using Windows.UI;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;

namespace ElkaUWP.DataLayer.Propertiary.Converters.EntityToEntity
{
    public class UsosSubjectEntitiesToPropertiarySubjectEntitiesConverter
    {
        public List<InProgressSubjectApproach> InProgressSubjects { get; private set; }
        public List<FinishedSubjectApproach> PassedOrFailedSubjects { get; private set; }

        public UsosSubjectEntitiesToPropertiarySubjectEntitiesConverter()
        {
            InProgressSubjects = new List<InProgressSubjectApproach>();
            PassedOrFailedSubjects = new List<FinishedSubjectApproach>();
        }

        public void Convert(Dictionary<string, Dictionary<string, GradesGradedSubject>> gradedSubjectsPerSemesterDictionary,
            Dictionary<string, List<CourseEdition>> coursesPerSemesterDictionary)
        {
            var semesterLiteralAndShortConverter = new SemesterLiteralAndShortConverter();

            var highestSemester = FindHighestSemester(coursesPerSemesterDictionary: coursesPerSemesterDictionary);
            var highestSemesterAsString =
                semesterLiteralAndShortConverter.ShortToString(semesterShort: highestSemester);

            // Process currently studied subjects
            foreach (var courseEdition in coursesPerSemesterDictionary[key: highestSemesterAsString])
            {
                var tempSubject = new InProgressSubjectApproach()
                {
                    SemesterLiteral = courseEdition.TermId,
                    ShortName = courseEdition.CourseId,
                };
                InProgressSubjects.Add(item: tempSubject);
            }
            
            // Process rest of subjects
            foreach (var gradedSemesterKey in gradedSubjectsPerSemesterDictionary.Keys)
            {
                var gradedSemester = gradedSubjectsPerSemesterDictionary[key: gradedSemesterKey];

                foreach (var gradedSubjectKey in gradedSemester.Keys)
                {
                    var gradedSubject = gradedSemester[key: gradedSubjectKey];

                    var gradedElement = gradedSubject.CourseGrades[0].Sub1;

                    // sometimes failed subject appears as the subject without grade.
                    // USOS allows for that and treats it internally as failed... I think
                    if (gradedElement is null)
                    {
                        var tempSubject = new FinishedSubjectApproach
                        {
                            GradeLiteral = "2",
                            IsPassed = false,
                            SemesterLiteral = gradedSemesterKey,
                            ShortName = gradedSubjectKey
                        };
                        PassedOrFailedSubjects.Add(item: tempSubject);
                    }
                    else
                    {
                        var tempSubject = new FinishedSubjectApproach
                        {
                            GradeLiteral = gradedElement.ValueSymbol,
                            IsPassed = gradedElement.Passes,
                            SemesterLiteral = gradedSemesterKey,
                            ShortName = gradedSubjectKey
                        };
                        PassedOrFailedSubjects.Add(item: tempSubject);
                    }
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
            PassedOrFailedSubjects.Clear();
        }
    }
}
