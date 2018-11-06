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

        public string DebugString ( )
        {
            return "Var. Type:" + Type.ToString ( ) + " Name:" + Name;
        }
    }
}