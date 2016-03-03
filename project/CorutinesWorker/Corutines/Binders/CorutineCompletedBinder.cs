using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorutinesWorker.Corutines
{

    /// <summary>
    /// Binder for completed event
    /// </summary>
    public class CorutineCompletedBinder : CorutineWinformsBinder
    {
        public delegate void CompletedHandler(object result);
        public CompletedHandler handler;
        public CorutineCompletedBinder(Control context, CompletedHandler handle):base(context)
        {
            handler = handle;
        }
        public override void Init()
        {
            //throw new NotImplementedException();
        }
        public override void OnReport(CorutineReport report)
        {
            //throw new NotImplementedException();
            if(report is CorutineReportResult)
            {
                CorutineReportResult res = report as CorutineReportResult;
                if (handler != null)
                {
                    //handle result in gui thread
                    control.Invoke((MethodInvoker)delegate
                    {
                        handler(res.result);
                    });
                }
            }
        }

    }
}
