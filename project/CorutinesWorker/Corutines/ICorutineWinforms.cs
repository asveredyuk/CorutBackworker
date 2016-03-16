using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorutinesWorker.Corutines
{
    /// <summary>
    /// Cortine, which is winforms-bindable
    /// </summary>
    public interface ICorutineWinforms : ICorutine
    {
        void BindProgressBar(ProgressBar bar);
        void BindText(Control c);
        void BindProgressText(Control c);
        void OnCompleted(CorutineCompletedBinder.CompletedHandler handler);
        void OnCancelled(CorutineCancelledBinder.CancelHandler handler);
    }
}
