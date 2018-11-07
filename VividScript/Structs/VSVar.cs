namespace VividScript.VStructs
{
    public class VSVar
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

        public dynamic Value =0;

        public string DebugString ( )
        {
            return "Var. Type:" + Type.ToString ( ) + " Name:" + Name;
        }
    }
}