using System;

namespace VividScript.VStructs
{
    public class VSFunc : VStruct
    {
        public string FuncName = "";
        public VSPars Pars = null;
        public VSCode Code = null;

        public VSFunc ( VTokenStream s ) : base ( s )
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
                    Console.WriteLine ( "Parsing parameters." );
                    Pars = new VSPars ( TokStream );
                    Code = new VSCode ( TokStream );
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