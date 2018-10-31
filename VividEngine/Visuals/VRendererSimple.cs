namespace Vivid3D.Visuals
{
    public class VRendererSimple : VRenderer
    {
        public VRendererSimple()
        {
        }

        public override void Init()
        {
            Layers.Add(new VRLSimple());
        }
    }
}