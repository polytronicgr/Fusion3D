using Vivid3D.Data;
using Vivid3D.Effect;

namespace Vivid3D.Visuals
{
    public class VRLDepth : VRenderLayer
    {
        public EDepth3D fx = null;

        public override void Init ( )
        {
            fx = new EDepth3D ( );
        }

        public override void Render ( VMesh m , VVisualizer v )
        {
            // m.Mat.Bind();
            fx.Bind ( );
            v.SetMesh ( m );
            v.Bind ( );
            v.Visualize ( );
            v.Release ( );
            fx.Release ( );
            //m.Mat.Release();
        }
    }
}