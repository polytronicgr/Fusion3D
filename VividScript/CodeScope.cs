using System.Collections.Generic;
using VividScript.VStructs;

namespace VividScript
{
    public class CodeScope
    {
        public CodeScope OutterScope = null;
        public List<VSVar> LocalVars = new List<VSVar>();

        public List<FuncLink> LocalFuncs = new List<FuncLink>();

        public void RegisterFunc ( FuncLink func )
        {
            LocalFuncs.Add ( func );
        }

        public void RegisterVar ( VSVar var )
        {
            LocalVars.Add ( var );
        }

        public FuncLink FindFunc ( string name, bool searchOutter )
        {
            foreach ( FuncLink func in LocalFuncs )
            {
                if ( func.Name == name )
                {
                    return func;
                }
            }
            if ( searchOutter )
            {
                if ( OutterScope != null )
                {
                    return OutterScope.FindFunc ( name, true );
                }
            }
            VME.Main.Error ( "Could not find func called:" + name, "ScopeError" );
            return null;
        }

        public VSVar FindVar ( string name, bool searchOutter )
        {
            foreach ( VSVar v in LocalVars )
            {
                if ( v.Name == name )
                {
                    return v;
                }
            }
            if ( searchOutter )
            {
                if ( OutterScope != null )
                {
                    return OutterScope.FindVar ( name, true );
                }
            }
            VME.Main.Error ( "Could not find variable called:" + name, "ScopeError" );
            return null;
        }
    }
}