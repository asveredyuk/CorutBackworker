using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorutinesWorker.Corutines
{
    public class CorutineTextBinder : CorutineWinformsBinder
    {
        protected readonly string pattern;
        protected readonly string defaultValue;
        public CorutineTextBinder(Control control, string _pattern = "{0}", string _defVal = "") : base(control)
        {
            pattern = _pattern;
            defaultValue = _defVal;
        }
        public override void Init()
        {
            control.Text = string.Format(pattern, defaultValue);
        }
        public override void OnReport(CorutineReport report)
        {
            if(report is CorutineReportText)
            {
                CorutineReportText repotext = report as CorutineReportText;
                //Label label = control as Label;
                control.Invoke((MethodInvoker)delegate()
                {
                    control.Text = string.Format(pattern, repotext.message);
                });
            }
        }
    }
}
