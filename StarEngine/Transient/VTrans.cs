using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace Vivid3D.Transient
{
    public delegate void Process();
    public class VTransient
    {
        public Thread LT = null;
        public Process Before = null;
        public Process Action = null;
        public Process After = null;
        public bool Complete = false;
        public VTransient()
        {

        }
        public VTransient(Process act, Process before = null, Process after = null)
        {
            Action = act;
            Before = before;
            After = after;
        }
        public void Do()
        {
            if (Before != null)
            {
                Before();
            }
                        LT = new Thread(new ThreadStart(DO_Thread));
            LT.Start();
            if (After != null)
            {
                After();
            }
        }
        public void DO_Thread()
        {
            if (Action != null)
            {
                Action();
            }
            Complete = true;
        }
    }
}
