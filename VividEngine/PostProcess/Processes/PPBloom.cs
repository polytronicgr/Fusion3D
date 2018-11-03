using Vivid3D.Effect;
using Vivid3D.Texture;

namespace Vivid3D.PostProcess.Processes
{
    public class PPBloom : VPostProcess
    {
        public VEExtract BExtract;
        public VEBloom BBloom;

        public float MinLevel = 0.2f;

        public override void Init ( )
        {
            BExtract = new VEExtract ( );
            BBloom = new VEBloom ( );
            NeedsPostRender = true;
        }

        public override void Bind ( VTex2D bb )
        {
            bb.Bind ( 0 );
            BExtract.MinLevel = MinLevel;
            BExtract.Bind ( );
        }

        public override void Render ( VTex2D bb )
        {
            DrawQuad ( );
        }

        public override void Release ( VTex2D bb )
        {
            BExtract.Release ( );
            bb.Release ( 0 );
        }

        private VTex2D pi = null;

        public override void PostBind ( VTex2D bb )
        {
            pi = ImageProcessing.ImageProcessor.BlurImage ( bb, 1f );
        }

        public override void PostRender ( VTex2D bb )
        {
            BBloom.Bind ( );

            Vivid3D.PostProcess.PostProcessRender.PBuf.BB.Bind ( 0 );
            pi.Bind ( 1 );

            DrawQuad ( );

            pi.Release ( 1 );
            Vivid3D.PostProcess.PostProcessRender.PBuf.BB.Release ( 0 );
            BBloom.Release ( );
        }
    }

    public class VEExtract : Effect3D
    {
        public float MinLevel = 0.7f;

        public VEExtract ( ) : base ( "", "Data\\Shader\\extractVS.glsl", "Data\\Shader\\extractFS.glsl" )
        {
        }

        public override void SetPars ( )
        {
            SetTex ( "tR", 0 );
            SetFloat ( "MinLevel", MinLevel );
        }
    }

    public class VEBloom : Effect3D
    {
        public VEBloom ( ) : base ( "", "Data\\Shader\\bloomVS.glsl", "Data\\Shader\\bloomFS.glsl" )
        {
        }

        public override void SetPars ( )
        {
            SetTex ( "tR1", 0 );
            SetTex ( "tR2", 1 );
            // SetFloat ( "blur", Blur );
        }
    }
}