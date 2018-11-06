using System;

namespace VividScript.VStructs
{
    public class VSPars : VStruct
    {
        public VSPars ( VTokenStream s ) : base ( s )
        {
        }

        public override void SetupParser ( )
        {
            Parser = ( t ) =>

            {
                Console.WriteLine ( "T:" + t.Token + " T:" + t.Text );
                if ( t.Token == Token.Comma )
                {
                    return;
                }
                switch ( t.Token )
                {
                    case Token.Int:
                    case Token.Float:
                    case Token.Byte:
                    case Token.Bool:
                    case Token.Double:
                    case Token.String:
                    case Token.Short:
                        VToken name = ConsumeNext();
                        Console.WriteLine ( "Par Type:" + t.Token.ToString ( ) + " Name:" + name.Text );
                        break;
                }

                if ( t.Token == Token.RightPara )
                {
                    Done = true;
                }
            };
        }
    }
}