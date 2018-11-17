namespace FusionScript.Structs
{
    public class StructPar : Struct
    {
        public VarType ParType = VarType.Bool;


        public override string DebugString ( )
        {
            return "Par: " + Name + " Type: " + ParType;
        }

       
    }
}