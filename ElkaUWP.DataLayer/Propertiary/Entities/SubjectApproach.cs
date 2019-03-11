using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class SubjectApproach
    {
        public string SemesterLiteral { get; set; }

        public string Id { get; set; }

        public string Acronym => Id.Substring(startIndex: Id.LastIndexOf('-') + 1);

        public string GradeLiteral { get; set; }

        public bool IsPassed { get; set; }

        public HashSet<string> StaffHashSet { get; set; }

        public string Name { get; set; }
    }
}
