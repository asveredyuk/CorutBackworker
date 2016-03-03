using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CorutBackworker.Corutines
{
    public abstract class CorutineWinformsBinder : ICorutineBinder
    {
        public Control control;
        public CorutineWinformsBinder(Control c)
        {
            control = c;
        }
        /// <summary>
        /// Called when report from corutine got
        /// </summary>
        /// <param name="report"></param>
        public abstract void OnReport(CorutineReport report);
        /// <summary>
        /// Called before starting corutine for initializing control
        /// </summary>
        public abstract void Init();
    }
}
