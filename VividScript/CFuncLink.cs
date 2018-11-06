namespace VividScript
{
    public class FuncLink
    {
        public string Name = "";
        public dynamic ManagedFunc;
        public CFuncLink Link;
        public FuncType Type = FuncType.CSharp;
    }

    public enum FuncType
    {
        CSharp, Managed, DLL
    }

    public class CFuncLink
    {
        public string Name = "";

        public CFunc Link = null;

        public dynamic Call ( params dynamic [ ] pars )
        {
            return Link?.Invoke ( pars );
        }
    }

    public class CFuncVar
    {
        public string Name = "";
    }

    public class CFuncVarString : CFuncVar
    {
        public string Value = "";
    }

    public delegate CFuncVar CFunc ( params dynamic [ ] pars );
}