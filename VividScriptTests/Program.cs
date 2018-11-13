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



            VME test_vme = new VME();
            test_vme.AddModule(new Module(s.EntryPoint));

          

            test_vme.ExecuteStaticFunc("testFunc");

         

            while ( true ) { }
        }
    }
}