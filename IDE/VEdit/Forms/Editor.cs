using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo;
using VividControls;
using WeifenLuo.WinFormsUI;
using WeifenLuo.WinFormsUI.Docking;
using Vivid3D.App;
using Vivid3D.Scene;
using Vivid3D.Gen;
using Vivid3D.UI;
using Vivid3D.Lighting;
using Vivid3D.Visuals;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace VividEdit.Forms
{
    public partial class Editor : DockContent
    {
        public VividGL Dis = null;


        public SceneGraph3D Graph = null;
        public GraphEntity3D Grid;
        public GraphLight3D Light;
        public GraphCam3D Cam;

        public Editor()
        {
            InitializeComponent();
            Dis = new VividGL(ON_Load);
            // Dis.Dock = DockStyle.Fill;
            Dis.Size = new Size(Width, Height);
            Dis.EvPaint = ON_Paint;
            Dis.EvResize = ON_Resize;
            this.Controls.Add(Dis);
//            dosize = true;

        }
        public bool dosize = false;
        void ON_Load()
        {
            Dis.Size = new Size(Width, Height);
            VForm.Set(Width, Height);
            Grid = GeoGen.Quad(100, 100);
            //Grid.Rot(new Vector3(-90, 0, 0), Space.Local);


            Grid.AlwaysAlpha = true;
            var mat = new Vivid3D.Material.Material3D();
            Grid.Renderer = new VRNoFx();
            Vivid3D.Texture.VTex2D.Lut.Clear();
            mat.TCol = new Vivid3D.Texture.VTex2D("data\\ui\\grid2.jpg", Vivid3D.Texture.LoadMethod.Single, true);
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
          //  Cam.Rot(new Vector3(-10, 0, 0), Space.Local);
            //Cam.LookAt(new Vector3(0, 0, 0), Vector3.UnitY);
        }
        void ON_Paint()
        {
          

            GL.ClearColor(0.7f, 0.7f, 0.1f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // if (run)
            //{
            //    Graph?.Render();
            // }

            Graph?.Render();
            Dis.SwapBuffers();
        }
        void ON_Resize()
        {
            Dis.Size = new Size(Width, Height);
            VForm.SetSize(Width, Height);
            Dis.Invalidate();

        }
        public void InitSize()
        {
            ON_Resize();
        }
        private void Editor_Resize(object sender, EventArgs e)
        {
            if(dosize)
            {
                ON_Resize();
            }
            if (Dis != null)
            {
                //ON_Resize();
            }
        }
    }
}
