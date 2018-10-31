namespace Vivid3D.Visuals
{
    public class VRParticle : VRenderer
    {
        public override void Init()
        {
            Layers.Add(new VRLParticle());
        }
    }
}