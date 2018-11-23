using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionScript.Structs.Compute
{
    public class StructComputeFunc
    {
        public ComputeVarType ReturnType = ComputeVarType.Void;
        public List<ComputeVar> InVars = new List<ComputeVar>();
        public string FuncName = "";

    }
}
