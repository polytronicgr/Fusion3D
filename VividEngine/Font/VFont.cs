using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using System.IO;
namespace Vivid3D.Font
{
    public class VFont
    {
        public string Path = "";
        public List<VGlyph> Glypth = new List<VGlyph>();

        public int Width(string t)
        {
            int sw = 0;
            foreach(char c in t)
            {
                VGlyph v = Glypth[(int)c];
                sw += (int)((float)v.W / 1.3f);
            }
            return sw;
        }
        public int Height()
        {
            return Glypth[0].H;
        }
        public VFont(string path)
        {
            Path = path;
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            for(int c=0;c<255;c++)
            {
                VGlyph g = new VGlyph
                {
                    W = r.ReadInt16(),
                    H = r.ReadInt16()
                };

                byte[] img = new byte[g.W * g.H * 4];

                for(int y=0;y<g.H;y++)
                {
                    for(int x=0;x<g.W;x++)
                    {
                        int loc = (y * g.W * 4) + (x * 4);
                        img[loc++] = r.ReadByte();
                        img[loc++] = r.ReadByte();
                        img[loc++] = r.ReadByte();
                        img[loc] = r.ReadByte();
                    }
                }
                g.Img = new VTex2D(g.W, g.H, img, true);
                Glypth.Add(g);
            }

            fs.Close();
            fs = null;
        }

    }
    public class VGlyph
    {
        public VTex2D Img = null;
        public int W=0, H=0;
    }
}
