using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CorutinesWorker.Corutines
{
    /// <summary>
    /// Multithreaded corutine
    /// </summary>
    public abstract class CorutineMultithreadedBase : CorutineBase
    {
        /// <summary>
        /// List of all corutines (each per thread)
        /// </summary>
        protected IEnumerable<CorutineReport>[] corutines;
        /// <summary>
        /// Container for returned values
        /// </summary>
        protected ConcurrentQueue<CorutineReport> reportQueue;
        /// <summary>
        /// List of percentages for each thread
        /// </summary>
        protected int[] percentages;
        /// <summary>
        /// List of threads-slaves
        /// </summary>
        protected Thread[] threads;
        /// <summary>
        /// List of results, returned by thread
        /// </summary>
        protected List<CorutineReportResult> results;
        /// <summary>
        /// Create multithreaded corutine
        /// </summary>
        /// <param name="reports">List of iterables one per thread</param>
        protected CorutineMultithreadedBase(IEnumerable<CorutineReport>[] reports)
        {
            corutines = reports;
            reportQueue = new ConcurrentQueue<CorutineReport>();
            threads = new Thread[reports.Length];
            results = new List<CorutineReportResult>();
            percentages = new int[reports.Length];
        }

        public override void Start()
        {
            SetIenumerable(ThreadMasterRun());                              //replace with our own ienumerable
            base.Start();
        }
        /// <summary>
        /// Method for running in background master-threads
        /// </summary>
        /// <returns></returns>
        private IEnumerable<CorutineReport> ThreadMasterRun()
        {
            StartThreadsSlaves();
            while (true)
            {
                if (CheckThreadsEnded())
                    break;                                                  //end
                while (reportQueue.Count == 0) ;                            //wait for message in queue
                CorutineReport report = null;
                if (!reportQueue.TryDequeue(out report))
                {
                    continue;
                }
                if (report is ThreadEndedReport)
                {
                    ThreadEndedReport r = report as ThreadEndedReport;
                    threads[r.threadId] = null;                             //mark as ended
                    continue;                                               //go to the next loop
                }
                if (report is ThreadProgressReport)
                {
                    ThreadProgressReport r = report as ThreadProgressReport;
                    percentages[r.threadId] = r.perc.percentage;            //update progress for current thread
                    yield return GetProgressReport();                       //and pass total progress result
                }
                if (report is CorutineReportResult)
                {
                    results.Add(report as CorutineReportResult);
                    continue;
                }
                yield return report;                                        //for all other report we process the as usual
            }
            yield return new CorutineReportResult(results);                 //return list of results
        }
        /// <summary>
        /// Create new combined progress report
        /// </summary>
        /// <returns></returns>
        protected CorutineReportPercentage GetProgressReport()
        {
            return new CorutineReportPercentage(percentages.Sum()/percentages.Length);
        }
        /// <summary>
        /// Check if all thread has ended
        /// </summary>
        /// <returns></returns>
        private bool CheckThreadsEnded()
        {
            foreach (var thread1 in threads)
            {
                if (thread1 != null)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Start threads-slaves
        /// </summary>
        private void StartThreadsSlaves()
        {
            for (int i = 0; i < threads.Length; i++)
            {
                Thread t = new Thread(ThreadSlaveRun);
                threads[i] = t;
                t.Start(i);
            }
        }
        /// <summary>
        /// Method for running in threads-slaves
        /// </summary>
        /// <param name="threadId">id of thread</param>
        private void ThreadSlaveRun(object threadId)
        {
            int i = (int) threadId;
            IEnumerable<CorutineReport> cor = corutines[i];
            foreach (var ite in cor)
            {
                CorutineReport item = ite;
                //if this is progress report, we need to convert it
                if(item is CorutineReportPercentage)
                    item = new ThreadProgressReport(i,ite as CorutineReportPercentage);         
                //process report
                ProcessReportSlave(item);
                if (cancel)
                {
                    //if cancelled - report about it
                    ProcessReportSlave(new CorutineReportCancelled());
                    //and exit
                    ProcessReportSlave(new ThreadEndedReport(i));
                    return;
                }
            }
            ProcessReportSlave(new ThreadEndedReport(i));
        }
        /// <summary>
        /// Process report from slave thread
        /// </summary>
        /// <param name="rep">Report</param>
        private void ProcessReportSlave(CorutineReport rep)
        {
            reportQueue.Enqueue(rep);
        }
        /// <summary>
        /// Class for report when thread ends
        /// </summary>
        protected class ThreadEndedReport:CorutineReport
        {
            public int threadId;

            public ThreadEndedReport(int tId)
            {
                threadId = tId;
            }
        }
        /// <summary>
        /// Class for report when thread reports progress
        /// </summary>
        protected class ThreadProgressReport : CorutineReport
        {
            public readonly CorutineReportPercentage perc;
            public readonly int threadId;

            public ThreadProgressReport(int tId, CorutineReportPercentage report)
            {
                threadId = tId;
                perc = report;
            }
        }
    }
}
