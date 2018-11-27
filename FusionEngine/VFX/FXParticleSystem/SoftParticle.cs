using OpenTK;
using FusionEngine.Util;

namespace FusionEngine.VFX
{
    public class SoftParticle : ParticleBase
    {
        public SoftParticle ( Tex.Tex2D img )
        {
            Img = img;
        }

        public override void Render ( )
        {
            Scene.SceneGraph g = VFX.VisualFX.Graph;

            int sw = FusionEngine.App.FusionApp.W;
            int sh = FusionEngine.App.FusionApp.H;

            float[] ox = new float[4];
            float[] oy = new float[4];

            ox [ 0 ] = ( -W / 2 );// * Graph.Z * Z;
            ox [ 1 ] = ( W / 2 );// * Graph.Z * Z;
            ox [ 2 ] = ( W / 2 );// * Graph.Z* Z ;
            ox [ 3 ] = ( -W / 2 );// *Graph.Z*Z;

            oy [ 0 ] = ( -H / 2 );// * Graph.Z*Z;
            oy [ 1 ] = ( -H / 2 );// *Graph.Z*Z;
            oy [ 2 ] = ( H / 2 );// * Graph.Z * Z;
            oy [ 3 ] = ( H / 2 );// * Graph.Z * Z;

            Vector2[] p = Maths.RotateOC(ox, oy, Rot, Z, 0, 0);

            p = Maths.Push ( p, X - g.X, Y - g.Y );

            p = Maths.RotateOC ( p, g.Rot, g.Z, 0, 0 );

            p = Maths.Push ( p, sw / 2, sh / 2 );

            Draw.Render.SetBlend ( Draw.BlendMode.SoftLight );

            Draw.Render.Col = new Vector4 ( 1, 1, 1, Alpha );

            Draw.Render.Image ( p, Img );

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

        public override ParticleBase Clone ( )
        {
            SoftParticle np = new SoftParticle ( Img )
            {
                XDrag = XDrag ,
                YDrag = YDrag ,
                ZDrag = ZDrag ,
                RDrag = RDrag
            };
            return np;
        }
    }
}