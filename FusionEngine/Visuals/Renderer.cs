using System.Collections.Generic;
using FusionEngine.Data;

namespace FusionEngine.Visuals
{
    public class Renderer
    {
        public List<RenderLayer> Layers = new List<RenderLayer>();
        public RLDepth RLD = null;

        public Renderer ( )
        {
            Init ( );
            RLD = new RLDepth ( );
        }

        public virtual void Init ( )
        {
        }

        public virtual void Bind ( Mesh3D m )
        {
        }

        public virtual void Render ( Mesh3D m )
        {
            foreach ( RenderLayer rl in Layers )
            {
                rl.Render ( m, m.Viz );
            }
        }

        public virtual void RenderDepth ( Mesh3D m )
        {
            RLD.Render ( m, m.Viz );
        }

        public virtual void Release ( Mesh3D m )
        {
        }
    }
}