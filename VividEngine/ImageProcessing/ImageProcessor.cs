using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
namespace Vivid3D.ImageProcessing
{
    public static class ImageProcessor
    {
        public static Vivid3D.PostProcess.Processes.VEBlur FXBlur;
        public static Vivid3D.FrameBuffer.VFrameBuffer FB;
        public static int IW, IH;
        public static Vivid3D.PostProcess.VEQuadR QFX = null;
        public static void InitIP()
        {
            FXBlur = new PostProcess.Processes.VEBlur();
            FB = new FrameBuffer.VFrameBuffer(App.AppInfo.W, App.AppInfo.H);
            QFX = new Vivid3D.PostProcess.VEQuadR();
            GenQuad();
        }
        public static Vivid3D.Texture.VTex2D BlurImage(Vivid3D.Texture.VTex2D img,float blur)
        {

            

            FXBlur.Blur = blur;

            FB.Bind();

            FXBlur.Bind();

            //var rt = new Vivid3D.Texture.VTex2D(img.W, img.H, false);
            
            img.Bind(0);

             FXBlur.Bind();

            DrawQuad();

            FXBlur.Release();

            img.Release(0);

          

            FB.Release();

            return FB.BB;

        }

        public static int qva = 0, qvb = 0;
        public static void DrawQuad()
        {
           // FB.BB.Bind(0);

        //    QFX.Bind();

            GL.BindVertexArray(qva);

            GL.BindBuffer(BufferTarget.ArrayBuffer, qvb);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            GL.DisableVertexAttribArray(0);
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
          //  QFX.Release();

            //FB.BB.Release(0);
        }
        public static void GenQuad()
        {

            qva = GL.GenVertexArray();

            GL.BindVertexArray(qva);

            float[] qd = new float[18];

            qd[0] = -1.0f;
            qd[1] = -1.0f;
            qd[2] = 0.0f;
            qd[3] = 1.0f; qd[4] = -1.0f; qd[5] = 0.0f;
            qd[6] = -1.0f; qd[7] = 1.0f; qd[8] = 0.0f;
            qd[9] = -1.0f; qd[10] = 1.0f; qd[11] = 0.0f;
            qd[12] = 1.0f; qd[13] = -1.0f; qd[14] = 0.0f;
            qd[15] = 1.0f; qd[16] = 1.0f; qd[17] = 0.0f;




            qvb = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, qvb);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(18 * 4), qd, BufferUsageHint.StaticDraw);
            //  GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        }
    }
}
