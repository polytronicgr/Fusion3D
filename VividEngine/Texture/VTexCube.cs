using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
namespace Vivid3D.Texture
{
    public class VTexCube : VTexBase
    {
        public VTexCube(int w, int h, byte[] f0, byte[] f1, byte[] f2, byte[] f3, byte[] f4, byte[] f5)
        {
            W = w;
            H = h;
            D = 1;
            GL.ActiveTexture(TextureUnit.Texture0);
            GenMap();
            List<byte[]> ll = new List<byte[]>();
            ll.Add(f1);
            ll.Add(f2);
            ll.Add(f3);
            ll.Add(f4);
            ll.Add(f5);
            ll.Add(f5);

            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeZ, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f2);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveZ, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f4);
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeX, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f1);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f3);
            GL.TexImage2D(TextureTarget.TextureCubeMapNegativeY, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f5);
            GL.TexImage2D(TextureTarget.TextureCubeMapPositiveY, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, f0);




            //for (int i = 0; i < 6; i++)
            // {
            //   GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, ll[i]);
            //}
        }
        public VTexCube(int w,int h)
        {
            W = w;
            H = h;
            D = 1;
            GL.ActiveTexture(TextureUnit.Texture0);
            GenMap();
            for(int i=0;i<6;i++)
            {
                //TextureTarget.
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            }


        }
      
        public byte[] Dat(string pf)
        {
            var TexData = new Bitmap(pf);
            W = TexData.Width;
            H = TexData.Height;
            var pixs = new byte[W * H * 4];
            int loc = 0;
            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
                    var p = TexData.GetPixel(x, y);
                    pixs[loc++] = p.R;
                    pixs[loc++] = p.G;
                    pixs[loc++] = p.B;
                    pixs[loc++] = p.A;

                }
            }
            return pixs;
        }
        private void GenMap()
        {
            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, ID);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
        }
        public override void Bind(int texu)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texu);
            GL.Enable(EnableCap.TextureCubeMap);
            GL.BindTexture(TextureTarget.TextureCubeMap, ID);
        }
        public override void Release(int texu)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texu);
            GL.BindTexture(TextureTarget.TextureCubeMap, 0);

            GL.Disable(EnableCap.TextureCubeMap);
        }
    }
}
