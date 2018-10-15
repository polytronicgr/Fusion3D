using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.Data
{
    public class VInfoMap<T,D>
    {
        public Dictionary<T, D> Map = new Dictionary<T, D>();
        public virtual void Add(T key,D data)
        {
            Map.Add(key, data);
        }
        public virtual bool Has(T key)
        {
            return Map.ContainsKey(key);
        }
        public virtual D Get(T key)
        {

            return Map[key];
        }
        public virtual void Remove(T key)
        {
            Map.Remove(key);
        }
        
    }
}
