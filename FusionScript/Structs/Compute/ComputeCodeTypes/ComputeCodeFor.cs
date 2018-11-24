using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionScript.Structs.Compute.ComputeCodeTypes
{
    public class ComputeCodeFor : ComputeCodeBase
    {
        public ComputeCodeAssign InitAssign;
        public ComputeCodeExpr Condition;
        public ComputeCodeAssign Inc;

    }
}
