using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace Vivid3D.PostProcess
{
    public class VPostProcess
    {
        public VPostProcess()
        {
            Init();
            GenQuad();
        }
        public virtual void Init()
        {

        }
        public virtual void Bind(VTex2D bb)
        {

        }
        public virtual void Render(VTex2D bb)
        {

        }
        public virtual void Release(VTex2D bb)
        {

        }
        public int qva = 0, qvb = 0;
        public void DrawQuad()
        {
           

          

            GL.BindVertexArray(qva);

            GL.BindBuffer(BufferTarget.ArrayBuffer, qvb);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            GL.DisableVertexAttribArray(0);
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      
        }
        public void GenQuad()
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
