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
using OpenTK.Graphics.OpenGL4;
namespace FoomED.Forms
{
    public class EditMapForm : WindowForm
    {
        public ImageForm IMG;
        public FrameBufferColor FB = null;
        public EditMapForm()
        {
            FB = new FrameBufferColor(1024, 1024);
            IMG = new ImageForm();
            IMG.SetImage(FB.BB);
            //body.Add(IMG);

            RenderMap();
            SubChanged = () =>
            {

                IMG.Set(5, 30, W - 10, H - 80);
                IMG.SetImage(FB.BB);
                if (!body.Forms.Contains(this))
                {
                    body.Add(IMG);
                }
                IMG.MouseDown = (b) =>
                {
                    Root.Forms.Remove(this);
                    Root.Forms.Add(this);
                };
                RenderMap();
            };

        }

        public void RenderMap()
        {
            FB.Bind();
            GL.ClearColor(0, 0, 0.15f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            FB.Release();


        }

    }
}
