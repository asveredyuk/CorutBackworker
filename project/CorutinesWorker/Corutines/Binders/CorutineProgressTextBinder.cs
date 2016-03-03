using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorutinesWorker.Corutines
{
    public class CorutineProgressTextBinder : CorutineTextBinder
    {

        public CorutineProgressTextBinder(Control control, string _pattern = "{0}% completed", string _defVal = "") : base(control,_pattern,_defVal)
        {

        }
        public override void OnReport(CorutineReport report)
        {
            //base.OnReport(report);
            if (report is CorutineReportPercentage)
            {
                CorutineReportPercentage repoperc = report as CorutineReportPercentage;
                //Label label = control as Label;
                control.Invoke((MethodInvoker)delegate()
                {
                    control.Text = string.Format(pattern, repoperc.percentage);
                });
            }
        }
    }
}
