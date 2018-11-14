using OpenTK;
using Fusion3D.Draw;

namespace Fusion3D.Font
{
    public class FontRenderer
    {
        public static void Draw ( Font2D font, string text, float x, float y, Vector4 col )
        {
            Draw ( font, text, ( int ) x, ( int ) y, col );
        }

        public static void Draw ( Font2D font, string text, float x, float y )
        {
            Draw ( font, text, ( int ) x, ( int ) y );
        }

        public static void Draw ( Font2D font, string text, int x, int y, Vector4 col )
        {
            int dx = x;
            Pen2D.BlendMod = PenBlend.Alpha;
            foreach ( char c in text )
            {
                VGlyph cg = font.Glypth[c];
                Pen2D.Rect ( dx, y, cg.W, cg.H, cg.Img, col );
                dx += ( int ) ( cg.W / 1.3f );
            }
        }

        public static void Draw ( Font2D font, string text, int x, int y )
        {
            Draw ( font, text, x, y, new Vector4 ( 1, 1, 1, 1 ) );
        }
    }
}