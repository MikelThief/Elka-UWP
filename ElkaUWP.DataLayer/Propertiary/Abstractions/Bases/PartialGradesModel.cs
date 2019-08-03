using System.Collections.Generic;
using System.Diagnostics;
using ElkaUWP.DataLayer.Propertiary.Entities;
using ElkaUWP.DataLayer.Studia.Entities;

namespace ElkaUWP.DataLayer.Propertiary.Abstractions.Bases
{
    [DebuggerDisplay("Subject = {SubjectId}, Semester = {SemesterLiteral}")]
    public class PartialGradesModel
    {
        public string SubjectId { get; set; }

        public string SemesterLiteral { get; set; }

        public List<PartialGradeNode> GradeNodes { get; set; }

        public List<PartialGradeItem> GradeList { get; set; }
    }
}
