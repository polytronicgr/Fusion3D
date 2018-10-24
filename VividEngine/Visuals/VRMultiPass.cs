using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.Visuals
{
    public class VRMultiPass : VRenderer
    {
        public VRMultiPass()
        {

        }
        public override void Init()
        {
            Layers.Add(new VRLMultiPass());
        }
    }
    public class VRNoFx : VRenderer
    {
        public VRNoFx()
        {

        }
        public override void Init()
        {
            Layers.Add(new VRLNoFX());
        }
    }
    public class VRTerrain : VRenderer
    {
        public VRTerrain()
        {

        }
        public override void Init()
        {
            Layers.Add(new VRLTerrain());
        }
    }
    public class VRLightMap : VRenderer
    {
        public VRLightMap()
        {

        }
        public override void Init()
        {
            Layers.Add(new VRLLightMap());
        }
    }
    public class VRMultiPassAnim : VRenderer
    {
        public VRMultiPassAnim()
        {

        }
        public override void Init()
        {
            Layers.Add(new VRLMultiPassAnim());
        }
    }
}
