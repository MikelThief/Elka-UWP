using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{
    public class FloorPlan
    {
        public FloorPlan(int level, Uri imageUri)
        {
            Level = level;
            ImageUri = imageUri;
        }

        public int Level { get; set; }

        public Uri ImageUri { get; set; }
    }
}
