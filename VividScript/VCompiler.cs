using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VividScript.VStructs;
namespace VividScript
{
    public class VCompiler
    {
        public VCompiledSource Compile(string path)
        {
            return Compile(new VSource(path));

        }
        public VCompiledSource Compile(VSource s)
        {
            VCompiledSource cs = new VCompiledSource();

            VSEntry entry = new VSEntry(s.Tokens);

            return cs;

        }
    }
}
