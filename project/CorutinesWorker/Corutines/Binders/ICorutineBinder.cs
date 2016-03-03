using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorutBackworker.Corutines
{
    public interface ICorutineBinder
    {
        /// <summary>
        /// Called when report from corutine got
        /// </summary>
        /// <param name="report"></param>
        void OnReport(CorutineReport report);
        /// <summary>
        /// Called before starting corutine for initializing control
        /// </summary>
        void Init();
    }
}
