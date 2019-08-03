using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElkaUWP.DataLayer.Usos.Entities;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    [DebuggerDisplay("PartialGradeNode: NodeType={Type}, Header={Header}, Points={Points}")]
    public class PartialGradeNode
    {
        public List<PartialGradeNode> Nodes { get; set; }

        public string Header { get; set; }

        public float? Points { get; set; }

        public NodeType Type { get; set; }

        public int Id { get; set; }

        public short Order { get; set; }

        public PartialGradeNode(List<PartialGradeNode> nodes, string header, float? points, NodeType type, int id, short order)
        {
            Nodes = nodes;
            Header = header;
            Points = points;
            Type = type;
            Id = id;
            Order = order;
        }
    }
}
