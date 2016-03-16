using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorutinesWorker.Corutines
{
    /// <summary>
    /// Int-percentage result report
    /// </summary>
    public sealed class CorutineReportPercentage : CorutineReport
    {
        public readonly int percentage;
        public CorutineReportPercentage(int perc)
        {
            percentage = perc;
        }
        public CorutineReportPercentage(int done, int total)
        {
            percentage = done * 100 / total;
        }
        
    }
}
