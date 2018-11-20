using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionScript.Structs.Compute
{
    public class StructComputeInput : Struct
    {
        public string InputName = "";
        public List<ComputeVar> Vars = new List<ComputeVar>();

        public StructComputeInput()
        {

        }

    }
    public class ComputeVar
    {
        public string Name = "";
        public ComputeVarType Type = ComputeVarType.Int;
    }
    public enum ComputeVarType
    {
        Vec2,Vec3,Vec4,Matrix4,Matrix3,Float,Int,Bool,Byte
    }
}
