using OpenTK.Graphics.OpenGL4;
using Fusion3D.FrameBuffer;
using Fusion3D.Scene;
using Fusion3D.Texture;

namespace Fusion3D.PostProcess.Processes
{
    public class PPOutLine : VPostProcess
    {
        public VEOutLine BFX = null;
        public float Blur = 0.7f;
        public FrameBufferColor DFB = null;
        public SceneGraph3D OutLineGraph;
        public VEQuadR QFX = null;
        public VECombineTex QCT = null;

        public override void Init ( )
        {
            BFX = new VEOutLine ( );
            DFB = new FrameBufferColor ( App.AppInfo.W, App.AppInfo.H );
            QFX = new VEQuadR ( );
            QCT = new VECombineTex ( );
            NeedsPostRender = true;
        }

        private Fusion3D.Texture.Texture2D ib = null;

        public override void Bind ( Texture2D bb )
        {
            ib = bb;
            DFB.Bind ( );
            GL.ClearColor ( 1, 0, 0, 0 );
            GL.Clear ( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );
            OutLineGraph.RenderDepth ( );

            DFB.Release ( );

            GL.Disable ( EnableCap.Blend );

            // var tmp = ImageProcessing.ImageProcessor.BlurImage(DFB.BB, 0.9f);

            DFB.BB.Bind ( 0 );
            bb.Bind ( 1 );
            BFX.Bind ( );
        }

        public override void Render ( Texture2D bb )
        {
            DrawQuad ( );
            //var tm = ImageProcessing.ImageProcessor.BlurImage()
        }

        public override void PostBind ( Texture2D bb )
        {
            pi = ImageProcessing.ImageProcessor.BlurImage ( bb, 0.03f );
        }

        public Fusion3D.Texture.Texture2D pi = null;

        public override void PostRender ( Texture2D bb )
        {
            // var img = ImageProcessing.ImageProcessor.BlurImage(bb, 0.8f);
            QCT.Level = 0.7f;
            QCT.Bind ( );

            Fusion3D.PostProcess.PostProcessRender.PBuf.BB.Bind ( 0 );
            pi.Bind ( 1 );
            DrawQuad ( );
            pi.Release ( 1 );
            Fusion3D.PostProcess.PostProcessRender.PBuf.BB.Release ( 0 );

            QCT.Release ( );
        }

        public override void Release ( Texture2D bb )
        {
            BFX.Release ( );
            bb.Release ( 1 );
            DFB.BB.Release ( 0 );
        }
    }

    public class VEOutLine : Fusion3D.Effect.Effect3D
    {
        public VEOutLine ( ) : base ( "", "data/Shader/outLineVS.txt", "data/Shader/outLineFS.txt" )
        {
        }

        public override void SetPars ( )
        {
            SetTex ( "tR", 0 );
            SetTex ( "tB", 1 );
        }
    }

    public class VECombineTex : Fusion3D.Effect.Effect3D
    {
        public float Level = 0.5f;

        public VECombineTex ( ) : base ( "", "data/Shader/outLinevs.txt", "data/Shader/combineTexFS.txt" )
        {
        }

        public override void SetPars ( )
        {
            SetTex ( "tR1", 0 );
            SetTex ( "tR2", 1 );
            SetFloat ( "level", Level );
        }
    }
}