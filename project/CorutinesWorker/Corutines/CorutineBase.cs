﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CorutinesWorker.Corutines
{
    /// <summary>
    /// Base class of corutine
    /// </summary>
    public abstract class CorutineBase : ICorutine
    {
        /// <summary>
        /// Ienumerable - corutine
        /// </summary>
        private IEnumerable<CorutineReport> corut;
        /// <summary>
        /// Is corutine under cancellation
        /// </summary>
        protected bool cancel;
        /// <summary>
        /// Thread, running in background
        /// </summary>
        private Thread thread;
        /// <summary>
        /// List of all binders
        /// </summary>
        public List<ICorutineBinder> binders;
        /// <summary>
        /// Is corutine now active
        /// </summary>
        public virtual bool IsWorking
        {
            get
            {
                return thread != null && thread.IsAlive;
            }
        }

        public virtual bool HasStarted
        {
            get { return thread != null; }
        }

        protected void SetIenumerable(IEnumerable<CorutineReport> rep)
        {
            if(HasStarted)
                throw new Exception("corutine has already started");
            corut = rep;
        }
        protected CorutineBase()
        {
            binders = new List<ICorutineBinder>();
        }
        protected CorutineBase(IEnumerable<CorutineReport> rep):this()
        {
            corut = rep;
        }
        /// <summary>
        /// Cancel execution
        /// </summary>
        public virtual void Cancel()
        {
            if(cancel)
                throw new Exception("already under cancellation");
            cancel = true;
        }
        /// <summary>
        /// Start execution
        /// </summary>
        public virtual void Start()
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
        protected virtual void ThreadRun()
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
        protected virtual void ProcessReport(CorutineReport report)
        {
            foreach (var item in binders)
            {
                item.OnReport(report);
            }
        }

        
    }
}
