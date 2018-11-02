using System.Drawing;
using System.Windows.Forms;

namespace VividEdit.Forms.Inspectors.ValueTypes
{
    public partial class InspectTexture : UserControl
    {
        public Vivid3D.Texture.VTex2D Value = null;

        public InspectTexture ( )
        {
            InitializeComponent ( );
        }

        public void AlignToValue ( )
        {
            Bitmap bm = new Bitmap(Value.W, Value.H);
            byte[] bs = Value.RawData;
            Bitmap nbm = new Bitmap(texView.Width, texView.Height);

            float xr = Value.W / (float)texView.Width;
            float yr = Value.H / (float)texView.Height;

            int bsiz = 3;
            if ( Value.Alpha )
            {
                bsiz = 4;
            }

            for ( int y = 0; y < texView.Height; y++ )
            {
                for ( int x = 0; x < texView.Width; x++ )
                {
                    int nx = (int)(x * xr);
                    int ny = (int)(y * yr);

                    int loc = (nx * bsiz) + (ny * Value.W * bsiz);

                    Color p = Color.FromArgb(255, bs[loc], bs[loc + 1], bs[loc + 2]);
                    nbm.SetPixel ( x, y, p );
                }
            }

            texView.Image = nbm;
            texView.Invalidate ( );
        }
    }
}