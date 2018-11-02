using Vivid3D.Data;

namespace Vivid3D.Visuals
{
    public class VRLSimple : VRenderLayer
    {
        public override void Init ( )
        {
        }

        public override void Render ( VMesh m, VVisualizer v )
        {
            v.SetMesh ( m );
            v.Visualize ( );
        }
    }
}