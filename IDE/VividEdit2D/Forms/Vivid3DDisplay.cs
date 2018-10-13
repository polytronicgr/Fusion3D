using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Platform.Windows;
using OpenTK.Platform;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace VividEdit.Forms
{
    public partial class Vivid3DDisplay : GLControl
    {
        public Vivid3DDisplay()
        {
            InitializeComponent();
        }

        private void Vivid3DDisplay_Paint(object sender, PaintEventArgs e)
        {
            GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SwapBuffers();
        }
    }
}
