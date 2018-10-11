using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Data;
namespace Vivid3D.Visuals
{
    public class VRenderer
    {
        public List<VRenderLayer> Layers = new List<VRenderLayer>();
        public VRLDepth RLD = null;
        public VRenderer()
        {
            Init();
            RLD = new VRLDepth();
        }
        public virtual void Init()
        {

        }
        public virtual void Bind(VMesh m)
        {

        }
        public virtual void Render(VMesh m)
        {
            foreach (VRenderLayer rl in Layers)
            {
                rl.Render(m, m.Viz);
            }
        }
        public virtual void RenderDepth(VMesh m)
        {
            RLD.Render(m, m.Viz);
        }
        public virtual void Release(VMesh m)
        {

        }
    }
}
