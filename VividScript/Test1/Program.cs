using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VividScript;
using VividScript.VStructs;
namespace Test1
{
    class Program
    {
        static void Main(string[] args)
        {
            var src = new VSource("test1.vs");
            var comp = new VCompiler();
            var s = comp.Compile(src);
            while (true)
            {

            }

        }
    }
}
