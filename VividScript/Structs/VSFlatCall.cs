using System;

namespace VividScript.VStructs
{
    public class VSFlatCall : VStruct
    {
        public VSCallPars CallPars = null;
        public string FuncName = "";

        public VSFlatCall ( VTokenStream s ) : base ( s )
        {
        }

        public override string DebugString ( )
        {
            return "FlatCall:" + FuncName + " Pars:" + CallPars.Pars.Count;
        }

        public override void Exec ( )
        {
            FuncLink funcLink = LocalScope.FindFunc(FuncName,true);
            if ( funcLink != null )
            {
                switch ( funcLink.Type )
                {
                    case FuncType.CSharp:
                        funcLink.Link.Call ( "Testing!" );
                        break;
                }
            }
        }

        public override void SetupParser ( )
        {
            Console.WriteLine ( "Parsing flat-call." );
            Parser = ( t ) =>
            {
                if ( t.Token == Token.LeftPara )
                {
                    return;
                }
                BackOne ( );
                CallPars = new VSCallPars ( TokStream );
                Console.WriteLine ( "FC:" + t.ToString ( ) );
            };
        }
    }
}