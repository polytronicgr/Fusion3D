using Fusion3D.Data;
using Fusion3D.Effect;

namespace Fusion3D.Visuals
{
    public class RLDepth : RenderLayer
    {
        public FXDepth3D fx = null;

        public override void Init ( )
        {
            fx = new FXDepth3D ( );
        }

        public override void Render ( Mesh3D m, Visualizer v )
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