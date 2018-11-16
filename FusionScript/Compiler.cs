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


            var parser = new Parser(s.Tokens);

            StructEntry entry = parser.Entry;

            cs.EntryPoint = entry;

            return cs;
        }
    }
}