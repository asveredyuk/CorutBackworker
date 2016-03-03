using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorutinesWorker.Corutines
{
    /// <summary>
    /// Corutine report for labels
    /// </summary>
    public class CorutineReportText:CorutineReport
    {
        public readonly string message;
        public CorutineReportText(string msg)
        {
            message = msg;
        }
    }
}
