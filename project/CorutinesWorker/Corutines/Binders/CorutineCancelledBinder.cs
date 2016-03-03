using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorutinesWorker.Corutines
{
    /// <summary>
    /// Binder for cancelled event
    /// </summary>
    public class CorutineCancelledBinder : CorutineWinformsBinder
    {
        public delegate void CancelHandler();
        public CancelHandler handler;
        public CorutineCancelledBinder(Control context, CancelHandler handle)
            : base(context)
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
            if(report is CorutineReportCancelled)
            {
                if (handler != null)
                {
                    //handle result in gui thread
                    control.Invoke((MethodInvoker)delegate
                    {
                        handler();
                    });
                }
            }
        }
    }
}
