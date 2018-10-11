using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Vivid3D.Texture;
using Vivid3D.App;
namespace Vivid3D.FrameBuffer
{
    public class VFrameBuffer
    {
        public int FBO = 0;
        public VTex2D BB;
        public VTexDepth DB;
        public int IW, IH;
        public int DRB = 0;

        public VFrameBuffer(int w,int h)
        {
            IW = w;
            IH = h;
            FBO = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            BB = new VTex2D(w, h, false);
            DB = new VTexDepth(w, h);
            DRB = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, DRB);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, w, h);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, DRB);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, BB.ID, 0);
            DrawBuffersEnum db = DrawBuffersEnum.ColorAttachment0;
            GL.DrawBuffers(1, ref db);
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Console.WriteLine("Framebuffer failure.");
                while(true)
                {
                   
                }
            }
            Console.WriteLine("Framebuffer success.");
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

        }
        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);
            GL.Viewport(0, 0, IW, IH);
            AppInfo.RW = IW;
            AppInfo.RH = IH;
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);



        }
        public void Release()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, AppInfo.W, AppInfo.H);
            AppInfo.RW = AppInfo.W;
            AppInfo.RH = AppInfo.H;
        }
    }
}
