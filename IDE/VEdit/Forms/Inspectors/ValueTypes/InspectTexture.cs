using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VividEdit.Forms.Inspectors.ValueTypes
{
    public partial class InspectTexture : UserControl
    {
        public Vivid3D.Texture.VTex2D Value = null;
        public InspectTexture()
        {
            InitializeComponent();
        }
        public void AlignToValue()
        {
            var bm = new Bitmap(Value.W, Value.H);
            byte[] bs = Value.RawData;
            int i = 0;
            var nbm = new Bitmap(texView.Width, texView.Height);

            float xr = (float)Value.W / (float)texView.Width;
            float yr = (float)Value.H / (float)texView.Height;
            
            int bsiz = 3;
            if (Value.Alpha) bsiz = 4;

            for(int y = 0; y < texView.Height; y++)
            {
                for(int x = 0; x < texView.Width; x++)
                {
                    int nx = (int)((float)x * xr);
                    int ny = (int)((float)y * yr);

                    int loc = (int)((nx * bsiz) + (ny * Value.W * bsiz));

                    Color p = Color.FromArgb(255, bs[loc], bs[loc + 1], bs[loc + 2]);
                    nbm.SetPixel(x, y, p);


                }
            }


            texView.Image = nbm;
            texView.Invalidate();

        }
    }
}
