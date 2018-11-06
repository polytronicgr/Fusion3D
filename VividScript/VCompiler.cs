using VividScript.VStructs;

namespace VividScript
{
    public class VCompiler
    {
        public VCompiledSource Compile ( string path )
        {
            return Compile ( new VSource ( path ) );
        }

        public VCompiledSource Compile ( VSource s )
        {
            VCompiledSource cs = new VCompiledSource();

            VSEntry entry = new VSEntry(s.Tokens);

            cs.EntryPoint = entry;

            return cs;
        }
    }
}