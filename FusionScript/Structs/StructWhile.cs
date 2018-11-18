namespace FusionScript.Structs
{
    public class StructWhile : Struct
    {
        public StructExpr Condition = null;
        public StructCode Code = null;

   
        public override string DebugString ( )
        {
            return "Conditon:" + Condition.DebugString ( );
        }

        public override dynamic Exec ( )
        {
            System.Console.WriteLine ( "ExecWhile" );
            while ( Condition.Exec ( )==1 )
            {
                Code.Exec ( );
            }
            //Code.Exec ( );

            return null;
        }

     
    }
}