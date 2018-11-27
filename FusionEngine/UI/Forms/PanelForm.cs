using FusionEngine.Texture;

namespace FusionEngine.Resonance.Forms
{
    public class PanelForm : UIForm
    {
        public Texture2D Tex = null;

        public PanelForm ( )
        {
            Tex = new Texture2D ( "data/UI/panel.png", LoadMethod.Single, false );

            void DrawFunc ( )
            {
                DrawForm ( Tex );
            }

            Draw = DrawFunc;
        }
    }
}