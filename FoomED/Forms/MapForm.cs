using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionEngine.Resonance;
using FusionEngine.FrameBuffer;
using FusionEngine.Texture;
using FusionEngine.Draw;
using FusionEngine.Resonance.Forms;
using OpenTK;
using InvaderEng.Map;
using OpenTK.Graphics.OpenGL4;
namespace FoomED.Forms
{
    public class MapForm : UIForm
    {
        public InvaderEng.Map.Map Map;

        public MapForm()
        {

            Draw = () =>
            {

                this.DrawFormSolid(new Vector4(0, 0, 0, 1));

                int cx = W / 2;
                int cy = H / 2;

                foreach(var e in Map.Elements)
                {
                    if (e is MapWall)
                    {
                        var mw = e as MapWall;
                        DrawLine(cx + (int)mw.X1, cy + (int)mw.Z1, cx + (int)mw.X2, cy + (int)mw.Z2, new Vector4(0, 1, 0, 1));
                    }
                }

                DrawFormSolid(new Vector4(1,0,0,1),cx + (int)Map.Cam.X-3, cy + (int)Map.Cam.Z-3, 6, 6);

                float la, ra;

                la = Map.Cam.Rot - 22.5f;
                ra = Map.Cam.Rot + 22.5f;

                la = MathHelper.DegreesToRadians(la);
                ra = MathHelper.DegreesToRadians(ra);

                DrawLine((cx + (int)Map.Cam.X), (cy + (int)Map.Cam.Z), (cx + (int)Map.Cam.X) + (int)(Math.Cos(la) * 120), (cy + (int)Map.Cam.Z) + (int)(Math.Sin(la)*120), new Vector4(0, 1, 0, 1));
                DrawLine((cx + (int)Map.Cam.X), (cy + (int)Map.Cam.Z), (cx + (int)Map.Cam.X) + (int)(Math.Cos(ra) * 120), (cy + (int)Map.Cam.Z) + (int)(Math.Sin(ra) * 120), new Vector4(0, 1, 0, 1));




            };

        }


    }
}
