using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using Vivid3D.Effect;
using Vivid3D.Texture;
using Vivid3D.App;
namespace Vivid3D.Draw
{
   public class VEQuadBlur2 : Effect3D
    {
        public Vector4 Col = Vector4.One;
        public float Blur = 0.2f;
        public VEQuadBlur2() : base("", "Data\\Shader\\blur2VS.glsl", "Data\\Shader\\blur2FS.glsl")
        {

        }
        public override void SetPars()
        {
            SetTex("tR", 0);
            SetVec4("col", Col);
            SetFloat("blur", Blur);
            SetMat("proj", Matrix4.CreateOrthographicOffCenter(0, AppInfo.RW, AppInfo.RH, 0, -1, 1));
        }
    }
    public class VEQuadBlur : Effect3D
    {
        public Vector4 Col = Vector4.One;
        public float Blur = 0.2f;
        public float Refract = 0.25f;
        public VEQuadBlur() : base("","Data\\Shader\\blurVS.glsl","Data\\Shader\\blurFS.glsl")
        {

        }
        public override void SetPars()
        {
            SetTex("tR", 0);
            SetTex("tB", 1);
            SetTex("tN", 2);
            SetFloat("blur", Blur);
            SetFloat("refract", Refract);
            SetBool("refractOn", Refract > 0);
            SetVec4("col", Col);
            SetMat("proj", Matrix4.CreateOrthographicOffCenter(0, AppInfo.RW, AppInfo.RH, 0, -1, 1));
        }
    }
    public class VEQuad : Effect3D
    {
        public Vector4 Col = Vector4.One;
        public VEQuad() : base("", "Data\\Shader\\drawVS.txt", "Data\\Shader\\drawFS.txt")
        {

        }
        public override void SetPars()
        {
            SetTex("tR", 0);
            SetVec4("col", Col);
            SetMat("proj",Matrix4.CreateOrthographicOffCenter(0, AppInfo.RW, AppInfo.RH, 0, -1,1));
       
           // Console.WriteLine("W:" + AppInfo.RW + " H:" + AppInfo.RH);
        }
    }
    public enum VBlend
    {
        Solid,Alpha,Additive,Modulate,ModulateX2,ModulateX4,Subtract,Burn
    }
    public static class VPen
    {
        public static Color4 ForeCol = Color4.White;
        public static Color4 BackCol = Color4.Black;
        public static VBlend BlendMod = VBlend.Solid;
        public static Matrix4 DrawMat = Matrix4.Identity;
        public static Matrix4 PrevMat = Matrix4.Identity;
        public static VEQuad QFX = null;
        public static VEQuadBlur BFX = null;
        public static VEQuadBlur2 BFX2 = null;
        public static int qva = -1, qvb = -1;
        public static VTex2D WhiteTex = null;
        public static void InitDraw()
        {
            QFX = new VEQuad();
            BFX = new VEQuadBlur();
            BFX2 = new VEQuadBlur2();
            WhiteTex = new VTex2D("Data\\ui\\skin\\white.png", LoadMethod.Single);
        }
        public static void DraqQuadBlur2()
        {
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            GL.Viewport(0, 0, AppInfo.W, AppInfo.H);
            //  GL.Disable(EnableCap.Blend);
            //GL.Disable(EnableCap.)

            //    WhiteTex.Bind(0);

            //BFX.Refract = refract;
   //         Console.WriteLine("R2:" + refract);
 //           BFX.Blur = blur;

            BFX2.Bind();


            GL.BindVertexArray(qva);

            GL.BindBuffer(BufferTarget.ArrayBuffer, qvb);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * 4, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 9 * 4, 5 * 4);

