using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CorutBackworker.Corutines
{
    public class CorutineProgressbarBinder : CorutineWinformsBinder
    {
        public CorutineProgressbarBinder(ProgressBar bar):base(bar)
        {
            
        }
        public override void OnReport(CorutineReport report)
        {
            if(report is CorutineReportPercentage)
            {
                //only for percentage report is capturesd
                CorutineReportPercentage percRepo = report as CorutineReportPercentage;
                ProgressBar bar = control as ProgressBar;
                bar.Invoke((MethodInvoker)delegate()
                {
                    bar.Value = percRepo.percentage;
                });
            }
        }
        public override void Init()
        {
            ProgressBar bar = control as ProgressBar;
            bar.Value = 0;
            //throw new NotImplementedException();
        }
    }
}
