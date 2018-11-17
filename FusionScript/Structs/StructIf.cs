namespace FusionScript.Structs
{
    public class StructIf : Struct
    {
        public StructExpr Condition;
        public StructCode TrueCode;

     
        public override dynamic Exec ( )
        {
            if ( Condition.Exec ( ) )
            {
                TrueCode.Exec ( );
            }
            return null;
        }

       
    }
}