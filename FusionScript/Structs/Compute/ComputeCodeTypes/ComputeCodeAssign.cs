using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionScript.Structs.Compute.ComputeCodeTypes
{
    public class ComputeCodeAssign : ComputeCodeBase
    {
        public bool Init = false;
        public string VarName = "";
        public ComputeCodeExpr Value = null;
        public ComputeVarType Type = ComputeVarType.Void;

    }
}
