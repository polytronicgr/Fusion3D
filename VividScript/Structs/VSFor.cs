using System;

namespace VividScript.VStructs
{
    public class VSFor : VStruct
    {
        public VSAssign Initial;
        public VSExpr Condition;
        public VSAssign Inc;
        public VSCode Code;

        public VSFor ( VTokenStream s ) : base ( s )
        {
        }

        public override dynamic Exec ( )
        {
            Initial.Exec ( );
            while ( Condition.Exec ( ) )
            {
                Code.Exec ( );
                Inc.Exec ( );
            }
            return null;
        }

        public override void SetupParser ( )
        {
            Parser = ( t ) =>
            {
                Console.WriteLine ( t.ToString ( ) + PeekNext ( ).ToString ( ) );
                Initial = new VSAssign ( TokStream );
                ConsumeNext ( );
                Condition = new VSExpr ( TokStream );
                Inc = new VSAssign ( TokStream );
                // ConsumeNext ( );
                Code = new VSCode ( TokStream );
                Done = true;
                return;
            };
        }
    }
}