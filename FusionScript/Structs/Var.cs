namespace FusionScript.Structs
{
    public class Var
    {
        public VarType Type
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public StructExpr Init = null;

        public dynamic Value =0;

        

        public string DebugString ( )
        {
            return "Var. Type:" + Type.ToString ( ) + " Name:" + Name;
        }
    }
}