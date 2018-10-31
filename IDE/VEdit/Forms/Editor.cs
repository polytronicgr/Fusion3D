﻿using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.Windows.Forms;
using Vivid3D.App;
using Vivid3D.Gen;
using Vivid3D.Lighting;
using Vivid3D.Scene;
using Vivid3D.Visuals;
using VividControls;
using WeifenLuo.WinFormsUI.Docking;

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
        public Vivid3D.Texture.VTex2D XIcon, YIcon, ZIcon, SIcon, LightIcon;
        private bool iconLoaded = false;
        public Vivid3D.PostProcess.PostProcessRender PRen;
        public Vivid3D.PostProcess.Processes.VPPBlur PPBlur;

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
            this.ContextMenuStrip = contextMenuStrip1;
            //            dosize = true;
        }

        public Vivid3D.Texture.VTex2D RIcon;

        public void LoadIcons()
        {
            if (iconLoaded) return;
            RIcon = new Vivid3D.Texture.VTex2D("data\\icon\\iconrot.png", Vivid3D.Texture.LoadMethod.Single, true);
            XIcon = new Vivid3D.Texture.VTex2D("data\\icon\\iconx.png", Vivid3D.Texture.LoadMethod.Single, true);
            YIcon = new Vivid3D.Texture.VTex2D("data\\icon\\icony.png", Vivid3D.Texture.LoadMethod.Single, true);
            ZIcon = new Vivid3D.Texture.VTex2D("data\\icon\\iconz.png", Vivid3D.Texture.LoadMethod.Single, true);
            SIcon = new Vivid3D.Texture.VTex2D("data\\icon\\iconscale.png", Vivid3D.Texture.LoadMethod.Single, true);
            LightIcon = new Vivid3D.Texture.VTex2D("data\\icon\\iconlight.png", Vivid3D.Texture.LoadMethod.Single, true);
            iconLoaded = true;
        }

        public SceneGraph3D GetGraph()
        {
            return Graph;
        }

        private Vector3 moveV = Vector3.Zero;
        private bool moving = false;
        private float moveS = 2.0f;
        private bool fast = false;
        private float mX = 0, mY = 0, mZ = 0;
        private bool spaceLock = false;

        private void ON_KeyDown(Keys k)
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
                    spaceLock = true;
                    fast = true;
                    break;
            }
        }

        private void ON_KeyUp(Keys k)
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
                    spaceLock = false;
                    fast = false;
                    break;
            }
        }

        private bool rotate = false;

        private bool InRect(int rx, int ry, int rw, int rh)
        {
            int x = msX;
            int y = msY;
            return (x >= rx && y >= ry && x <= (rx + rw) && y <= (ry + rh));
        }

        private bool allLock = false;
        private bool xLock = false, yLock = false, zLock = false;

        private void ON_MouseDown(MouseButtons b)
        {
            if (b == MouseButtons.Middle)
            {
                rotate = true;
            }
            if (b == MouseButtons.Left)
            {
                xLock = false;
                yLock = false;
                zLock = false;
                if (EMode == EditMode.Scale)
                {
                    if (InRect(tX - 16, tY - 16, 32, 32))
                    {
                        allLock = true;
                        xLock = yLock = zLock = false;
                        return;
                    }
                }
                if (InRect(tX - 6, tY - 80, 32, 32))
                {
                    yLock = true;
                    xLock = false;
                    zLock = false;
                    Console.WriteLine("Left");
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

                CurNode = null;
                foreach (var l in Graph.Lights)
                {
                    var sp = Vivid3D.Pick.Picker.CamTo2D(Cam, l.LocalPos);
                    if (InRect((int)sp.X - 16, (int)sp.Y - 16, 32, 32))
                    {
                        Console.WriteLine("Picked Light");
                        l.Select();
                        return;
                    }
                }

                //Graph.CamPick(mX, mY);
                var res = Graph.CamPick(msX, msY);
                if (res != null)
                {
                    Console.WriteLine("Picked:" + res.Node.Name);

                    Picked?.Invoke(res.Node);
                    CurNode = res.Node;
                    Selected.Root.Sub.Clear();
                    Selected.Root.AddProxy(CurNode);
                    VividEdit.VividED.Main.DockClassInspect.Inspect(res.Node);
                }
                else
                {
                    //    CurNode = null;
                }
            }
            // var l = Graph.GetList(true);
        }

        public GraphNode3D CurNode = null;

        private void ON_MouseUp(MouseButtons b)
        {
            if (b == MouseButtons.Middle)
            {
                rotate = false;
            }
            xLock = yLock = zLock = false;
            allLock = false;
        }

        public int msX, msY;

        private void ON_MouseMove(int x, int y, int dx, int dy)
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
            if (allLock)
            {
                mov = new Vector3(dx, dx, dx);
            }
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
                if (EMode == EditMode.Rotate)
                {
                }
                if (EMode == EditMode.Move)
                {
                    if (xLock || yLock || zLock)
                    {
                        //Console.WriteLine("Moved:" + mov);

                        if (SMode == SpaceMode.Local || spaceLock == true)
                        {
                            //    Console.WriteLine("Moving");
                            CurNode.Move(mov, Space.Local);
                        }
                        else
                        {
                            //      Console.WriteLine("Moving node.");
                            if (CurNode.Top == null)
                            {
                                mov.Y = -mov.Y;
                                mov = mov * 0.1f;

                                var mv2 = Vector3.TransformPosition(mov, Cam.World);
                                mv2 = mv2 - Cam.WorldPos;
                                float oy2 = mv2.Y;
                                CurNode.LocalPos = CurNode.LocalPos + mv2;
                                //Console.WriteLine("Rooted!");
                            }

                            mov.Y = -mov.Y;

                            var mv = Vector3.TransformPosition(mov, Cam.World);
                            mv = mv - Cam.WorldPos;
                            float oy = mv.Y;
                            //mv.Y = mv.Z;
                            //mv.Z = oy;

                            var tn = CurNode.TopList;

                            if (tn == null) return;
                            Matrix4 nm = Matrix4.Identity;
                            //tn.Remove(tn[0]) ;
                            //tn.Remove(tn[0]);
                            //tn.Remove(tn[tn.Count - 1]);
                            //tn.Remove(tn[tn.Count - 1]);
                            //tn.Remove(tn[0]);
                            Console.WriteLine("!!!");
                            foreach (var tn2 in tn)
                            {
                                //    Console.WriteLine("Tn2:" + tn2.Name);
                                nm = nm * tn2.World.Inverted();
                            }
                            if (zLock)
                            {
                                //mv.Z = -mv.Z;
                            }
                            else
                            {
                                //mv.X = -mv.X;
                            }
                            if (zLock)
                            {
                            }
                            //mv.Y = -mv.Y;

                            mv = Vector3.TransformVector(mv, nm);
                            //mv.Y = -mv.Y;
                            //if (zLock)
                            // {
                            //}
                            CurNode.LocalPos = CurNode.LocalPos + mv;
                        }
                    }
                }
                else if (EMode == EditMode.Rotate)
                {
                    if (xLock || yLock || zLock)
                    {
                        float y1 = mov.Y;
                        mov.Y = mov.Z;
                        mov.Z = y1;
                        if (SMode == SpaceMode.Local)
                        {
                            CurNode.Turn(mov, Space.Local);
                        }
                        else
                        {
                            CurNode.Turn(mov, Space.Local); ;
                        }
                    }
                }
                else if (EMode == EditMode.Scale)
                {
                    mov.X = mov.X * 0.02f;
                    mov.Y = mov.Y * 0.02f;
                    mov.Z = mov.Z * 0.02f;
                    CurNode.LocalScale = CurNode.LocalScale + mov;
                }
            }
        }

        public int tX = 20, tY = 20;
        public bool dosize = false;

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EMode = EditMode.Move;
        }

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EMode = EditMode.Rotate;
        }

        private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EMode = EditMode.Scale;
        }

        private void ON_Load()
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
            Light.Diff = new Vector3(3, 3, 3);
            Light.CastShadows = true;
            Light.Spec = new Vector3(2, 2, 2);
            var l2 = new GraphLight3D();
            l2.LocalPos = new Vector3(-90, 90, -80);
            l2.Range = 800;
            l2.On = true;
            l2.Diff = new Vector3(0.8f, 1.8f, 1.8f);
            l2.CastShadows = true;
            var l3 = new GraphLight3D();
            l3.LocalPos = new Vector3(70, 120, 80);
            l3.Diff = new Vector3(1.4f, 2.2f, 2.2f);
            l3.Range = 800;
            l3.On = true;
            l3.CastShadows = true;

            Graph = new SceneGraph3D();
            Graph.Add(Grid);
            //  Graph.Add(Light);
            //   Graph.Add(l2);
            //   Graph.Add(l3);

            Cam = new GraphCam3D();
            Graph.Add(Cam);
            PRen = new Vivid3D.PostProcess.PostProcessRender(Width, Height);
            var vo = new Vivid3D.PostProcess.Processes.PPOutLine();
            var bpp = new Vivid3D.PostProcess.Processes.VPPBlur();
            //PPBlur.Blur = 0.4f;
            PRen.Scene = Graph;
            PRen.Add(vo);
            //  vo.SubProcesses.Add(bpp);
            //PRen.Add(bpp);

            Selected = new SceneGraph3D();
            Selected.Add(Cam);
            Selected.Add(Light);
            Selected.Add(Grid);
            vo.OutLineGraph = Selected;
            Vivid3D.ImageProcessing.ImageProcessor.InitIP();
            //Cam.Rot(new Vector3(45, 0, 0), Space.Local);
            //    Grid.Rot(new Vector3(-30, 0, 0), Space.Local);
            Cam.Pos(new Vector3(0, 100, 350), Space.Local);
            //  Cam.Rot(new Vector3(-10, 0, 0), Space.Local);
            //Cam.LookAt(new Vector3(0, 0, 0), Vector3.UnitY);
        }

        public SceneGraph3D Selected;
        private EditMode EMode = EditMode.Move;
        private SpaceMode SMode = SpaceMode.Global;

        public void AddLight(Vivid3D.Lighting.GraphLight3D l)
        {
            Graph.Lights.Add(l);
            l.Selected = ON_LightSelected;
        }

        public void ON_LightSelected(GraphNode3D node)
        {
            CurNode = node;
            ConsoleView.Log("Light Selected", "Info");
            Console.WriteLine("Light On :" + (node == null).ToString());
            VividEdit.VividED.Main.DockClassInspect.Inspect(node as object);
        }

        private void ON_Paint()
        {
            if (fast)
            {
                Light.LocalPos = Cam.LocalPos;
            }
            GL.ClearColor(0, 0, 0, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Graph?.Root.UpdateNode(0.1f);
            // if (run)
            //{
            //    Graph?.Render();
            // }
            Graph?.RenderShadows();
            PRen.Render();
            // Graph?.Render();
            Vivid3D.Effect.EMultiPass3D.LightMod = 0.4f;

            //Graph?.Render();
            LoadIcons();

            foreach (var rl in Graph.Lights)
            {
                var lp = Vivid3D.Pick.Picker.CamTo2D(Cam, rl.LocalPos);
                Vivid3D.Draw.VPen.BlendMod = Vivid3D.Draw.VBlend.Alpha;
                Vivid3D.Draw.VPen.Rect(lp.X - 16, lp.Y - 16, 32, 32, LightIcon);
            }

            if (CurNode != null)
            {
                // Console.WriteLine("Node render");

                var res = Vivid3D.Pick.Picker.CamTo2D(Cam, CurNode.WorldPos);

                tX = (int)res.X;
                tY = (int)res.Y;

                Vivid3D.Draw.VPen.BlendMod = Vivid3D.Draw.VBlend.Alpha;

                Vivid3D.Draw.VPen.Rect(tX - 3, tY - 3, 6, 6, new Vector4(1, 1, 1, 1));

                Vivid3D.Draw.VPen.Rect(tX - 6, tY - 80, 32, 32, YIcon);
                Vivid3D.Draw.VPen.Rect(tX + 80, tY - 3, 32, 32, XIcon);
                Vivid3D.Draw.VPen.Rect(tX + 32, tY - 32, 32, 32, ZIcon);
                switch (EMode)
                {
                    case EditMode.Move:
                        Vivid3D.Draw.VPen.Rect(6, 6, 16, 16, YIcon);
                        Vivid3D.Draw.VPen.Rect(30, 16, 16, 16, XIcon);
                        Vivid3D.Draw.VPen.Rect(20, 8, 16, 16, ZIcon);
                        break;

                    case EditMode.Rotate:
                        Vivid3D.Draw.VPen.Rect(6, 6, 64, 64, RIcon);
                        break;

                    case EditMode.Scale:
                        Vivid3D.Draw.VPen.Rect(6, 6, 64, 64, SIcon);
                        Vivid3D.Draw.VPen.Rect(tX - 16, tY - 16, 32, 32, SIcon);
                        break;
                }
            }
            Dis.SwapBuffers();
        }

        private void ON_Resize()
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
            if (dosize)
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
                Cam.Move(new Vector3(mX * moveS, mY * moveS, mZ * moveS), Space.Local);
                // Light.LocalPos = Cam.LocalPos;
            }

            Dis.Invalidate();
        }
    }

    public delegate void NodePicked(Vivid3D.Scene.GraphNode3D node);

    public enum EditMode
    {
        Move, Rotate, Scale
    }

    public enum SpaceMode
    {
        Local, Global
    }
}