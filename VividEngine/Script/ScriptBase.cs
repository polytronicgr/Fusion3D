using CSScriptLibrary;
using Vivid3D.Scene;

namespace Vivid3D.Script
{
    public class ScriptLink : GraphNode3D
    {
        private dynamic script;

        public string FilePath
        {
            get;
            set;
        }

        public bool Compiled
        {
            get;
            set;
        }

        public ScriptLink ( )
        {
            Compiled = false;
        }

        public void Compile ( GraphNode3D node )
        {
            if ( Compiled )
            {
                return;
            }

            Compiled = true;
            script = CSScript.Evaluator.LoadCode ( System.IO.File.ReadAllText ( FilePath ) );
            script.Pad = new XInput.XPad ( 0 );
            script.Node = node;
            System.Console.WriteLine ( "Script:" + FilePath + " Compiled." );
        }

        public void Update ( )
        {
            script.Update ( );
        }

        public void Begin ( )
        {
            script.Begin ( );
        }

        public void End ( )
        {
            script.End ( );
        }
    }
}