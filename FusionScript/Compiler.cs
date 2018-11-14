using FusionScript.Structs;

namespace FusionScript
{
    public class Compiler
    {
        public CompiledSource Compile ( string path )
        {
            return Compile ( new ScriptSource ( path ) );
        }

        public CompiledSource Compile ( ScriptSource s )
        {
            CompiledSource cs = new CompiledSource();

            StructEntry entry = new StructEntry(s.Tokens);

            cs.EntryPoint = entry;

            return cs;
        }
    }
}