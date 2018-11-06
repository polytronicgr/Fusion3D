using System.Collections.Generic;
using VividScript.VStructs;

namespace VividScript
{
    public class LinkEngineBase
    {
        public CodeScope Scope = null;

        public List<VSVar> LocalVars = new List<VSVar>();

        public List<FuncLink> LocalFuncs = new List<FuncLink>();

        public virtual void RegisterFunc ( FuncLink func )
        {
            LocalFuncs.Add ( func );
        }

        public virtual void RegisterVar ( VSVar var )
        {
            LocalVars.Add ( var );
        }

        public virtual FuncLink FindFunc ( string name, bool searchOutter )
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
                if ( Scope.OutterScope != null )
                {
                    return Scope.OutterScope.FindFunc ( name, true );
                }
            }
            VME.Main.Error ( "Could not find func called:" + name, "ScopeError" );
            return null;
        }

        public virtual VSVar FindVar ( string name, bool searchOutter )
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
                if ( Scope.OutterScope != null )
                {
                    return Scope.OutterScope.FindVar ( name, true );
                }
            }
            VME.Main.Error ( "Could not find variable called:" + name, "ScopeError" );
            return null;
        }
    }
}