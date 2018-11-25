using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionScript.Structs.Compute
{
    public class StructComputeStruct : Struct
    {
        public string StructName = "";
        public List<ComputeVar> Vars = new List<ComputeVar>();
        public string LocalName = "";
        public bool One = false;
        public bool LinearData = false;
        public StructComputeStruct()
        {

        }

        public StructComputeStruct Copy()
        {
            var r = new StructComputeStruct();
            r.StructName = StructName;
            r.Vars = Vars;
            r.LocalName = "";
            r.LinearData = LinearData;
            return r;
        }

    }
    public class ComputeVar
    {
        public string Name = "";
        public ComputeVarType Type = ComputeVarType.Int;
        public bool Pointer = false;
        public string StructName = "";
    }
    public enum ComputeVarType
    {
        Vec2,Vec3,Vec4,Matrix4,Matrix3,Float,Int,Bool,Byte,Void,Struct
    }
}
