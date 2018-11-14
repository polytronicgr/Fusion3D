namespace Fusion3D.Resonance.Forms
{
    public class ImageForm : UIForm
    {
        public ImageForm ( )
        {
            Col = new OpenTK.Vector4 ( 1, 1, 1, 1 );
            void DrawFunc ( )
            {
                if ( Peak )
                {
                    if ( Refract )
                    {
                        DrawFormBlurRefract ( CoreTex, NormTex, Blur, Col, RefractV );
                    }
                    else
                    {
                        DrawFormBlur ( CoreTex, Blur, Col );
                    }
                }
                else
                {
                    DrawForm ( CoreTex );
                }
            }

            Draw = DrawFunc;
        }
    }
}