using System;
using System.Collections.Generic;

namespace FusionScript.Structs
{
    public class StructCallPars : Struct
    {
        public List<StructExpr> Pars = new List<StructExpr>();

        public StructCallPars ( TokenStream s ) : base ( s )
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
                        Pars.Add ( new StructExpr ( TokStream ) );
                        var ntok = PeekNext();

                        break;

                    default:
                        if ( t.Token == Token.Comma )
                        {
                        }
                        if(t.Token == Token.EndLine)
                        {
                            Done = true;
                            return;
                        }
                        if (t.Token == Token.RightPara) 
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