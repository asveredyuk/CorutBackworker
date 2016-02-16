using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorutBackworker.Corutines
{
    /// <summary>
    /// Int-percentage result report
    /// </summary>
    public class CorutineReportPercentage : CorutineReport
    {
        public readonly int percentage;
        public CorutineReportPercentage(int perc)
        {
            percentage = perc;
        }
    }
}
