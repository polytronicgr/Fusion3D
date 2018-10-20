using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Vivid3D.Help
{
    public class IOHelp
    {
        public static BinaryWriter w;
        public static BinaryReader r;
        public static void WriteMatrix(OpenTK.Matrix4 m)
        {
            WriteVec(m.Row0);
            WriteVec(m.Row1);
            WriteVec(m.Row2);
            WriteVec(m.Row3);
        }
        public static void WriteVec(OpenTK.Vector4 v)
        {
            w.Write(v.X);
            w.Write(v.Y);
            w.Write(v.Z);
            w.Write(v.Z);
        }
        public static void WriteVec(OpenTK.Vector3 v)
        {
            w.Write(v.X);
            w.Write(v.Y);
            w.Write(v.Z);
        }
        public static void WriteFloat(float v)
        {
            w.Write(v);
        }
        public static void WriteBool(bool v)
        {
            w.Write(v);
        }
        public static OpenTK.Matrix4 ReadMatrix()
        {
            var m = new OpenTK.Matrix4(ReadVec4(), ReadVec4(), ReadVec4(), ReadVec4());
            return m;
        }
        public static OpenTK.Vector4 ReadVec4()
        {
            return new OpenTK.Vector4(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
        }
        public static OpenTK.Vector3 ReadVec3()
        {
            return new OpenTK.Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
        }
        public static float ReadFloat()
        {
            return r.ReadSingle();
        }
        public static bool ReadBool()
        {
            return r.ReadBoolean();
        }
    }
}
