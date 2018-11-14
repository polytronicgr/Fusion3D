using System.Threading;

namespace Fusion3D.Transient
{
    public delegate void Process ( );

    public class Transient
    {
        public Thread LT = null;
        public Process Before = null;
        public Process Action = null;
        public Process After = null;
        public bool Complete = false;

        public Transient ( )
        {
        }

        public Transient ( Process act, Process before = null, Process after = null )
        {
            Action = act;
            Before = before;
            After = after;
        }

        public void Do ( )
        {
            if ( Before != null )
            {
                Before ( );
            }
            LT = new Thread ( new ThreadStart ( DO_Thread ) );
            LT.Start ( );
            if ( After != null )
            {
                After ( );
            }
        }

        public void DO_Thread ( )
        {
            if ( Action != null )
            {
                Action ( );
            }
            Complete = true;
        }
    }
}