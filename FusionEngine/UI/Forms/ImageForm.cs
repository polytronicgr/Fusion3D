namespace FusionEngine.Resonance.Forms
{
    public class ImageForm : UIForm
    {
        public ImageForm ( )
        {
            MouseDown = (b) =>
            {

                Click?.Invoke(0);

            };
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