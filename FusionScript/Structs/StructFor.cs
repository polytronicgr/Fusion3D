using System;

namespace FusionScript.Structs
{
    public class StructFor : Struct
    {
        public StructAssign Initial;
        public StructExpr Condition;
        public StructAssign Inc;
        public StructCode Code;

        public StructFor ( TokenStream s ) : base ( s )
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
                Initial = new StructAssign ( TokStream );
                ConsumeNext ( );
                Condition = new StructExpr ( TokStream );
                Inc = new StructAssign ( TokStream );
                // ConsumeNext ( );
                Code = new StructCode ( TokStream );
                Done = true;
                return;
            };
        }
    }
}