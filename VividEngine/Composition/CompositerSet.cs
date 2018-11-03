using System.Collections.Generic;

namespace Vivid3D.Composition
{
    public class CompositerSet
    {
        public List<Compositer> Composites = new List<Compositer>();

        public CompositerSet ( params Compositer [ ] com )
        {
            Composites.AddRange ( com );
        }

        public Composite Get ( )
        {
            Composite c = new Composite();
            foreach ( Compositer cc in Composites )
            {
                c.AddCompositer ( cc );
            }
            return c;
        }
    }
}