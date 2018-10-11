using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.Tex
{
    public class TexBase
    {
        public int ID
        {
            get;
            set;
        }
        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }
        public bool Alpha
        {
            get;
            set;
        }
        
        public virtual void Bind(int texunit)
        {

        }
        public virtual void Unbind(int texunit)
        {

        }

    }
}
