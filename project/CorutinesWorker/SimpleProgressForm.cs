using CorutinesWorker.Corutines;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorutinesWorker
{
    public partial class SimpleProgressForm : Form
    {
        public bool askWhenClosed = true;
        public bool closeAfterFinished = true;
        public bool showCompletedDialog = true;
        public bool showCancelledDialog = true;
        public string title = "Progress";
        //string title = "Progress",bool askWhenClosed = true, bool closeAfterFinished = true, bool showCompletedDialog = true, bool showCancelledDialog = true
        Corutine corutine;
        public SimpleProgressForm(Corutine corut)
        {
            InitializeComponent();
            corutine = corut;
            
        }
        private void SimpleProgressForm_Load(object sender, EventArgs e)
        {
            this.Text = title;
            corutine.BindProgressBar(progressBar);
            corutine.BindText(label);
            corutine.BindProgressText(this);
            
            corutine.OnCompleted(delegate(object res)
            {
                if (showCompletedDialog)
                    MessageBox.Show(res!= null?res.ToString():"null got", "Completed");
                if (closeAfterFinished)
                    this.ForceClose();
            });
            corutine.OnCancelled(delegate()
            {
                if (showCancelledDialog)
                    MessageBox.Show("Cancelled");
                if (closeAfterFinished)
                    this.ForceClose();
            });


            corutine.Start();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            corutine.Cancel();
        }
        public void Start()
        {
            this.ShowDialog();
        }
        bool forceClose = false;

        private void ForceClose()
        {
            forceClose = true;
            this.Close();
        }
        private void SimpleProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (forceClose)
                return;
            
            if(corutine.IsWorking)
            {
                if (askWhenClosed)
                {
                    DialogResult res = MessageBox.Show("Corutine is working, do you want to cancel?", "Are you shure", MessageBoxButtons.YesNo);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        corutine.Cancel();
                    }
                }
                else
                {
                    corutine.Cancel();
                }
            }
            e.Cancel = true;//cancel
        }

    
    }
}
