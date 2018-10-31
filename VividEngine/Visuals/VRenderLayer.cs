using Vivid3D.Data;

namespace Vivid3D.Visuals
{
    public class VRenderLayer
    {
        public VRenderLayer ( )
        {
            Init ( );
        }

        public virtual void Init ( )
        {
        }

        public virtual void Bind ( VMesh m , VVisualizer v )
        {
        }

        public virtual void Render ( VMesh m , VVisualizer v )
        {
        }

        public virtual void Release ( VMesh m , VVisualizer v )
        {
        }
    }
}