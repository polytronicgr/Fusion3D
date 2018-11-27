using System.Collections.Generic;

namespace FusionEngine.Data
{
    public class VInfoMap<T, D>
    {
        public Dictionary<T, D> Map = new Dictionary<T, D>();

        public virtual void Add ( T key, D data )
        {
            Map.Add ( key, data );
        }

        public virtual bool Has ( T key )
        {
            return Map.ContainsKey ( key );
        }

        public virtual D Get ( T key )
        {
            return Map [ key ];
        }

        public virtual void Remove ( T key )
        {
            Map.Remove ( key );
        }
    }
}