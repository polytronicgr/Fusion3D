using System;
using System.Collections.Generic;

namespace VividScript.VStructs
{
    public class VSModule : VStruct
    {
        public string ModuleName = "";
        public List<VSVar> Vars = new List<VSVar>();

        public VSModule ( VTokenStream s ) : base ( s )
        {
        }

        public override string DebugString ( )
        {
            return "Module:" + ModuleName + " Vars:" + Vars.Count;
        }

        public override void SetupParser ( )
        {
            PreParser = ( t ) =>
            {
                ModuleName = t.Text;
            };
            Parser = ( t ) =>
            {
                if ( t.Token == Token.End )
                {
                    Done = true;
                    return;
                }
                switch ( t.Class )
                {
                    case TokenClass.Type:

                        Console.WriteLine ( "Parsing Variable definitions." );
                        BackOne ( );
                        VSDefineVars vdef = new VSDefineVars(TokStream);
                        foreach ( VSVar nv in vdef.Vars )
                        {
                            Vars.Add ( nv );
                        }
                        //Structs.Add(vdef);

                        break;
                }
            };
        }
    }
}