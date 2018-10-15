using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using Vivid3D.Draw;
using OpenTK;
namespace Vivid3D.Resonance.Forms
{
    public class PanelForm : UIForm
    {

        public VTex2D Tex = null;

        public PanelForm()
        {

            Tex = new VTex2D("Data\\UI\\panel.png", LoadMethod.Single, false);

            void DrawFunc()
            {

                this.DrawForm(Tex);

            }

            Draw = DrawFunc;

        }

    }
}
