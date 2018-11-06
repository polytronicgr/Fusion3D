using System;
using System.Collections.Generic;

namespace VividScript.VStructs
{
    public class VSExpr : VStruct
    {
        public List<VSExpr> Expr = new List<VSExpr>();

        public VSExpr ( VTokenStream s ) : base ( s )
        {
        }

        public override void SetupParser ( )
        {
            Console.WriteLine ( "Parsing expression." );
            Parser = ( t ) =>
            {
                Console.WriteLine ( "PE:" + t );
                switch ( t.Class )
                {
                    case TokenClass.Value:
                        Console.WriteLine ( "Value:" + t.Text );
                        break;

                    default:
                        Done = true;
                        return;
                        break;
                }
            };
        }
    }
}