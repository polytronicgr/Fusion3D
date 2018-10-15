using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using System.IO;
using Vivid3D.Archive;
namespace Vivid3D.Tex
{
    public class Tex2D : TexBase
    {

        public string Name
        {
            get;
            set;
        }
        public string Path
        {
            get;
            set;
        }
        public byte[] RawData;
        public static byte[] TmpStore = null;
        public override string ToString()
        {
            return "Img:" + Name;    
        }
        public Tex2D(Bitmap bit,bool alpha)
        {
            if (TmpStore == null)
            {

                TmpStore = new byte[4096 * 4096 * 4];

            }

            GL.Enable(EnableCap.Texture2D);
            ID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, ID);

            Bitmap img = bit;
            
            //System.Drawing.Imaging.BitmapData dat = img.LockBits( new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.);

            Width = img.Width;
            Height = img.Height;
            Alpha = alpha;

            int pc = 3;
            if (Alpha) pc = 4;
            RawData = new byte[Width * Height * pc];

            //GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.)

            

            int pi = 0;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    var pix = img.GetPixel(x, y);
                    RawData[pi++] = pix.R;
                    RawData[pi++] = pix.G;
                    RawData[pi++] = pix.B;
                    if (alpha)
                    {
                        RawData[pi++] = pix.A;
                    }

                }
            }


            if (alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, RawData);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Width, Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, RawData);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.BindTexture(TextureTarget.Texture2D, 0);

        }
        public static Dictionary<VirtualEntry, Tex2D> VLut = new Dictionary<VirtualEntry, Tex2D>();
        public static Dictionary<string, Tex2D> Lut = new Dictionary<string, Tex2D>();
        public Tex2D(VirtualEntry entry,bool alpha)
        {
            Path = entry.Name;
           
          
            if (VLut.ContainsKey(entry)) 
            {

                var ot = VLut[entry];
                    ID = ot.ID;
                Width = ot.Width;
                Height = ot.Height;
                Alpha = ot.Alpha;
                Name = ot.Name;
                Path = ot.Path;
                RawData = ot.RawData;
               /// Console.WriteLine("Lut:" + Name);
                return;
            }
            else
            {
 
                VLut.Add(entry, this);
               

            }



            Width = entry.Par[0];
            Height = entry.Par[1];
            Alpha = entry.Par[2] == 1 ? true : false;

            int pc = 3;
            if (Alpha) pc = 4;

            RawData = entry.RawData;

            GL.Enable(EnableCap.Texture2D);
            ID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, ID);


            //GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.)

    


         

            if (alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, RawData);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Width, Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, RawData);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        public Tex2D(string path, bool alpha, byte[] data)
        {

            GL.Enable(EnableCap.Texture2D);
            ID = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, ID);


            if (alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, TmpStore);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Width, Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, TmpStore);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.BindTexture(TextureTarget.Texture2D, 0);

        }
        public Tex2D(string path,bool alpha=false)
        {

            if(path == string.Empty || path == "" || path == null)
            {
                return;
            }
            Path = path;

            if (Lut.ContainsKey(path))
            {

                var ot = Lut[path];
                ID = ot.ID;
                Width = ot.Width;
                Height = ot.Height;
                Path = ot.Path;
                Alpha = ot.Alpha;
                Name = ot.Name;
                return;
            }
            else
            {
                Lut.Add(path, this);
            }
            
           

            Console.WriteLine("Reading:" + path);

           
            
            GL.Enable(EnableCap.Texture2D);
            ID = GL.GenTexture();
            
            GL.BindTexture(TextureTarget.Texture2D, ID);

            if (new FileInfo(path + ".cache").Exists)
            {
                FileStream fs = new FileStream(path + ".cache", FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);
                Name = r.ReadString();
                Path = r.ReadString();
                Width = r.ReadInt16();
                Height = r.ReadInt16();
                Alpha = r.ReadBoolean();
                int pc = 3;
                if (Alpha) pc = 4;

                RawData = new byte[Width * Height * pc];

                r.Read(RawData, 0,Width * Height * pc);

                fs.Close();
                fs = null;

            }
            else
            {

                Bitmap img = new Bitmap(path);
                //System.Drawing.Imaging.BitmapData dat = img.LockBits( new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.);

                Width = img.Width;
                Height = img.Height;
                Alpha = alpha;

                int pc = 3;
                if (Alpha) pc = 4;

                RawData = new byte[Width * Height * pc];

                //GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.)



                int pi = 0;
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        var pix = img.GetPixel(x, y);
                        RawData[pi++] = pix.R;
                        RawData[pi++] = pix.G;
                        RawData[pi++] = pix.B;
                        if (alpha)
                        {
                            RawData[pi++] = pix.A;
                        }

                    }
                }

                FileStream fs = new FileStream(path + ".cache", FileMode.Create, FileAccess.Write);
                BinaryWriter w = new BinaryWriter(fs);
                if(Name == null || Name == string.Empty)
                {
                    Name = "Tex2D";
                }
                w.Write(Name);
                w.Write(Path);
                w.Write((short)Width);
                w.Write((short)Height);
                w.Write(Alpha);
                pc = 3;
                if (alpha) pc = 4;
                w.Write(RawData, 0, Width * Height * pc);
                fs.Flush();
                fs.Close();
                fs = null;
            }

            if (alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, RawData);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Width, Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte,RawData);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,(int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,(int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.BindTexture(TextureTarget.Texture2D, 0);

        }

        public override void Bind(int texunit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texunit);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, ID);
        }
        public override void Unbind(int texunit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + texunit);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);

        }
    }
}
