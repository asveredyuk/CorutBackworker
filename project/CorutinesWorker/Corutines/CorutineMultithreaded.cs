using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorutinesWorker.Corutines
{
    public class CorutineMultithreaded : CorutineMultithreadedBase, ICorutineWinforms
    {
        protected Control context;
        public CorutineMultithreaded(Control _context, IEnumerable<CorutineReport>[] coruts)
            : base(coruts)
        {
            context = _context;
        }

        public void BindProgressBar(ProgressBar bar)
        {
            binders.Add(new CorutineProgressbarBinder(bar));
        }
        public void BindText(Control c)
        {
            binders.Add(new CorutineTextBinder(c));
        }
        public void BindProgressText(Control c)
        {
            binders.Add(new CorutineProgressTextBinder(c));
        }
        public void OnCompleted(CorutineCompletedBinder.CompletedHandler handler)
        {
            binders.Add(new CorutineCompletedBinder(context, handler));
        }
        public void OnCancelled(CorutineCancelledBinder.CancelHandler handler)
        {
            binders.Add(new CorutineCancelledBinder(context, handler));
        }

    }
}
