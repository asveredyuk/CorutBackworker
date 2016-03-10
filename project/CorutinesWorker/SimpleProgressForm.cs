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
        /// <summary>
        /// Bind role - none (ex. hides label), show progress or show text messages
        /// </summary>
        public enum TextBindRole {None, Progress, Text}

        public bool askWhenClosed = true;
        public bool closeAfterFinished = true;
        public bool showCompletedDialog = true;
        public bool showCancelledDialog = true;
        public TextBindRole titleBindRole = TextBindRole.None;
        public TextBindRole labelBindRole = TextBindRole.Progress;
        public string title = "Progress";
        private Corutine corutine;
        public SimpleProgressForm(Corutine corut)
        {
            InitializeComponent();
            corutine = corut;
            
        }
        private void SimpleProgressForm_Load(object sender, EventArgs e)
        {
            this.Text = title;
            //check if we do not need label
            if (labelBindRole == TextBindRole.None)
                label.Visible = false;
            //bind progress bar to corutine
            corutine.BindProgressBar(progressBar);
            //bind label to corutine, if it is enabled
            if(labelBindRole == TextBindRole.Text)
                corutine.BindText(label);
            if(labelBindRole == TextBindRole.Progress)
                corutine.BindProgressText(label);

            //bind this form to corutine, if enabled
            if(titleBindRole == TextBindRole.Progress)
                corutine.BindProgressText(this);
            if(titleBindRole == TextBindRole.Text)
                corutine.BindText(this);
            //on completed event
            corutine.OnCompleted(delegate(object res)
            {
                if (showCompletedDialog)
                    MessageBox.Show(res!= null?res.ToString():"null got", "Completed");
                if (closeAfterFinished)
                    this.ForceClose();
            });
            //on cancelled event
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
        /// <summary>
        /// Start corutine
        /// </summary>
        public void Start()
        {
            this.ShowDialog();
        }
        /// <summary>
        /// Used for closing form without any messagebox
        /// </summary>
        bool forceClose = false;
        /// <summary>
        /// Close form without any notifications
        /// </summary>
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
