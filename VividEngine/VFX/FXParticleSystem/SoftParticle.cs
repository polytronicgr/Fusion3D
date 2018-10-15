using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Vivid3D.Util;
namespace Vivid3D.VFX
{
    public class SoftParticle : ParticleBase
    {

        public SoftParticle(Tex.Tex2D img)
        {
            Img = img;
        }

        public override void Render()
        {

            
            var g = VFX.VisualFX.Graph;

            int sw = Vivid3D.App.VividApp.W;
            int sh = Vivid3D.App.VividApp.H;

            float[] ox = new float[4];
            float[] oy = new float[4];

            ox[0] = (-W / 2);// * Graph.Z * Z;
            ox[1] = (W / 2);// * Graph.Z * Z;
            ox[2] = (W / 2);// * Graph.Z* Z ;
            ox[3] = (-W / 2);// *Graph.Z*Z;

            oy[0] = (-H / 2);// * Graph.Z*Z;
            oy[1] = (-H / 2);// *Graph.Z*Z;
            oy[2] = (H / 2);// * Graph.Z * Z;
            oy[3] = (H / 2);// * Graph.Z * Z;


            Vector2[] p = Maths.RotateOC(ox, oy, Rot, Z, 0, 0);

            float mx, my;

            p = Maths.Push(p, X - g.X, Y - g.Y);

            p = Maths.RotateOC(p, g.Rot, g.Z, 0, 0);

            p = Maths.Push(p, sw / 2, sh / 2);

            Draw.Render.SetBlend(Draw.BlendMode.SoftLight);

            Draw.Render.Col = new Vector4(1, 1, 1, Alpha);

            Draw.Render.Image(p, Img);


            /*
            p = Maths.Rotate(p, Rot, 1.0f);

            p = Maths.Push(p, X, Y);

            Draw.Render.SetBlend(Draw.BlendMode.SoftLight);

            Draw.Render.Col = new Vector4(1, 1, 1, Alpha);

            Draw.Render.Image(p, Img);
            */
            //Console.WriteLine("X:" + X + " Y:" + Y + " Z:" + Z);
            //   Console.WriteLine("W" + W + " H:" + H);

        }

        public override ParticleBase Clone()
        {
            var np = new SoftParticle(Img);
            np.XDrag = XDrag;
            np.YDrag = YDrag;
            np.ZDrag = ZDrag;
            np.RDrag = RDrag;
            return np;
        }

        

    }
}
