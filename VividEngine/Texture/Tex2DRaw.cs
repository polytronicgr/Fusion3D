namespace Vivid3D.Texture
{
    public class Tex2DRaw
    {
        public int W
        {
            get;
            set;
        }

        public int H { get; set; }
        public byte [ ] Data { get; set; }
        public bool Alpha { get; set; }

        public Tex2DRaw ( int w, int h, bool alpha )
        {
            Alpha = alpha;
            W = w;
            H = h;
            Data = new byte [ W * H * ( Alpha ? 4 : 3 ) ];
            for ( int y = 0; y < H; y++ )
            {
                for ( int x = 0; x < w; x++ )
                {
                    int loc = y * w * 3 + x *3;
                    Data [ loc ] = 0;
                    Data [ loc + 1 ] = 0;
                    Data [ loc + 2 ] = 255;
                }
            }
        }

        public void SetPixel ( int x, int y, int r, int g, int b, int a = 255 )
        {
            if ( Alpha )
            {
                int loc = y * W * 4;
                loc += x * 4;
                Data [ loc ] = ( byte ) r;
                Data [ loc + 1 ] = ( byte ) g;
                Data [ loc + 2 ] = ( byte ) b;
                Data [ loc + 3 ] = ( byte ) a;
            }
            else
            {
                int loc = y * W * 3;
                loc += x * 3;
                Data [ loc ] = ( byte ) r;
                Data [ loc + 1 ] = ( byte ) g;
                Data [ loc + 2 ] = ( byte ) b;
            }
        }
    }
}