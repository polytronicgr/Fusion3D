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
using Vivid3D.App;
using Vivid3D.Scene;
using Vivid3D.Gen;
using Vivid3D.UI;
using Vivid3D.Lighting;
using Vivid3D.Visuals;
namespace VividEdit.Forms
{
    public partial class Vivid3DDisplay : GLControl
    {

        public SceneGraph3D Graph = null;
        public GraphEntity3D Grid;
        public GraphLight3D Light;
        public GraphCam3D Cam;
        public Vivid3DDisplay()
        {
            InitializeComponent();

            

        }



        private void Vivid3DDisplay_Paint(object sender, PaintEventArgs e)
        {
            GL.ClearColor(0.7f, 0.7f, 0.7f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Graph?.Render();

            SwapBuffers();
        }

        bool loaded = false;
        private void Vivid3DDisplay_Load(object sender, EventArgs e)
        {


            Vivid3D.Texture.VTex2D.Lut.Clear();
            VForm.Set(Width, Height);
            Grid = GeoGen.Quad(100, 100);
            Grid.AlwaysAlpha = true;
            var mat = new Vivid3D.Material.Material3D();
            Grid.Renderer = new VRNoFx();     
            mat.TCol = new Vivid3D.Texture.VTex2D("data\\ui\\grid.png", Vivid3D.Texture.LoadMethod.Single, true);
            Grid.Meshes[0].Mat = mat;
            Light = new GraphLight3D();
            Graph = new SceneGraph3D();
            Graph.Add(Grid);
            Graph.Add(Light);
            Cam = new GraphCam3D();
            Graph.Add(Cam);
            //Cam.Rot(new Vector3(45, 0, 0), Space.Local);
        //    Grid.Rot(new Vector3(-30, 0, 0), Space.Local);
            Cam.Pos(new Vector3(0, 0, 350), Space.Local);
        }



        private void Vivid3DDisplay_Resize(object sender, EventArgs e)
        {
            VForm.SetSize(Width, Height);

        }
    }
 
}
