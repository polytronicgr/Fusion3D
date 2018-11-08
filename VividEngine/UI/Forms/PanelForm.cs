using Vivid3D.Texture;

namespace Vivid3D.Resonance.Forms
{
    public class PanelForm : UIForm
    {
        public VTex2D Tex = null;

        public PanelForm ( )
        {
            Tex = new VTex2D ( "data/UI/panel.png", LoadMethod.Single, false );

            void DrawFunc ( )
            {
                DrawForm ( Tex );
            }

            Draw = DrawFunc;
        }
    }
}