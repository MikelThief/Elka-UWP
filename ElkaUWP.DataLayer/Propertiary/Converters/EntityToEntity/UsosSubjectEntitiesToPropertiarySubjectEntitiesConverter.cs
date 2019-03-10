using System.Collections.Generic;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Entities;

namespace ElkaUWP.DataLayer.Propertiary.Converters.EntityToEntity
{
    public class UsosSubjectEntitiesToPropertiarySubjectEntitiesConverter
    {
        public List<Subject> InProgressSubjects { get; private set; }
        public List<Subject> FailedSubjects { get; private set; }
        public List<Subject> PassedSubjects { get; private set; }

        public void Convert(Dictionary<string, Dictionary<string, List<ExamRepGradedSubject>>> gradedSubjectsPerSemesterDictionary,
            Dictionary<string, List<CourseEdition>> coursesPerSemesterDictionary)
        {
            var semesterLiteralAndShortConverter = new SemesterLiteralAndShortConverter();

            var highestSemester = FindHighestSemester(coursesPerSemesterDictionary: coursesPerSemesterDictionary);
            var highestSemesterAsString =
                semesterLiteralAndShortConverter.ShortToString(semesterShort: highestSemester);

            foreach (var semester in coursesPerSemesterDictionary.Keys)
            {
                var coursesInSemester = coursesPerSemesterDictionary[key: semester];

                foreach (var course in coursesInSemester)
                {
                    foreach (var gradedSemesterKey in gradedSubjectsPerSemesterDictionary.Keys)
                    {
                        var gradedSemester = gradedSubjectsPerSemesterDictionary[key: gradedSemesterKey];

                        if(gradedSemester.ContainsKey(key: course.CourseId))
                        {
                            // path taken when subject was approached more than once or is failed/passed
                            var pastSubjectTrial = gradedSemester[course.CourseId];

                            // EiTI has list of passing sections defined as a single element
                            // This must be revised if EiTI decidec to follow. for instance, WAT implementation - multiple passing sections
                            // Serio USOS, nie dało się tej organizacji dnaych bardziej spierdolić?
                            var pastSessions = pastSubjectTrial[0].Sessions;

                            foreach (var pastSession in pastSessions)
                            {

                            }


                        }
                        else
                        {
                            // path taken when subject was approached once
                            var subject = new Subject
                            {
                                FullName = course.CourseName.Pl,
                                ShortName = course.CourseId,
                                SemesterLiteral = course.TermId,
                                GradeLiteral = ""
                            };

                            // Subject is currently being studied
                            if(subject.SemesterLiteral == highestSemesterAsString)
                                InProgressSubjects.Add(item: subject);
                        }
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
            InProgressSubjects = null;
            FailedSubjects = null;
            PassedSubjects = null;
        }
    }
}
