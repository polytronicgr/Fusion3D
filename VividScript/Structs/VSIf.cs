namespace VividScript.VStructs
{
    public class VSIf : VStruct
    {
        public VSExpr Condition;
        public VSCode TrueCode;

        public VSIf ( VTokenStream s ) : base ( s )
        {
        }

        public override dynamic Exec ( )
        {
            if ( Condition.Exec ( ) )
            {
                TrueCode.Exec ( );
            }
            return null;
        }

        public override void SetupParser ( )
        {
            Parser = ( t ) =>
            {
                ConsumeNext ( );
                Condition = new VSExpr ( TokStream );
                TrueCode = new VSCode ( TokStream );
                Done = true;
                return;
            };
        }
    }
}