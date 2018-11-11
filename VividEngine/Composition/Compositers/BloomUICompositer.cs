using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.Composition.Compositers
{
    public class BloomUICompositer : Compositer
    {
        public BloomUICompositer() : base(1)
        {
            InputFrame = new FrameTypes.FrameUI();

            Types[0] = new FrameTypes.FrameEffect();


            dynamic fe = Types[0];

            Types[0].TexBind.Add(InputFrame);

            fe.FX = new PostProcess.Processes.VEExtract();

            fe.FX.MinLevel = 0.1f;

            //  Types[1].TexBind.Add(InputFrame);
            OutputFrame = Types[0];
        
        }

    }
}
