using System;

namespace FusionScript.Structs
{
    public class StructFunc : Struct
    {
        public string FuncName = "";
        public StructPars Pars = null;
        public StructCode Code = null;

        public StructFunc ( TokenStream s ) : base ( s )
        {
            LocalScope = new CodeScope("FuncScope");
        }

        public override string DebugString ( )
        {
            return "Func:" + FuncName;
        }

        public override void SetupParser ( )
        {
            PreParser = ( t ) =>
            {
                FuncName = t.Text;
            };

            Parser = ( t ) =>
            {
                if ( t.Token == Token.LeftPara )
                {

                    Pars = new StructPars ( TokStream );
                    Code = new StructCode ( TokStream );
                }
                if ( t.Token == Token.End )
                {
                    Done = true;
                    return;
                }
            };
        }
    }
}