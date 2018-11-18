using System;

namespace FusionScript.Structs
{
    public class StructFor : Struct
    {
        public StructAssign Initial;
        public StructExpr Condition;
        public StructAssign Inc;
        public StructCode Code;

       

        public override dynamic Exec ( )
        {
            Initial.Exec ( );
            while ( Condition.Exec ( )==1 )
            {
                Code.Exec ( );
                Inc.Exec ( );
            }
            return null;
        }

    
    }
}