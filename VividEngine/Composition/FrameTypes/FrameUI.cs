using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.Composition.FrameTypes
{
    public class FrameUI : FrameType
    {
        public Vivid3D.Resonance.UI GUI = null;
        public override void Generate()
        {
            if (Regenerate == false)
            {
                return;
            }

            BindTarget();

            GUI.Render();


            ReleaseTarget();
        }

    }
}
