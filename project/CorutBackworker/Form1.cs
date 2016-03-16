using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CorutinesWorker.Corutines;
using CorutinesWorker;
namespace CorutBackworker
{
    public partial class Form1 : Form
    {
        const int LOWER = 2;
        const int HIGHER = 100001*3;
        public Form1()
        {
            InitializeComponent();
        }
        ICorutineWinforms corut;
        private void button1_Click(object sender, EventArgs e)
        {
           //StartInOneThread();
           StartInMultiThreads(8);
        }
        private void StartInOneThread()
        {
            corut = new Corutine(this, dumbCorut(LOWER, HIGHER));
            //corut.binders.Add(new CorutineProgressbarBinder(progressBar1));
            //corut.binders.Add(new CorutineLabelBinder(label1));
            /*corut.BindLabel(label1);
            corut.BindProgressBar(progressBar1);*/
            SimpleProgressForm form = new SimpleProgressForm(corut) { showCompletedDialog = false };
            corut.OnCompleted(delegate(object result)
            {
                List<int> results = result as List<int>;
                MessageBox.Show(results.Count.ToString());
            });
            corut.OnCancelled(delegate
            {
                MessageBox.Show("process cancelled");
            });
            form.Start();
        }

        private void StartInMultiThreads(int num)
        {
            /* our task shoud be available to be split into subtasks
             * for this example we simply split our range to some subranges
             * */
            IEnumerable<CorutineReport>[] cors = new IEnumerable<CorutineReport>[num];
            int now = LOWER;
            int delta = (HIGHER - LOWER)/num;
            for (int i = 0; i < num; i++)
            {
                if (i < num - 1)
                {
                    cors[i] = dumbCorut(now, now + delta);
                    now += delta;
                }
                else
                {
                    cors[i] = dumbCorut(now, HIGHER);
                }
            }
            //create multithreaded corutine
            corut = new CorutineMultithreaded(this, cors);
            //oh yes, everything else is same, except the result!
            SimpleProgressForm form = new SimpleProgressForm(corut) { showCompletedDialog = false };
            corut.OnCompleted(delegate(object result)
            {
                //here we get a list of results (from each thread)
                List<CorutineReportResult> results = result as List<CorutineReportResult>;
                int sum = (from res in results
                    select (res.result as List<int>).Count).Sum();
                MessageBox.Show(sum.ToString());
            });
            corut.OnCancelled(delegate
            {
                MessageBox.Show("process cancelled");
            });
            form.Start();
        }


        /// <summary>
        /// Dumb algorithm to find prime numbers
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IEnumerable<CorutineReport> dumbCorut(int from, int to)
        {
            /* because refreshing GUI takes some time
             * we cannot report a lot
             * here we can report each 1000 checked
             * */
            const int REPORT_EACH = 1000;

            List<int> results = new List<int>();
            for (int i = from; i <= to; i++)
            {
                if (checkIsSimple(i))
                    results.Add(i);
                //get the progress
                //if now we should report a progress
                if (i%REPORT_EACH == 0 || i == to)
                {
                    int len = to - from;
                    int progress = (i - from) * 100 / len;
                    //report percentage
                    yield return new CorutineReportPercentage(progress);
                    //and report some result info
                    yield return new CorutineReportText(string.Format("Found: {0}", results.Count));
                }
            }
            //in the end of our corutine we return a result
            yield return new CorutineReportResult(results);
        }
        /// <summary>
        /// Dumb algorithm of checking if number is prime
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool checkIsSimple(int a)
        {
            for (int i = 2; i < a; i++)
            {
                if (a % i == 0)
                    return false;
            }
            return true;
        }

        private void beCancel_Click(object sender, EventArgs e)
        {
            if(corut != null)
            {
                corut.Cancel();
            }
        }

    }
}
