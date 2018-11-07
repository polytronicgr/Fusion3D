namespace VividScript.VStructs
{
    public class VSWhile : VStruct
    {
        public VSExpr Condition = null;
        public VSCode Code = null;

        public VSWhile ( VTokenStream s ) : base ( s )
        {
        }

        public override string DebugString ( )
        {
            return "Conditon:" + Condition.DebugString ( );
        }

        public override dynamic Exec ( )
        {
            System.Console.WriteLine ( "ExecWhile" );
            while ( Condition.Exec ( ) )
            {
                Code.Exec ( );
            }
            //Code.Exec ( );

            return null;
        }

        public override void SetupParser ( )
        {
            Parser = ( t ) =>
            {
                Condition = new VSExpr ( TokStream );
                Code = new VSCode ( TokStream );
                Done = true;
                return;
            };
        }
    }
}