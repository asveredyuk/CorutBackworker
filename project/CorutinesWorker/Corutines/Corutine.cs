using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorutBackworker.Corutines
{
    public  class Corutine : CorutineBase
    {
        protected Control context;
        public Corutine(Control _context, IEnumerable<CorutineReport> corut):base(corut)
        {
            context = _context;
        }

        public void BindProgressBar(ProgressBar bar)
        {
            binders.Add(new CorutineProgressbarBinder(bar));
        }
        public void BindLabel(Label label)
        {
            binders.Add(new CorutineLabelBinder(label));
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
