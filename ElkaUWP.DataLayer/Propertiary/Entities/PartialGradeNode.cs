using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Entities;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    [DebuggerDisplay("PartialGradeNode: Type={Type}, Description={Desciption}, Points={Points}")]
    public class PartialGradeNode
    {
        public List<PartialGradeNode> Nodes { get; set; }

        public string Desciption { get; set; }

        public float? Points { get; set; }

        public NodeType Type { get; set; }

        public int Id { get; set; }

        public short Order { get; set; }

        public string Name { get; set; }
    }
}
