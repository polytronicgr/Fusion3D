using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Vivid3D.Texture
{
    public enum LoadMethod
    {
        Single,Multi
    }
    public class VTex2D : VTexBase
    {
        public Thread LoadThread = null;
        public Mutex LoadMutex = new Mutex();
        public Bitmap TexData = null;
        public bool Loaded = false;
        public bool Binded = false;
        public byte[] pixs = null;
        public bool Alpha = false;

        public void ReadBitmap(BinaryReader r)
        {
            short bw = r.ReadInt16();
            short bh = r.ReadInt16();
            TexData = new Bitmap(bw, bh);
            pixs = r.ReadBytes(bw * bh * 4);
            W = bw;
            H = bh;
            Alpha = true;
            return;
            for(int y = 0; y < bh; y++)
            {
                for(int x = 0; x < bw; x++)
                {
                    byte[] col = r.ReadBytes(4);
                    System.Drawing.Color nc = System.Drawing.Color.FromArgb(col[3], col[0], col[1], col[2]);
                    TexData.SetPixel(x, y, nc);
                }
            }
            W = bw;
            H = bh;
        }
        public void SkipBitmap(BinaryReader r)
        {
            short bw = r.ReadInt16();
            short bh = r.ReadInt16();
            r.BaseStream.Seek(bw * bh * 4, SeekOrigin.Current);
        }
        public void SaveTex(string p)
        {
            if (string.IsNullOrEmpty(p))
            {
                return;
            }
            if (File.Exists(p)) return;
            FileStream fs = new FileStream(p, FileMode.Create, FileAccess.Write);
            BinaryWriter w = new BinaryWriter(fs);
          
            
                var sd = new Bitmap(TexData, 32, 32);
                WriteBitmap(sd,w);
                WriteBitmap(TexData,w);
          
            fs.Flush();
            fs.Close();
        }
        public void WriteBitmap(Bitmap b,BinaryWriter w)
        {
            w.Write((short)b.Width);
            w.Write((short)b.Height);
            for(int y = 0; y < b.Height; y++)
            {
                for(int x = 0; x < b.Width; x++)
                {
                    var p = b.GetPixel(x, y);
                    w.Write(p.R);
                    w.Write(p.G);
                    w.Write(p.B);
                    w.Write(p.A);
                }
            }
        }
        public void CopyTex(int x,int y)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, this.ID);
            GL.CopyTexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, x,y, W, H);
            GL.BindTexture(TextureTarget.Texture2D, 0);

        }

        ~VTex2D()
        {
            //NewMethod();
        }

        public void Delete()
        {
            GL.DeleteTexture(ID);
        }

        public VTex2D(int w,int h,bool alpha=false)
        {
            GenTex(w, h, alpha);

        }

        private void GenTex(int w, int h, bool alpha)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            Alpha = alpha;
            W = w;
            H = h;
            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, ID);
            if (alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, w, h, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
            }
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.PixelStore(PixelStoreParameter.PackAlignment, 4 * 4);
        }

        public VTex2D(int w,int h,byte[] dat,bool alpha = true)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            Alpha = alpha;
            W = w;
            H = h;
            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, ID);
            if (alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, w, h, 0, PixelFormat.Rgba, PixelType.UnsignedByte, dat);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, dat);
            }
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.PixelStore(PixelStoreParameter.PackAlignment, 4 * 4);
            pixs = dat;

        }
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
      
        public static Dictionary<string, VTex2D> Lut = new Dictionary<string, VTex2D>();

        public VTex2D(string path,LoadMethod lm,bool alpha = true)
        {

            if (File.Exists(path) == false)
            {
                return;
            }
            if (path == string.Empty || path == "" || path == null)
            {
                return;
            }
            Path = path;

            if (Lut.ContainsKey(path))
            {

                var ot = Lut[path];
                ID = ot.ID;
                W = ot.W;
                H = ot.H;
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
                W = r.ReadInt16();
                H = r.ReadInt16();
                Alpha = r.ReadBoolean();
                int pc = 3;
                if (Alpha) pc = 4;

                RawData = new byte[W * H * pc];

                r.Read(RawData, 0, W * H * pc);

                fs.Close();
                fs = null;

            }
            else
            {

                Bitmap img = new Bitmap(path);
                //System.Drawing.Imaging.BitmapData dat = img.LockBits( new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.);

                W = img.Width;
                H = img.Height;
                Alpha = alpha;

                int pc = 3;
                if (Alpha) pc = 4;

                RawData = new byte[W * H * pc];

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
                if (Name == null || Name == string.Empty)
                {
                    Name = "Tex2D";
                }
                w.Write(Name);
                w.Write(Path);
                w.Write((short)W);
                w.Write((short)H);
                w.Write(Alpha);
                pc = 3;
                if (alpha) pc = 4;
                w.Write(RawData, 0, W * H * pc);
                fs.Flush();
                fs.Close();
                fs = null;
            }

            if (alpha)
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, W, H, 0, PixelFormat.Rgba, PixelType.UnsignedByte, RawData);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, W, H, 0, PixelFormat.Rgb, PixelType.UnsignedByte, RawData);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        bool PreLoaded = false;
        FileStream nf;
        BinaryReader nr;
        public void T_LoadVTex()
        {
            ReadBitmap(nr);

            //W = TexData.Width;
            //H = TexData.Height;
            D = 1;
            Alpha = true;
            //SetPix();
            //SaveTex(Path + ".vtex");
            Loaded = true;
            nf.Close();
            nf = null;
            nr = null;

        }
        public override void Bind(int texu)
        {
            if(Loaded==true && Binded==false)
            {
                BindData();
                Binded = true;
            }

            GL.Enable(EnableCap.Texture2D);
            GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
          //  GL.ClientActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
            GL.BindTexture(TextureTarget.Texture2D, ID);
        }
        public override void Release(int texu)
        {
            GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
           // GL.ClientActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + texu));
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);
        }
        public void SetPix()
        {
            pixs = new byte[W * H * 4];
            int loc = 0;
            for (int y=0;y<H;y++)
            {
                for(int x=0;x<W;x++)
                {
                    var p = TexData.GetPixel(x, y);
                    pixs[loc++] = p.R;
                    pixs[loc++] = p.G;
                    pixs[loc++] = p.B;
                    pixs[loc++] = p.A;
                  
                }
            }
        }
        public void BindData()
        {
            GL.Enable(EnableCap.Texture2D);
            ID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, ID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.PixelStore(PixelStoreParameter.PackAlignment, 4 * 4);
           GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, W, H, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixs);
            GL.Disable(EnableCap.Texture2D);

        }
        public void T_LoadTex()
        {
       
            TexData = new Bitmap(Path); 

            W = TexData.Width;
            H = TexData.Height;
            D = 1;
            Alpha = true;
            SetPix();
            SaveTex(Path + ".vtex");
            Loaded = true;
        }
    }
}
