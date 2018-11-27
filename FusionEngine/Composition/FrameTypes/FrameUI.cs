using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionEngine.Composition.FrameTypes
{
    public class FrameUI : FrameType
    {
        public FusionEngine.Resonance.UI GUI = null;
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
