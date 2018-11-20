using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionScript.Structs.Compute
{
    public class StructCompute : Struct
    {

        public string Name = "";
        public List<Compute.StructComputeInput> Inputs = new List<StructComputeInput>();
        public List<ComputeVar> LocalVars = new List<ComputeVar>();

    }
}
