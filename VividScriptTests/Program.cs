using VividScript;

namespace VividScriptTests
{
    internal class Program
    {
        private static void Main ( string [ ] args )
        {
            VSource src = new VSource("test1.vs");
            VCompiler comp = new VCompiler();
            VCompiledSource s = comp.Compile(src);
            while ( true )
            {
            }
        }
    }
}