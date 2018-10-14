using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Vivid3D.App;
namespace VividControls
{
    public partial class VividGL : OpenTK.GLControl
    {
        public OnLoad EvLoad = null;
        public OnPaint EvPaint = null;
        public OnResize EvResize = null;
        public VividGL(OnLoad load) : base(new OpenTK.Graphics.GraphicsMode(32,24,0,8))
        {
            EvLoad = load;
            InitializeComponent();
        }

        private void VividGL_Paint(object sender, PaintEventArgs e)
        {
            EvPaint?.Invoke();
        }

        private void VividGL_Load(object sender, EventArgs e)
        {
            EvLoad?.Invoke();
          
        }

        private void VividGL_Resize(object sender, EventArgs e)
        {
            EvResize?.Invoke();
        }
    }
    public delegate void OnLoad();
    public delegate void OnPaint();
    public delegate void OnResize();
}
