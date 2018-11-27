using FusionEngine.Effect;
using FusionEngine.Texture;

namespace FusionEngine.PostProcess.Processes
{
    public class PPMotionBlur : VPostProcess
    {
        public EVelBuf BFX = null;
        public EMotionBlur RFX = null;

        //public float Blur = 0.7f;
        public FrameBuffer.FrameBufferColor VelFBO;

        public override void Init ( )
        {
            BFX = new EVelBuf ( );
            RFX = new EMotionBlur ( );
            VelFBO = new FrameBuffer.FrameBufferColor ( 512, 512 );
        }

        public override void Bind ( Texture2D bb )
        {
            VelFBO.Bind ( );

            FXG.FXOverride = BFX;

            PostProcessRender.Active.Scene.RenderNoLights ( );
            VelFBO.Release ( );
            FXG.FXOverride = null;

            //bb.Bind(0);
            //BFX.Bind();
            bb.Bind ( 0 );
            VelFBO.BB.Bind ( 1 );

            RFX.Bind ( );
        }

        public override void Render ( Texture2D bb )
        {
            //return;
            DrawQuad ( );
        }

        public override void Release ( Texture2D bb )
        {
            RFX.Release ( );
            bb.Release ( 0 );
            VelFBO.BB.Release ( 1 );
            // BFX.Release();

            // bb.Release(0);
        }
    }

    public class EMotionBlur : Effect3D
    {
        public float Blur = 0.5f;

        public EMotionBlur ( ) : base ( "", "data/Shader/vsMotionBlur.glsl", "data/Shader/fsMotionBlur.glsl" )
        {
        }

        public override void SetPars ( )
        {
            SetTex ( "colorBuffer", 0 );
            SetTex ( "velocityMap", 1 );
        }
    }

    public class EVelBuf : Effect3D
    {
        public float DC = 0;

        public EVelBuf ( ) : base ( "", "data/Shader/vsVelBuf.glsl", "data/Shader/fsVelBuf.glsl" )
        {
        }

        public override void SetPars ( )
        {
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat ( "model", Effect.FXG.Local );
            SetMat ( "view", FXG.Cam.CamWorld );
            SetMat ( "proj", FXG.Cam.ProjMat );
            SetMat ( "pview", FXG.Cam.PrevCamWorld );
            SetMat ( "pmodel", FXG.PrevLocal );

            //SetTex("tC", 0)
        }
    }
}