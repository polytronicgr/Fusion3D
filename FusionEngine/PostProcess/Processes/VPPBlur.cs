using Fusion3D.Effect;
using Fusion3D.Texture;

namespace Fusion3D.PostProcess.Processes
{
    public class VPPBlur : VPostProcess
    {
        public VEBlur BFX = null;
        public float Blur = 0.7f;

        public override void Init ( )
        {
            BFX = new VEBlur ( );
            NeedsPostRender = false;
        }

        public override void Bind ( Texture2D bb )
        {
            BFX.Blur = Blur;
            bb.Bind ( 0 );
            BFX.Bind ( );
        }

        public override void Render ( Texture2D bb )
        {
            DrawQuad ( );
        }

        public override void Release ( Texture2D bb )
        {
            BFX.Release ( );
            bb.Release ( 0 );
        }
    }

    public class VEBlur : Effect3D
    {
        public float Blur = 0.5f;

        public VEBlur ( ) : base ( "", "data/Shader/blurVS.txt", "data/Shader/blurFS.txt" )
        {
        }

        public override void SetPars ( )
        {
            SetTex ( "tR", 0 );
            SetFloat ( "blur", Blur );
        }
    }
}