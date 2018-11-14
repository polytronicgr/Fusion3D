using Fusion3D.Data;

namespace Fusion3D.Visuals
{
    public class Visualizer
    {
        public VertexData<float> dat = null;
        public Mesh3D md = null;
        public int Vertices = 0, Indices = 0;

        public Visualizer ( int vc, int ic )
        {
            Vertices = vc;
            Indices = ic;
        }

        public virtual void SetData ( VertexData<float> d )
        {
        }

        public virtual void SetMesh ( Mesh3D m )
        {
        }

        public virtual void FinalAnim ( )
        {
        }

        public virtual void Final ( )
        {
        }

        public virtual void Init ( )
        {
        }

        public virtual void Bind ( )
        {
        }

        public virtual void Update ( )
        {
        }

        public virtual void Visualize ( int sub )
        {
        }

        public virtual void Visualize ( )
        {
        }

        public virtual void Release ( )
        {
        }

        public virtual void Clean ( )
        {
        }
    }
}