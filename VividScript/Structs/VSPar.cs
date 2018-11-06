namespace VividScript.VStructs
{
    public class VSPar : VStruct
    {
        public VarType ParType = VarType.Bool;

        public VSPar ( VTokenStream s ) : base ( s, true )
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