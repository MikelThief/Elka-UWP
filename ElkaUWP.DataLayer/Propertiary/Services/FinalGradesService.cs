using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Propertiary.Converters;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Usos.Abstractions.Bases;
using ElkaUWP.DataLayer.Usos.Entities;
using ElkaUWP.DataLayer.Usos.Services;

namespace ElkaUWP.DataLayer.Propertiary.Services
{
    public class FinalGradesService
    {
        private GradesService _gradesService;
        private CoursesService _coursesService;

        public FinalGradesService(GradesService gradesService, CoursesService coursesService)
        {
            _gradesService = gradesService;
            _coursesService = coursesService;
        }

        public async Task<IEnumerable<SubjectApproach>> GetAllAsync()
        {
            var result = new List<SubjectApproach>();

            var coursesPerSemesterDictionary = await _coursesService.UserAsync().ConfigureAwait(continueOnCapturedContext: false);
            var gradedSubjectsPerSemesterDictionary = await _gradesService.Terms2Async().ConfigureAwait(continueOnCapturedContext: false);

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

                            if (string.IsNullOrEmpty(value: gradeLiteral) &&
                                coursesPerSemesterDictionaryKey != highestSemesterAsString)
                                gradeLiteral = "2";

                        }
                    }

                    if (string.IsNullOrEmpty(value: gradeLiteral) &&
                             coursesPerSemesterDictionaryKey == highestSemesterAsString)
                        gradeLiteral = "⏳";

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

                    result.Add(item: tempSubject);
                }
            }
            return result;
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
    }
}
