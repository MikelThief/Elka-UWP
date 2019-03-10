using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anotar.NLog;

namespace ElkaUWP.DataLayer.Propertiary.Converters
{
    /// <summary>
    /// Convertes between string representation of the semester id and integer value
    /// <example>
    /// 2019Z -> 20190
    /// 20191 -> 2019L
    /// </example>  
    /// </summary>
    public class SemesterLiteralAndShortConverter
    {
        public string ShortToString(short semesterShort)
        {
            var stringified = semesterShort.ToString();

            try
            {
                if (stringified.EndsWith('0'))
                    return stringified.Substring(startIndex: 0, length: 4) + "L";
                else if(stringified.EndsWith('1'))
                    return stringified.Substring(startIndex: 0, length: 4) + "Z";
                else throw new ArgumentOutOfRangeException(paramName: nameof(semesterShort), message: nameof(semesterShort) + " doesn't end with either 0 or 1");
            }
            catch (ArgumentOutOfRangeException aoorexc)
            {
                LogTo.ErrorException(message: "Failed to convert " + nameof(semesterShort) + " to string representation", exception: aoorexc);
                throw;
            }
            // 
        }

        public short LiteralToShort(string semesterLiteral)
        {
            string toParse = default;
            try
            {
                if (semesterLiteral.EndsWith(value: 'L'))
                    toParse = semesterLiteral.Substring(startIndex: 0, length: 4) + "0";
                else if (semesterLiteral.EndsWith(value: 'Z'))
                    toParse = semesterLiteral.Substring(startIndex: 0, length: 4) + "1";
                else throw new ArgumentOutOfRangeException(paramName: nameof(semesterLiteral), message: nameof(semesterLiteral) + " doesn't end with either Z or L");
            }
            catch (ArgumentOutOfRangeException aoorexc)
            {
                LogTo.ErrorException(message: "Failed to convert " + nameof(semesterLiteral) + " to short-primitive representation", exception: aoorexc);
                throw;
            }
            // expected results are XXXX0 for summer semester and XXXX1 for winter semester
            return short.Parse(s: toParse);
        }
    }
}
