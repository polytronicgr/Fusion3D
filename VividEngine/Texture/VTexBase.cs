using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.Texture
{
    public enum Format
    {
        RGB,RGBA,A,Depth16,Depth32,Stencil16,Stencil32,FrameBuffer16,FrameBuffer32
    }
    public class VTexBase
    {
        public int ID = 0;
        public string Path = "";
        public int W, H, D;
        public Format Form = Format.RGBA;

        
        public virtual void Bind(int texu)
        {

        }
        public virtual void Release(int texu)
        {

        }
    }
}
