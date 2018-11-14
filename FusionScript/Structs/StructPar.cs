namespace FusionScript.Structs
{
    public class StructPar : Struct
    {
        public VarType ParType = VarType.Bool;

        public StructPar ( TokenStream s ) : base ( s, true )
        {
            Done = true;
        }

        public override string DebugString ( )
        {
            return "Par: " + Name + " Type: " + ParType;
        }

        public override void SetupParser ( )
        {
        }
    }
}