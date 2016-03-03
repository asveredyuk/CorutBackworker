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
        public Form1()
        {
            InitializeComponent();
        }
        Corutine corut;
        private void button1_Click(object sender, EventArgs e)
        {
            corut = new Corutine(this, dumbCorut(2, 100000));
            //corut.binders.Add(new CorutineProgressbarBinder(progressBar1));
            //corut.binders.Add(new CorutineLabelBinder(label1));
            /*corut.BindLabel(label1);
            corut.BindProgressBar(progressBar1);*/
            SimpleProgressForm form = new SimpleProgressForm(corut);
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
        /// <summary>
        /// Dumb algorithm to find simple numbers
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IEnumerable<CorutineReport> dumbCorut(int from, int to)
        {
            List<int> results = new List<int>();
            for (int i = from; i <= to; i++)
            {
                if (checkIsSimple(i))
                    results.Add(i);
                //get the progress
                int len = to - from;
                int progress = (i - from)*100 / len;
                yield return new CorutineReportPercentage(progress);
                yield return new CorutineReportText(string.Format("Found: {0}", results.Count));
            }
            yield return new CorutineReportResult(results);
        }
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
