using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Tex;
using OpenTK;
using System.IO;
namespace Vivid3D.Scene
{
    public class GraphLight : GraphNode
    {
        public Vector3 Diffuse
        {
            get;
            set;
        }
        public Vector3 Specular
        {
            get;
            set;
        }
        public float Shiny
        {
            get;
            set;
        }
        public float Range
        {
            get;
            set;
        }
        public LightType Type
        {
            get;
            set;
        }
        public bool On
        {
            get;
            set;
        }
        public bool CastShadows
        {
            get;
            set;
        }
        public GraphLight()
        {
            Diffuse = new Vector3(0.5f, 0.5f, 0.5f);
            Specular = new Vector3(0, 0, 0);
            Shiny = 0.2f;
            Range = 500;
            Type = LightType.Point;
            On = true;
            CastShadows = true;
        }
        public void Write(BinaryWriter w)
        {
            w.Write(X);
            w.Write(Y);
            w.Write(Z);
            w.Write(Rot);
            w.Write(Diffuse.X);
            w.Write(Diffuse.Y);
            w.Write(Diffuse.Z);
            w.Write(Specular.X);
            w.Write(Specular.Y);
            w.Write(Specular.Z);
            w.Write(Shiny);
            w.Write(Range);
            w.Write(On);
            w.Write(CastShadows);
            w.Write((int)Type);

        }
        public void Read(BinaryReader r)
        {
            X = r.ReadSingle();
            Y = r.ReadSingle();
            Z = r.ReadSingle();
            Rot = r.ReadSingle();
            var d = new Vector3();
            d.X = r.ReadSingle();
            d.Y = r.ReadSingle();
            d.Z = r.ReadSingle();
            Diffuse = d;
            var s = new Vector3();
            s.X = r.ReadSingle();
            s.Y = r.ReadSingle();
            s.Z = r.ReadSingle();
            Specular = s;
            Shiny = r.ReadSingle();
            Range = r.ReadSingle();
            On = r.ReadBoolean();
            CastShadows = r.ReadBoolean();
            Type = (LightType)r.ReadInt32();
        }
        
    }
    public enum LightType
    {
        Point,Directional,Ambient,Spot
    }
}
