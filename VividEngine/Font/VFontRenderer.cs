using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Draw;
using Vivid3D.Texture;
using OpenTK;
namespace Vivid3D.Font
{
    public class VFontRenderer
    {
        public static void Draw(VFont font,string text,float x,float y,Vector4 col)
        {
            Draw(font, text, (int)x, (int)y, col);
        }
        public static void Draw(VFont font,string text,float x,float y)
        {
            Draw(font, text, (int)x, (int)y);
        }
        public static void Draw(VFont font,string text,int x,int y,Vector4 col)
        {
            int dx = x;
            VPen.BlendMod = VBlend.Alpha;
            foreach(Char c in text)
            {
                VGlyph cg = font.Glypth[(int)c];
                VPen.Rect(dx, y, cg.W, cg.H, cg.Img,col);
                dx += (int)((float)cg.W / 1.3f);
            }
        }
        public static void Draw(VFont font,string text,int x,int y)
        {
            Draw(font, text, x, y, new Vector4(1, 1, 1, 1));
        }
    }
}
