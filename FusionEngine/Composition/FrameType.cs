using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using FusionEngine.FrameBuffer;

namespace FusionEngine.Composition
{
    public class FrameType
    {
        public string Name
        {
            get;
            set;
        }

        public bool Regenerate
        {
            get;
            set;
        }

        public List<FrameType> TexBind
        {
            get; set;
        }

        public FrameBufferColor FrameBuffer
        {
            get;
            set;
        }

        public int FrameWidth
        {
            get;
            set;
        }

        public int FrameHeight
        {
            get;
            set;
        }

        public Scene.SceneGraph3D Graph
        {
            get;
            set;
        }

        public PostProcess.VEQuadR QuadFX = null;

        public FrameType ( )
        {
            Regenerate = true;
            FrameWidth = App.AppInfo.W;
            FrameHeight = App.AppInfo.H;
            FrameBuffer = new FrameBufferColor ( FrameWidth, FrameHeight );
            QuadFX = new PostProcess.VEQuadR ( );
            GenQuad ( );
            TexBind = new List<FrameType> ( );
            //FrameBuffer
        }

        public int qva = 0, qvb = 0;

        public void DrawQuad ( )
        {
            // FB.BB.Bind ( 0 );

            // QuadFX.Bind ( );

            GL.BindVertexArray ( qva );

            GL.BindBuffer ( BufferTarget.ArrayBuffer, qvb );
            GL.EnableVertexAttribArray ( 0 );
            GL.VertexAttribPointer ( 0, 3, VertexAttribPointerType.Float, false, 0, 0 );

            GL.DrawArrays ( PrimitiveType.Triangles, 0, 6 );

            GL.DisableVertexAttribArray ( 0 );
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            //QuadFX.Release ( );

            //FB.BB.Release ( 0 );
        }

        public void GenQuad ( )
        {
            qva = GL.GenVertexArray ( );

            GL.BindVertexArray ( qva );

            float[] qd = new float[18];

            qd [ 0 ] = -1.0f;
            qd [ 1 ] = -1.0f;
            qd [ 2 ] = 0.0f;
            qd [ 3 ] = 1.0f; qd [ 4 ] = -1.0f; qd [ 5 ] = 0.0f;
            qd [ 6 ] = -1.0f; qd [ 7 ] = 1.0f; qd [ 8 ] = 0.0f;
            qd [ 9 ] = -1.0f; qd [ 10 ] = 1.0f; qd [ 11 ] = 0.0f;
            qd [ 12 ] = 1.0f; qd [ 13 ] = -1.0f; qd [ 14 ] = 0.0f;
            qd [ 15 ] = 1.0f; qd [ 16 ] = 1.0f; qd [ 17 ] = 0.0f;

            qvb = GL.GenBuffer ( );
            GL.BindBuffer ( BufferTarget.ArrayBuffer, qvb );
            GL.BufferData ( BufferTarget.ArrayBuffer, new IntPtr ( 18 * 4 ), qd, BufferUsageHint.StaticDraw );
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void BindTarget ( )
        {
            FrameBuffer.Bind ( );
        }

        public void ReleaseTarget ( )
        {
            FrameBuffer.Release ( );
        }

        public virtual void Generate ( )
        {
        }

        public void BindTex ( )
        {
            int tn=0;
            foreach ( FrameType tex in TexBind )
            {
                tex.FrameBuffer.BB.Bind ( tn );
                tn++;
            }
        }

        public void ReleaseTex ( )
        {
            int tn=0;
            foreach ( FrameType tex in TexBind )
            {
                tex.FrameBuffer.BB.Release ( tn );
                tn++;
            }
        }

        public void Present ( )
        {
            FrameBuffer.BB.Bind ( 0 );
            QuadFX.Bind ( );
            DrawQuad ( );
            QuadFX.Release ( );
            FrameBuffer.BB.Release ( 0 );
        }
    }
}