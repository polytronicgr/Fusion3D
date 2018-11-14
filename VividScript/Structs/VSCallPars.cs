using System;
using System.Collections.Generic;

namespace VividScript.VStructs
{
    public class VSCallPars : VStruct
    {
        public List<VSExpr> Pars = new List<VSExpr>();

        public VSCallPars ( VTokenStream s ) : base ( s )
        {
        }

        public override void SetupParser ( )
        {
        
            Parser = ( t ) =>
            {
         
                switch ( t.Class )
                {
                    case TokenClass.Id:
                    case TokenClass.Value:
             
                        BackOne ( );
                        Pars.Add ( new VSExpr ( TokStream ) );
                        Done = true;
                        break;

                    default:
                        if ( t.Token == Token.Comma )
                        {
                        }
                        if ( t.Token == Token.RightPara )
                        {
                            Done = true;
                            return;
                        }
                        break;
                }
            };
        }
    }
}