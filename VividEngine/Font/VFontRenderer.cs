using OpenTK;
using Vivid3D.Draw;

namespace Vivid3D.Font
{
    public class VFontRenderer
    {
        public static void Draw ( VFont font , string text , float x , float y , Vector4 col )
        {
            Draw ( font , text , ( int ) x , ( int ) y , col );
        }

        public static void Draw ( VFont font , string text , float x , float y )
        {
            Draw ( font , text , ( int ) x , ( int ) y );
        }

        public static void Draw ( VFont font , string text , int x , int y , Vector4 col )
        {
            int dx = x;
            VPen.BlendMod = VBlend.Alpha;
            foreach ( char c in text )
            {
                VGlyph cg = font.Glypth[c];
                VPen.Rect ( dx , y , cg.W , cg.H , cg.Img , col );
                dx += ( int ) ( cg.W / 1.3f );
            }
        }

        public static void Draw ( VFont font , string text , int x , int y )
        {
            Draw ( font , text , x , y , new Vector4 ( 1 , 1 , 1 , 1 ) );
        }
    }
}