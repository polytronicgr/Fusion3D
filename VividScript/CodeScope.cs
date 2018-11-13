using VividScript.VStructs;

namespace VividScript
{
    public class CodeScope
    {
        public CodeScope OutterScope = null;

        public LinkEngineBase Linker = null;

        public string ScopeID = "";

        public CodeScope(string id)
        {

            ScopeID = id;
            Linker = new LinkEngineBase()
            {
                Scope = this
            };

        }

        public void RegisterFunc ( FuncLink link )
        {
            Linker.RegisterFunc ( link );
        }

        public void RegisterVar ( VSVar var )
        {
            Linker.RegisterVar ( var );
        }

        public FuncLink FindFunc ( string name, bool outterScope )
        {
            return Linker.FindFunc ( name, outterScope );
        }

        public VSVar FindVar ( string name, bool outterScope )
        {
            return Linker.FindVar ( name, outterScope );
        }

        
    }
}