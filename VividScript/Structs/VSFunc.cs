using System;

namespace VividScript.VStructs
{
    public class VSFunc : VStruct
    {
        public string FuncName = "";

        public VSFunc ( VTokenStream s ) : base ( s )
        {
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
                    VSPars pars = new VSPars(TokStream);
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