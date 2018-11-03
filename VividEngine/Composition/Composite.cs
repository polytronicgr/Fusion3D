using System.Collections.Generic;

namespace Vivid3D.Composition
{
    public class Composite
    {
        public List<Compositer> Composites
        {
            get;
            set;
        }

        public Vivid3D.Scene.SceneGraph3D Graph
        {
            get;
            set;
        }

        public Composite ( )
        {
            Composites = new List<Compositer> ( );
        }

        public void SetGraph ( Scene.SceneGraph3D graph )
        {
            Graph = graph;
            foreach ( Compositer cos in Composites )
            {
                cos.Graph = graph;
            }
        }

        public void AddCompositer ( Compositer cos )
        {
            cos.Graph = Graph;
            Composites.Add ( cos );
        }

        public void Render ( )
        {
            foreach ( Compositer cos in Composites )
            {
                cos.Process ( );
                cos.PresentFrame ( 0 );
            }
        }
    }
}