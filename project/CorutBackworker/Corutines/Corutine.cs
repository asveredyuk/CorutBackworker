using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CorutBackworker.Corutines
{
    class Corutine
    {
        /// <summary>
        /// Ienumerable - corutine
        /// </summary>
        private IEnumerable<CorutineReport> corut;
        /// <summary>
        /// Is corutine under cancellation
        /// </summary>
        private bool cancel;
        /// <summary>
        /// Thread, running in background
        /// </summary>
        private Thread thread;
        /// <summary>
        /// List of all binders
        /// </summary>
        public List<CorutineWinformsBinder> binders;

        public Corutine(IEnumerable<CorutineReport> rep)
        {
            corut = rep;
            binders = new List<CorutineWinformsBinder>();
        }
        /// <summary>
        /// Cancel execution
        /// </summary>
        public void Cancel()
        {
            if(cancel)
                throw new Exception("already under cancellation");
            cancel = true;
        }
        /// <summary>
        /// Start execution
        /// </summary>
        public void Start()
        {
            if(thread != null)
            {
                throw new Exception("corutine was already started!");
            }
            thread = new Thread(ThreadRun);
            thread.Start();
        }
        /// <summary>
        /// Method, for running from another thread
        /// </summary>
        private void ThreadRun()
        {
            foreach (var item in corut)
            {
                //process given report
                ProcessReport(item);
                if (cancel)
                {
                    //if cancelled - report about it
                    ProcessReport(new CorutineReportCancelled());
                    //and exit
                    return;
                }
            }
        }
        /// <summary>
        /// Handle report, given from another thread
        /// </summary>
        /// <param name="report"></param>
        private void ProcessReport(CorutineReport report)
        {
            foreach (var item in binders)
            {
                item.OnReport(report);
            }
        }

        
    }
}
