﻿using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace Vivid3D.Composition
{
    public class Composite
    {
        public List<Compositer> Composites
        {
            get;
            set;
        }

        public Vivid3D.Scene.SceneGraph3D Graph
        {
            get;
            set;
        }

        private  PostProcess.VEQuadR QuadFX;

        public Composite ( )
        {
            Composites = new List<Compositer> ( );
            QuadFX = new PostProcess.VEQuadR ( );
            GenQuad ( );
        }

        public void SetGraph ( Scene.SceneGraph3D graph )
        {
            Graph = graph;
            foreach ( Compositer cos in Composites )
            {
                cos.Graph = graph;
            }
        }

        public void AddCompositer ( Compositer cos )
        {
            cos.Graph = Graph;
            Composites.Add ( cos );
        }

        public void Render ( )
        {
            foreach ( Compositer cos in Composites )
            {
                cos.Process ( );
                // cos.PresentFrame ( 0 );
            }

            foreach ( Compositer cos in Composites )
            {
                switch ( cos.Blend )
                {
                    case FrameBlend.Add:
                        GL.Enable ( EnableCap.Blend );
                        GL.BlendFunc ( BlendingFactor.One, BlendingFactor.One );
                        break;
                }

                cos.OutputFrame.FrameBuffer.BB.Bind ( 0 );

                DrawQuad ( );

                cos.OutputFrame.FrameBuffer.BB.Release ( 0 );
            }
        }

        //Composites [ 1 ].PresentFrame ( 1 );

        public int qva = 0, qvb = 0;

        public void DrawQuad ( )
        {
            // FB.BB.Bind ( 0 );

            QuadFX.Bind ( );

            GL.BindVertexArray ( qva );

            GL.BindBuffer ( BufferTarget.ArrayBuffer, qvb );
            GL.EnableVertexAttribArray ( 0 );
            GL.VertexAttribPointer ( 0, 3, VertexAttribPointerType.Float, false, 0, 0 );

            GL.DrawArrays ( PrimitiveType.Triangles, 0, 6 );

            GL.DisableVertexAttribArray ( 0 );
            // GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            QuadFX.Release ( );

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
    }
}