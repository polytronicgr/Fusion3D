using FusionScript;

namespace VividScriptTests
{
    internal class Program
    {
        private static void Main ( string [ ] args )
        {
            ScriptSource src = new ScriptSource("test1.vs");
            Compiler comp = new Compiler();
            CompiledSource s = comp.Compile(src);



            ManagedHost test_vme = new ManagedHost();
            test_vme.AddModule(new Module(s.EntryPoint));

          

            test_vme.ExecuteStaticFunc("test");

         

            while ( true ) { }
        }
    }
}