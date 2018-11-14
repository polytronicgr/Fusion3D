using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion3D.Script;
namespace FusionIDE.script
{
    public class Scripts
    {

        public static void ScanMods()
        {

            var mod_dir = new DirectoryInfo("data/module/");

            Console.WriteLine("Scanning modules..");

            foreach (var file in mod_dir.GetFiles())
            {

                Console.WriteLine("Module:" + file.Name);
                LoadMod(file.FullName);
            }


            Console.WriteLine("Modules scanned.");




        }

        public static void LoadMod(string path)
        {

            var compiler = new FusionScript.Compiler();
            var src = new FusionScript.ScriptSource(path);
            var csrc = compiler.Compile(src);
            


        }

    }
}
