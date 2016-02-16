using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorutBackworker.Corutines
{
    /// <summary>
    /// Class to report the result of algorithm
    /// </summary>
    public class CorutineReportResult : CorutineReport
    {
        public readonly object result;
        public CorutineReportResult(object obj)
        {
            result = obj;
        }
    }
}
