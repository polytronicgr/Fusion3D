using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Vivid3D.Texture
{
    public class VTexDepth : VTexBase
    {

        public VTexDepth(int w, int h)
        {
           
            W = w;
            H = h;
            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, ID);
        
           GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, w, h, 0, PixelFormat.DepthComponent,PixelType.UnsignedByte, IntPtr.Zero);
         
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.PixelStore(PixelStoreParameter.PackAlignment, 4);
        }

        public override void Bind(int texu)
        {
            

            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
  
            GL.BindTexture(TextureTarget.Texture2D, ID);
        }
        public override void Release(int texu)
        {
            GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
          
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
        }
    }
}