            uint[] ind = new uint[4];
            ind[0] = 0;
            ind[1] = 1;
            ind[2] = 2;
            ind[3] = 3;
            GL.DrawElements<uint>(PrimitiveType.Quads, 4, DrawElementsType.UnsignedInt, ind);
            //GL.DrawArrays(PrimitiveType.Quads, 0, 4);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            BFX2.Release();

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
        }
        public static void DrawQuadBlur(float blur,float refract = 0)
        {
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            GL.Viewport(0, 0, AppInfo.W, AppInfo.H);
            //  GL.Disable(EnableCap.Blend);
            //GL.Disable(EnableCap.)

            //    WhiteTex.Bind(0);

            BFX.Refract = refract;
          
            BFX.Blur = blur;

            BFX.Bind();


            GL.BindVertexArray(qva);

            GL.BindBuffer(BufferTarget.ArrayBuffer, qvb);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * 4, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 9 * 4, 5 * 4);

            uint[] ind = new uint[4];
            ind[0] = 0;
            ind[1] = 1;
            ind[2] = 2;
            ind[3] = 3;
            GL.DrawElements<uint>(PrimitiveType.Quads, 4, DrawElementsType.UnsignedInt, ind);
            //GL.DrawArrays(PrimitiveType.Quads, 0, 4);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            BFX.Release();

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

        }
        public static void DrawQuad()
        {
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);
         
            GL.Viewport(0, 0, AppInfo.W, AppInfo.H);
          //  GL.Disable(EnableCap.Blend);
            //GL.Disable(EnableCap.)

            //    WhiteTex.Bind(0);

            QFX.Bind();

       
            GL.BindVertexArray(qva);

            GL.BindBuffer(BufferTarget.ArrayBuffer, qvb);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9*4, 0);
              GL.EnableVertexAttribArray(1);
              GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 9 * 4, 3 * 4);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 9 * 4, 5 * 4);

            uint[] ind = new uint[4];
            ind[0] = 0;
            ind[1] = 1;
            ind[2] = 2;
            ind[3] = 3;
            GL.DrawElements<uint>(PrimitiveType.Quads, 4, DrawElementsType.UnsignedInt, ind);
            //GL.DrawArrays(PrimitiveType.Quads, 0, 4);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            QFX.Release();

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

          //  WhiteTex.Release(0);

        }
        public static void Line(int x,int y,int x2,int y2)
        {
            Line(x, y, x2, y2, Vector4.One);
        }
        public static void Line(int x,int y,int x2,int y2,Vector4 c)
        {
            Line(x, y,x2,y2, c, c);
        }
        public static void Line(int x,int y,int x2,int y2,Vector4 c1,Vector4 c2)
        {
            float a1 = x;
            float b1 = y;
            float a2 = x2;
            float b2 = y2;

            float steps = 0;
            float d1 = Math.Abs(a2 - a1);
            float d2 = Math.Abs(b2 - b1);
            if (d1 > d2)
            {
                steps = d1;
            }
            else
            {
                steps = d2;
            }
            float xi = (a2 - a1) / steps;
            float yi = (b2 - b1) / steps;
            float dx = a1;
            float dy = b1;
            WhiteTex.Bind(0);
            Vector4 vc = Vector4.One;
            float cr1 = c1.X;
            float cg1 = c1.Y;
            float cb1 = c1.Z;
            float ca1 = c1.W;
            float ri = (c2.X - cr1) / steps;
            float gi = (c2.Y - cg1) / steps;
            float bi = (c2.Z - cb1) / steps;
            float ai = (c2.W - ca1) / steps;

            xi *= 2;
            yi *= 2;
            ri *= 2;
            gi *= 2;
            bi *= 2;
            ai *= 2;

            for (int i=0;i<steps;i+=2)
            {

                // RectRaw((int)dx,(int) dy, 2, 2, Vector4.One,Vector4.One);

                vc.X = cr1;
                vc.Y = cg1;
                vc.Z = cb1;
                vc.W = ca1;
                GenQuad((int)dx, (int)dy, 2,2,vc,vc);

                DrawQuad();

                dx += xi;
                dy += yi;
                cr1 += ri;
                cg1 += gi;
                cb1 += bi;
                ca1 += ai;
            }
            WhiteTex.Release(0);
        }
        public static void GenQuad(int x, int y, int w, int h, Vector4 c1, Vector4 c2)
        {


            if (qva == -1)
            {
                qva = GL.GenVertexArray();
            }
            GL.BindVertexArray(qva);

            float z = 0;

            float[] qd = new float[36];

            qd[0] = x;
            qd[1] = y;
            qd[2] = z;
            qd[3] = 0;
            qd[4] = 0;
            qd[5] = c1.X;
            qd[6] = c1.Y;
            qd[7] = c1.Z;
            qd[8] = c1.W;


            qd[9] = x+w;
            qd[10] = y;
            qd[11] = z;
            qd[12] = 1;
            qd[13] = 0;
            qd[14] = c1.X;
            qd[15] = c1.Y;
            qd[16] = c1.Z;
            qd[17] = c1.W;


            qd[18] = x+w;
            qd[19] = y+h;
            qd[20] = z;
            qd[21] = 1;
            qd[22] = 1;
            qd[23] = c2.X;
            qd[24] = c2.Y;
            qd[25] = c2.Z;
            qd[26] = c2.W;


            qd[27] = x;
            qd[28] = y+h;
            qd[29] = z;
            qd[30] = 0;
            qd[31] = 1;
            qd[32] = c2.X;
            qd[33] = c2.Y;
            qd[34] = c2.Z;
            qd[35] = c2.W;



            /*
            qd[20] = x;
            qd[21] = y+h;
            qd[22] = z;
            qd[23] = 0;
            qd[24] = 1;

            qd[25] = x;
            qd[26] = y;
            qd[27] = z;
            qd[28] = 0;
            qd[29] = 0;
            */


            if (qvb == -1)
            {
                qvb = GL.GenBuffer();
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, qvb);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(36 * 4), qd, BufferUsageHint.StaticDraw);
            //  GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        }
        public static void SetProj(int x,int y,int w,int h)
        {
            DrawMat = Matrix4.CreateOrthographicOffCenter(x, x + w, y + h, y, 0, 1);
        }
        public static void Bind()
        {
        //    GL.Color4(ForeCol);
            switch(BlendMod)
            {
                case VBlend.Solid:
                    GL.Disable(EnableCap.Blend);
                    break;
                case VBlend.Alpha:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    break;
            }
            
          //  GL.MatrixMode(MatrixMode.Projection);
          //  GL.LoadMatrix(ref DrawMat);
           // GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

        }
        public static void Release()
        {

        }
        public static void Rect(int x,int y,int w,int h,VTex2D img)
        {
            Rect(x, y, w, h, img, Vector4.One);
        }
        public static void Rect(int x,int y,int w,int h,VTex2D img,Vector4 c)
        {
            Rect(x, y, w, h, img, c, c);
        }
        public static void RectRaw(int x,int y,int w,int h,Vector4 t1,Vector4 t2)
        {
            GenQuad(x, y, w, h, t1, t2);
        
            DrawQuad();
       
            // GL.Begin(BeginMode.Quads);
            // GL.Vertex2(x, y);
            //GL.Vertex2(x + width, y);
            //GL.Vertex2(x + width, y + height);
            //GL.Vertex2(x, y + height);
            //GL.End();
         
        }
        public static void RectBlurRefract(int x,int y,int w,int h,VTex2D img,VTex2D bimg,VTex2D nimg,Vector4 tc,Vector4 bc,float blur,float refract)
        {
            BFX.Col = tc;
           
            Bind();
            GenQuad(x, y, w, h, tc, bc);
            img.Bind(0);
            bimg.Bind(1);
            nimg.Bind(2);
            DrawQuadBlur(blur,refract);
            nimg.Release(2);
            bimg.Release(1);
            img.Release(0);
            // GL.Begin(BeginMode.Quads);
            // GL.Vertex2(x, y);
            //GL.Vertex2(x + width, y);
            //GL.Vertex2(x + width, y + height);
            //GL.Vertex2(x, y + height);
            //GL.End();
            Release();
        }
        public static void RectBlur2(int x,int y,int w,int h,VTex2D img,Vector4 col,float blur)
        {
            BFX2.Col = col;
            BFX2.Blur = blur;
            Bind();
            GenQuad(x, y, w, h, col, col);
            img.Bind(0);
            DraqQuadBlur2();
            img.Release(0);
            Release();
        }
        public static void RectBlur(int x,int y,int w,int h,VTex2D img,VTex2D bimg,Vector4 tc,Vector4 bc,float blur)
        {
            BFX.Col = tc;
            BFX.Refract = 0;
            Bind();
            GenQuad(x, y, w, h, tc, bc);
            img.Bind(0);
            bimg.Bind(1);
            DrawQuadBlur(blur);
            bimg.Release(1);
            img.Release(0);
            // GL.Begin(BeginMode.Quads);
            // GL.Vertex2(x, y);
            //GL.Vertex2(x + width, y);
            //GL.Vertex2(x + width, y + height);
            //GL.Vertex2(x, y + height);
            //GL.End();
            Release();
        }
        public static void Rect(int x,int y,int w,int h,VTex2D img,Vector4 tc,Vector4 bc)
        {
            QFX.Col = tc;
             Bind();
            GenQuad(x, y, w, h, tc, bc);
            img.Bind(0);
            DrawQuad();
            img.Release(0);
            // GL.Begin(BeginMode.Quads);
            // GL.Vertex2(x, y);
            //GL.Vertex2(x + width, y);
            //GL.Vertex2(x + width, y + height);
            //GL.Vertex2(x, y + height);
            //GL.End();
            Release();
        }
        public static void Rect(int x,int y,int w,int h,Vector4 tc,Vector4 bc)
        {
            QFX.Col = tc;
            Bind();
            GenQuad(x, y, w, h, tc, bc);
            WhiteTex.Bind(0);
            DrawQuad();
            WhiteTex.Release(0);
            // GL.Begin(BeginMode.Quads);
            // GL.Vertex2(x, y);
            //GL.Vertex2(x + width, y);
            //GL.Vertex2(x + width, y + height);
            //GL.Vertex2(x, y + height);
            //GL.End();
            Release();
        }
        public static void Rect(int x,int y,int width,int height,Vector4 col)
        {
            Rect(x, y, width, height, col, col);
        }
        public static void Rect(float x,float y,float w,float h,Vector4 col)
        {
            Rect((int)x, (int)y, (int)w, (int)h, col);
        }
        public static void Rect(float x,float y,float w,float h,Vector4 tc,Vector4 bc)
        {
            Rect((int)x, (int)y, (int)w, (int)h, tc, bc);
        }
        public static void Rect(float x,float y,float w,float h,VTex2D img)
        {
            Rect((int)x, (int)y, (int)w, (int)h, img);
        }
        public static void Rect(float x,float y,float w,float h,VTex2D img,Vector4 col)
        {
            Rect((int)x, (int)y, (int)w, (int)h, img, col);
        }
        public static void Rect(float x,float y,float w,float h,VTex2D img,Vector4 tc,Vector4 bc)
        {
            Rect((int)x, (int)y, (int)w, (int)h, img, tc, bc);
        }
        public static void Line(float x,float y,float x2,float y2)
        {
            Line((int)x, (int)y, (int)x2, (int)y2);
        }
        public static void Line(float x,float y,float x2,float y2,Vector4 col)
        {
            Line((int)x, (int)y, (int)x2, (int)y2, col);
        }
        public static void Line(float x,float y,float x2,float y2,Vector4 c1,Vector4 c2)
        {
            Line((int)x, (int)y, (int)x2, (int)y2, c1, c2);
        }
    }
}
