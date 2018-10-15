using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.FX;
using Vivid3D.Scene;
using Vivid3D.Util;
namespace Vivid3D.FXS
{
    public class FXLitImage : VEffect
    {

        public GraphLight Light
        {
            get;
            set;
        }

        public SceneGraph Graph
        {
            get;
            set;
        }

        public FXLitImage() : base("","Data/Shader/LitImageVS.glsl","Data/Shader/LitImageFS.glsl")
        {

        }
        public override void SetPars()
        {
            float sw, sh;
            sw = Vivid3D.App.VividApp.W;
            sh = Vivid3D.App.VividApp.H;
            float px, py, pz;

            // px = Light.X + Graph.X;
            // py = Light.Y + Graph.Y;
            px = Light.X * Graph.Z;
            py = Light.Y * Graph.Z;

            //px = (sw / 2) + px;
            //py = (sh / 2) + py;

            px = px - Graph.X * Graph.Z;
            py = py - Graph.Y * Graph.Z;

            var res = Maths.Rotate(px, py, Graph.Rot, 1.0f);

            res = Maths.Push(res, sw / 2, sh / 2);
                




            SetTex("tDiffuse", 0);
            SetVec3("lPos", new OpenTK.Vector3(res.X,res.Y,0));
            SetVec3("lDif", Light.Diffuse);
            SetVec3("lSpec", Light.Specular);
            SetFloat("lShiny", Light.Shiny);
            SetFloat("lRange", Light.Range * Graph.Z);
            SetFloat("sWidth", Vivid3D.App.VividApp.W);
            SetFloat("sHeight", Vivid3D.App.VividApp.H);

        }
    }
}
