using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anotar.NLog;

namespace ElkaUWP.DataLayer.Propertiary.Entities
{

    public class Subject
    {
        public string SemesterLiteral { get; set; }

        public short SemesterID
        {
            get
            {
                string toParse = default;
                try
                {
                    if (SemesterLiteral.EndsWith(value: 'L'))
                        toParse = SemesterLiteral.Substring(startIndex: 0, length: 4) + "0";
                    else if(SemesterLiteral.EndsWith(value: 'Z'))
                        toParse = SemesterLiteral.Substring(startIndex: 0, length: 4) + "1";
                    else throw new ArgumentOutOfRangeException(paramName: nameof(SemesterLiteral), message: nameof(SemesterLiteral) + " doesn't end with either Z or L");
                }
                catch (ArgumentOutOfRangeException aoorexc)
                {
                    LogTo.FatalException(message: "Failed to convert " + nameof(SemesterLiteral) + " to short-primitive representation", exception: aoorexc);
                    throw;
                }
                // expected results are XXXX0 for summer semester and XXXX1 for winter semester
                return short.Parse(s: toParse);
            }
        }

        public string FullName { get; set; }

        public string ShortName { get; set; }

        public string GradeLiteral { get; set; }
    }
}
 