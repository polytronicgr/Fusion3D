using FusionEngine.Data;
using FusionEngine.Effect;

namespace FusionEngine.Visuals
{
    public class RLParticle : RenderLayer
    {
        public FXParticle fx = null;

        public override void Init ( )
        {
            fx = new FXParticle ( );
        }

        public override void Render ( Mesh3D m, Visualizer v )
        {
            m.Mat.Bind ( );
            // Lighting.GraphLight3D.Active.ShadowFB.Cube.Bind(2);
            fx.Bind ( );
            v.SetMesh ( m );
            v.Bind ( );
            v.Visualize ( );
            v.Release ( );
            fx.Release ( );
            //Lighting.GraphLight3D.Active.ShadowFB.Cube.Release(2);
            m.Mat.Release ( );
        }
    }
}