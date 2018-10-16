using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using Vivid3D.FrameBuffer;
using Vivid3D.Scene;
using OpenTK.Graphics.OpenGL4;
namespace Vivid3D.PostProcess.Processes
{
    public class PPOutLine : VPostProcess
    {
        public VEOutLine BFX = null;
        public float Blur = 0.7f;
        public VFrameBuffer DFB = null;
        public SceneGraph3D OutLineGraph;
        public override void Init()
        {
            BFX = new VEOutLine();
            DFB = new VFrameBuffer(App.AppInfo.W, App.AppInfo.H);
        }
        public override void Bind(VTex2D bb)
        {

            DFB.Bind();
            GL.ClearColor(1, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            OutLineGraph.RenderDepth();

            DFB.Release();

            GL.Disable(EnableCap.Blend);
            

            DFB.BB.Bind(0);
            bb.Bind(1);
            BFX.Bind();

        }
        public override void Render(VTex2D bb)
        {
            DrawQuad();
        }
        public override void Release(VTex2D bb)
        {
            BFX.Release();
            bb.Release(1);
            DFB.BB.Release(0);
        }
    }
    public class VEOutLine : Vivid3D.Effect.Effect3D
    {
        
        public VEOutLine() : base("", "Data\\Shader\\outLineVS.txt", "Data\\Shader\\outLineFS.txt")
        {

        }
        public override void SetPars()
        {
            SetTex("tR", 0);
            SetTex("tB", 1);
        }
    }
}

