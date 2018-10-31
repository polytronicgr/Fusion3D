namespace Vivid3D.Material
{
    public class MaterialParticle3D : Material3D
    {
        public override void Bind()
        {
            Active = this;
            TCol.Bind(0);
        }

        public override void Release()
        {
            TCol.Release(0);
            Active = null;
        }
    }
}