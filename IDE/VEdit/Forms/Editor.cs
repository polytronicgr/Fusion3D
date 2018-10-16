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
        public NodePicked Picked = null;
        public Vivid3D.Texture.VTex2D XIcon, YIcon, ZIcon;
        bool iconLoaded = false;
        public Editor()
        {
            InitializeComponent();
            Dis = new VividGL(ON_Load);
            // Dis.Dock = DockStyle.Fill;
            Dis.Size = new Size(Width, Height);
            Dis.EvPaint = ON_Paint;
            Dis.EvResize = ON_Resize;
            Dis.EvMouseMoved = ON_MouseMove;
            Dis.EvMouseDown = ON_MouseDown;
            Dis.EvMouseUp = ON_MouseUp;
            Dis.EvKeyDown = ON_KeyDown;
            Dis.EvKeyUp = ON_KeyUp;
            this.Controls.Add(Dis);
          
//            dosize = true;

        }

        public void LoadIcons()
        {
            if (iconLoaded) return;
            XIcon = new Vivid3D.Texture.VTex2D("data\\icon\\iconx.png", Vivid3D.Texture.LoadMethod.Single, true);
            YIcon = new Vivid3D.Texture.VTex2D("data\\icon\\icony.png", Vivid3D.Texture.LoadMethod.Single, true);
            ZIcon = new Vivid3D.Texture.VTex2D("data\\icon\\iconz.png", Vivid3D.Texture.LoadMethod.Single, true);
            iconLoaded = true;
        }

        public SceneGraph3D GetGraph() { return Graph; }
        Vector3 moveV = Vector3.Zero;
        bool moving = false;
        float moveS = 2.0f;
        bool fast = false;
        float mX=0, mY=0, mZ=0;
        void ON_KeyDown(Keys k)
        {

           
         
                
            

            switch (k)
            {
                case Keys.S:
                    moving = true;
                    mZ = 1.0f;
                    break;
                case Keys.A:
                    moving = true;
                    mX = -1;
                    break;
                case Keys.D:
                    moving = true;
                    mX = 1;
                    break;
                case Keys.W:
                    moving = true;
                    mZ = -1;
                    break;
                case Keys.Shift:
                case Keys.LShiftKey:
                case Keys.ShiftKey:
                    moveS = 5.0f;
                    break;
            }

        }

        void ON_KeyUp(Keys k)
        {


            switch (k)
            {

                case Keys.A:
                    mX = 0;
                    break;
                case Keys.S:
                    mZ = 0;
                    break;
                case Keys.D:
                    mX = 0;
                    break;
                case Keys.W:
                    moving = false;
                    mZ = 0; 
                break;
                case Keys.Shift:
                case Keys.LShiftKey:
                case Keys.ShiftKey:
                    moveS = 2.0f;
                    break;
            }
        }

        bool rotate = false;
        bool InRect(int rx,int ry,int rw,int rh)
        {
            int x = msX;
            int y = msY;
            return (x >= rx && y >= ry && x <= (rx + rw) && y <= (ry + rh));
        }
        bool xLock=false, yLock=false, zLock=false;
        void ON_MouseDown(MouseButtons b)
        {
            if(b==MouseButtons.Right)
            {
                rotate = true;
            }
            if (b == MouseButtons.Left)
            {

                xLock = false;
                yLock = false;
                zLock = false;
                if (InRect(tX - 6, tY - 80, 32, 32))
                {
                    yLock = true;
                    xLock = false;
                    zLock = false;
                    return;
                }
                if (InRect(tX + 80, tY - 3, 32, 32))
                {
                    xLock = true;
                    yLock = false;
                    zLock = false;
                    return;
                }
                if (InRect(tX + 32, tY - 32, 32, 32))
                {
                    zLock = true;
                    yLock = false;
                    xLock = false;
                    return;
                }



                //Graph.CamPick(mX, mY);
                var res = Graph.CamPick(msX, msY);
                if (res != null)
                {
                    Console.WriteLine("Picked:" + res.Node.Name);
                    Picked?.Invoke(res.Node);
                    CurNode = res.Node;
                }
                else
                {
                    CurNode = null;
                }

            }
            // var l = Graph.GetList(true);
               
        }
        GraphNode3D CurNode = null;
        void ON_MouseUp(MouseButtons b)
        {
            if (b == MouseButtons.Right)
            {
                rotate = false;
            }
            xLock = yLock = zLock = false;
        }
        public int msX, msY;
        void ON_MouseMove(int x, int y, int dx, int dy)
        {
            float rx = ((float)dx) * 0.2f;
            float ry = ((float)dy) * 0.2f;
            if (rotate)
            {
                Cam.Turn(new Vector3(-ry, -rx, 0), Space.Local);

            }
            msX = x;
            msY = y;
            Vector3 mov = Vector3.Zero;
            if (xLock)
            {
                mov = new Vector3(dx, 0, 0);
            }
            if (yLock)
            {
                mov = new Vector3(0, dy, 0);
            }
            if (zLock)
            {
                mov = new Vector3(0, 0, dx);
            }
            if (CurNode != null)
            {
                if (xLock||yLock||zLock)
                {
                    
                
                    CurNode.Move(mov, Space.Local);
               
                }
            }
        }
        public int tX=20, tY=20;
        public bool dosize = false;
        void ON_Load()
        {
            Dis.Size = new Size(Width, Height);
            VForm.Set(Width, Height);
            Grid = GeoGen.Quad(100, 100);
            Grid.Rot(new Vector3(-90, 0, 0), Space.Local);


            Grid.AlwaysAlpha = true;
            var mat = new Vivid3D.Material.Material3D();
            Grid.Renderer = new VRNoFx();
            Vivid3D.Texture.VTex2D.Lut.Clear();
            mat.TCol = new Vivid3D.Texture.VTex2D("data\\ui\\grid.png", Vivid3D.Texture.LoadMethod.Single, true);
            Grid.Meshes[0].Mat = mat;
            Light = new GraphLight3D();
            Light.Pos(new Vector3(20, 120, 250), Space.Local);
            Light.Range = 800;
            Light.On = true;
            Light.Diff = new Vector3(2, 2, 2);
            Light.CastShadows = true;
            
            Graph = new SceneGraph3D();
            Graph.Add(Grid);
            Graph.Add(Light);
            Cam = new GraphCam3D();
            Graph.Add(Cam);
            //Cam.Rot(new Vector3(45, 0, 0), Space.Local);
            //    Grid.Rot(new Vector3(-30, 0, 0), Space.Local);
            Cam.Pos(new Vector3(0, 100, 350), Space.Local);
          //  Cam.Rot(new Vector3(-10, 0, 0), Space.Local);
            //Cam.LookAt(new Vector3(0, 0, 0), Vector3.UnitY);
        }
        void ON_Paint()
        {
          

            GL.ClearColor(0.7f, 0.7f, 0.7f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // if (run)
            //{
            //    Graph?.Render();
            // }
            Graph?.RenderShadows();
            Graph?.Render();

            if (CurNode != null)
            {

                var res = Vivid3D.Pick.Picker.CamTo2D(Cam, CurNode.WorldPos);

                tX = (int)res.X;
                tY = (int)res.Y;
                LoadIcons();

                Vivid3D.Draw.VPen.BlendMod = Vivid3D.Draw.VBlend.Alpha;

                Vivid3D.Draw.VPen.Rect(tX-3, tY-3, 6, 6, new Vector4(1, 1, 1, 1));

                Vivid3D.Draw.VPen.Rect(tX - 6, tY - 80, 32, 32, YIcon);
                Vivid3D.Draw.VPen.Rect(tX + 80, tY - 3, 32, 32, XIcon);
                Vivid3D.Draw.VPen.Rect(tX + 32, tY - 32, 32, 32, ZIcon);

            }
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

        private void EditTimer_Tick(object sender, EventArgs e)
        {
            if (moving)
            {
                Cam.Move(new Vector3(mX * moveS,mY * moveS,mZ * moveS), Space.Local);
               // Light.LocalPos = Cam.LocalPos;
                
            }

            Dis.Invalidate();
        }
    }
    public delegate void NodePicked(Vivid3D.Scene.GraphNode3D node);
}
