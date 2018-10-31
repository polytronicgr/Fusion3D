using System.Drawing;
using System.IO;

namespace DataCore.DataTypes
{
    public class DataBitmap : Data
    {
        public Bitmap Map { get; set; }

        public DataBitmap ( )
        {
        }

        public DataBitmap ( Bitmap map )
        {
            Map = map;
        }

        public override void GenerateBytes ( )
        {
            _RawData = new byte [ Map.Width * Map.Height * 4 ];
            _Bytes = _RawData.Length;

            int dl = 0;

            for ( int y = 0 ; y < Map.Height ; y++ )
            {
                for ( int x = 0 ; x < Map.Width ; x++ )
                {
                    Color col = Map.GetPixel(x, y);
                    _RawData [ dl++ ] = col.R;
                    _RawData [ dl++ ] = col.G;
                    _RawData [ dl++ ] = col.B;
                    _RawData [ dl++ ] = col.A;
                }
            }

            MemoryStream ns = new MemoryStream(Bytes + 8);
            BinaryWriter bw = new BinaryWriter(ns);
            bw.Write ( Map.Width );
            bw.Write ( Map.Height );
            bw.Write ( _RawData );
            _RawData = ns.ToArray ( );
            Bytes = _RawData.Length;
        }

        public override void Reconstruct ( )
        {
            MemoryStream ns = new MemoryStream(_RawData);
            BinaryReader br = new BinaryReader(ns);

            int mw = br.ReadInt32();
            int mh = br.ReadInt32();

            byte[] rgb = new byte[mw * mh * 4];

            ns.Read ( rgb , 0 , mw * mh * 4 );

            Map = new Bitmap ( mw , mh );

            int dl = 0;

            for ( int y = 0 ; y < mh ; y++ )
            {
                for ( int x = 0 ; x < mw ; x++ )
                {
                    int r = rgb[dl++];
                    int g = rgb[dl++];
                    int b = rgb[dl++];
                    int a = rgb[dl++];

                    Color col = Color.FromArgb(a, r, g, b);

                    Map.SetPixel ( x , y , col );
                }
            }
        }
    }
}