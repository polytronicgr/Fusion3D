using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Data;
using Vivid3D.Effect;
namespace Vivid3D.Visuals
{
    public class VRLParticle : VRenderLayer
    {
        public EParticle fx = null;
        public override void Init()
        {
            fx = new EParticle();
        }
        public override void Render(VMesh m, VVisualizer v)
        {

            m.Mat.Bind();
           // Lighting.GraphLight3D.Active.ShadowFB.Cube.Bind(2);
            fx.Bind();
            v.SetMesh(m);
            v.Bind();
            v.Visualize();
            v.Release();
            fx.Release();
            //Lighting.GraphLight3D.Active.ShadowFB.Cube.Release(2);
            m.Mat.Release();
        }

    }
}
