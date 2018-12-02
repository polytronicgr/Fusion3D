using System;

namespace FusionScript.Structs
{
    public class StructFlatCall : Struct
    {
        public StructCallPars CallPars = null;
        public string FuncName = "";

     

        public override string DebugString ( )
        {
            return "FlatCall:" + FuncName + " Pars:" + CallPars.Pars.Count;
        }

        public override dynamic Exec ( )
        {
            FuncLink funcLink = ManagedHost.CurrentScope.FindFunc(FuncName,true);
            if ( funcLink != null )
            {
                dynamic[] par = new dynamic[CallPars.Pars.Count];
                int i=0;
                foreach ( StructExpr exp in CallPars.Pars )
                {
                    par [ i ] = exp.Exec ( );
                    i++;
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

       
    }
}