using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion3D.Resonance;
namespace Fusion3D.Resonance.Forms
{
    public class LabelForm : UIForm 
    {

        public LabelForm()
        {
            void EV_Draw()
            {
                DrawText(Text, 3, 3);
            }

            Draw = EV_Draw;

        }

    }
}
