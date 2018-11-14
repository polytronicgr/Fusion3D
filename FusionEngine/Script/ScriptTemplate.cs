using System;

namespace Fusion3D.Script
{
    public class ScriptTemplate : ScriptMod
    {
        public override void Begin ( )
        {
            Console.WriteLine ( "Begun Sciprt!" );
            base.Begin ( );
        }

        public override void End ( )
        {
            Console.WriteLine ( "Ended Script!" );
            base.End ( );
        }
    }
}