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

        public Corutine(IEnumerable<CorutineReport> rep)
        {
            corut = rep;
        }

        public void Cancel()
        {
            if(cancel)
                throw new Exception("already under cancellation");
            cancel = true;
        }
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

        private void ProcessReport(CorutineReport report)
        {

        }
        
    }
}
