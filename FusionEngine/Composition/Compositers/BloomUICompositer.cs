using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion3D.Composition.Compositers
{
    public class BloomUICompositer : Compositer
    {
        public BloomUICompositer() : base(3)
        {
            InputFrame = new FrameTypes.FrameUI();

            Types[0] = new FrameTypes.FrameEffect();


            dynamic fe = Types[0];

            Types[0].TexBind.Add(InputFrame);

            fe.FX = new PostProcess.Processes.VEExtract();

            fe.FX.MinLevel = 0.62f;

            Types[1] = new FrameTypes.FrameEffect();

            dynamic fe2 = Types[1];
            Types[1].TexBind.Add(Types[0]);

            fe2.FX = new PostProcess.Processes.VEBlur();


            fe2.FX.Blur = 0.5f;

            Types[2] = new FrameTypes.FrameEffect();

            dynamic fe3 = Types[2];
            Types[2].TexBind.Add(Types[1]);
            Types[2].TexBind.Add(InputFrame);

            fe3.FX = new PostProcess.Processes.VEBloom();

            

            //  Types[1].TexBind.Add(InputFrame);
            OutputFrame = Types[2];
        
        }

    }
}
