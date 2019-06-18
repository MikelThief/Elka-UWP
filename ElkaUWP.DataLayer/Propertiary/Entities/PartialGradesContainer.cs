using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    [DebuggerDisplay("Subject = {SubjectId}, Semester = {SemesterLiteral}")]
    public class PartialGradesContainer
    {
        public List<PartialGradeNode> Nodes { get; set; }

        public string SubjectId { get; set; }

        public string SemesterLiteral { get; set; }

        public Dictionary<string, string> StudiaDictionary { get; set; }
    }
}
