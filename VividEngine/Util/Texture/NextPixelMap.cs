namespace Vivid3D.Util.Texture
{
    public class NextPixelMap
    {
        public PixelMap Detail;
        public PixelMap NextPixel;

        public NextPixelMap ( int w, int h )
        {
            Detail = new PixelMap ( w, h );
            NextPixel = new PixelMap ( w, h );
        }
    }

    public class PixelMap
    {
        public byte[] Dat = null;
        public bool[] Used = null;
        public int prevX=0, prevY =0;

        public PixelMap ( int w, int h )
        {
            Used = new bool [ w * h ];
            Dat = new byte [ w * h * 3 ];
            for ( int y = 0; y < h; y++ )
            {
                for ( int x = 0; x < w; x++ )
                {
                    Used [ y * w + x ] = false;
                    int loc = y * w * 3 + x *3;
                    Dat [ loc ] = 0;
                    Dat [ loc + 1 ] = 0;
                    Dat [ loc + 2 ] = 0;
                }
            }
        }
    }
}