using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionScript;
using FusionScript.Structs;
namespace Foom
{
    class Program
    {
        static void Main(string[] args)
        {

            ScriptSource src = new ScriptSource("Foom/main.fs");
            Compiler comp = new Compiler();
            CompiledSource s = comp.Compile(src);



            ManagedHost test_vme = new ManagedHost();
            test_vme.AddModule(new Module(s.EntryPoint));

            FoomApp Foom = null;

            dynamic F_InitFoom(dynamic[] pars)
            {
                Console.WriteLine("Foom booting up.");

                Foom = new FoomApp();

                return null;
            }

            var InitFoom = new CFuncLink
            {
                Link = F_InitFoom,
                Name = "InitFoom"
            };
            test_vme.RegFunc("InitFoom", InitFoom);
            

            System.Console.WriteLine("R:" + test_vme.ExecuteStaticFunc("Entry"));

            while (true)
            {

            }

        }
    }
}
