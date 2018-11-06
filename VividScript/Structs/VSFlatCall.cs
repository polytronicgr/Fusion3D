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