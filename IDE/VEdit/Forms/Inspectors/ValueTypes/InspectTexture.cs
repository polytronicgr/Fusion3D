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
            for (int y = 0; y < Value.H; y++)
            {
                for(int x = 0; x < Value.W; x++)
                {

                    Color p = Color.FromArgb(bs[i], bs[i + 1], bs[i + 2]);
                    bm.SetPixel(x, y, p);
                    i += 3;
                }
            }
            bm = new Bitmap(bm, new Size(texView.Width, texView.Height));
            texView.Image = bm;
            texView.Invalidate();

        }
    }
}
