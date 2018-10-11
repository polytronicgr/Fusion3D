using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Data;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace Vivid3D.Visuals
{
    public class VVVBO : VVisualizer
    {
        public int vert_vbo, norm_vbo, uv_vbo, bi_vbo, tan_vbo;
        public int va, vbo, eb, tvbo, nbvo, tanvbo, bivbo;
        public VVVBO(int vc,int ic) :base(vc,ic)
        {

        }
        public override void SetData(VVertexData<float> d)
        {
            dat = d;
        }
        public override void SetMesh(VMesh m)
        {
            md = m;
        }
        public override void Final()
        {
            va = GL.GenVertexArray();
            GL.BindVertexArray(va);

            vert_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vert_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices*3*4),md.Vertices, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            uv_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, uv_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 2 * 4), md.UV, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            norm_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, norm_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 3 * 4), md.Norm, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);

            bi_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, bi_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 3 * 4), md.Bi, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, 0);

            tan_vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tan_vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(md.NumVertices * 3 * 4), md.Tan, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(4);
            GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, 0, 0);


            //GL.EnableVertexAttribArray(1);

            /*
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, md.Data.Components * 4, 12);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, md.Data.Components * 4, 20);
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, md.Data.Components * 4, 32);
            GL.EnableVertexAttribArray(4);
            GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, md.Data.Components * 4, 44);
            */
            GL.BindVertexArray(0);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
            GL.DisableVertexAttribArray(3);
            GL.DisableVertexAttribArray(4);

            eb = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eb);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(Indices * 4), md.Indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public override void Bind()
        {
            GL.BindVertexArray(va);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eb);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
           // GL.EnableVertexAttribArray(3);
           // GL.EnableVertexAttribArray(4);

        }
        public override void Release()
        {
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        public override void Visualize()
        {
            GL.DrawElements(BeginMode.Triangles, Indices, DrawElementsType.UnsignedInt, 0);

        }
    }
}
