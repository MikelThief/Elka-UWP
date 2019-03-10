using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class FinishedSubjectApproach : InProgressSubjectApproach
    {
        public string GradeLiteral { get; set; }

        public bool IsPassed { get; set; }
    }
}
