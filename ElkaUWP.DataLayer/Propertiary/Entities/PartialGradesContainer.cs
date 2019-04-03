using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class PartialGradesContainer
    {
        public PartialGradesTree UsosTree { get; set; }

        public string SubjectId { get; set; }

        public string SemesterLiteral { get; set; }
    }
}
