using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class PartialGradesTree
    {
        public string SubjectId { get; set; }

        public string SemesterLiteral { get; set; }

        public List<PartialGradeNode> Nodes;
    }
}
