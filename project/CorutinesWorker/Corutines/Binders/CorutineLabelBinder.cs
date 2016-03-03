using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorutBackworker.Corutines
{
    public class CorutineLabelBinder : CorutineWinformsBinder
    {
        readonly string pattern;
        readonly string defaultValue;
        public CorutineLabelBinder(Label label, string _pattern = "{0}", string _defVal = "") : base(label)
        {
            pattern = _pattern;
            defaultValue = _defVal;
        }
        public override void Init()
        {
            Label label = control as Label;
            label.Text = string.Format(pattern, defaultValue);
        }
        public override void OnReport(CorutineReport report)
        {
            if(report is CorutineReportText)
            {
                CorutineReportText repotext = report as CorutineReportText;
                Label label = control as Label;
                label.Invoke((MethodInvoker)delegate()
                {
                    label.Text = string.Format(pattern, repotext.message);
                });
            }
        }
    }
}
