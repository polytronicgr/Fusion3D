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

        public override dynamic Exec ( )
        {
            FuncLink funcLink = LocalScope.FindFunc(FuncName,true);
            if ( funcLink != null )
            {
                dynamic[] par = new dynamic[CallPars.Pars.Count];
                int i=0;
                foreach ( VSExpr exp in CallPars.Pars )
                {
                    par [ i ] = exp.Exec ( );
                }

                switch ( funcLink.Type )
                {
                    case FuncType.CSharp:
                        funcLink.Link.Call ( par );
                        break;
                }
            }
            return null;
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
                Done = true;
            };
        }
    }
}