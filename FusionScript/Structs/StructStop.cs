using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionScript;
namespace FusionScript.Structs
{
    public class StructStop : Struct
    {

        public override dynamic Exec()
        {
            Console.WriteLine("Stopped:");
            return null;
        }

    }
}
