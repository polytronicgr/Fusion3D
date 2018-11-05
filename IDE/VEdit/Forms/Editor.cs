using OpenTK;
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
    public delegate void NodePicked ( Vivid3D.Scene.GraphNode3D node );

    public enum EditMode
    {
        Move, Rotate, Scale
    }

    public enum SpaceMode
    {
        Local, Global
    }

    public partial class Editor : DockContent
    {
        public GraphCam3D Cam;
        public GraphNode3D CurNode = null;
        public VividGL Dis = null;

        public bool dosize = false;
        public Vivid3D.XInput.XPad EditPad = null;
        public SceneGraph3D Graph = null;
        public GraphEntity3D Grid;
        public GraphLight3D Light;
        public int msX, msY;
        public Timer PadTimer = null;
        public NodePicked Picked = null;
        public Vivid3D.PostProcess.Processes.VPPBlur PPBlur;
        public Vivid3D.PostProcess.PostProcessRender PRen;
        public Vivid3D.Texture.VTex2D RIcon;
        public SceneGraph3D Selected;
        public int tX = 20, tY = 20;
        public Vivid3D.Texture.VTex2D XIcon, YIcon, ZIcon, SIcon, LightIcon;
        private bool allLock = false;
        private EditMode EMode = EditMode.Move;
        private bool fast = false;
        private bool iconLoaded = false;
        private float moveS = 2.0f;

        private Vector3 moveV = Vector3.Zero;

        private bool moving = false;

        private float mX = 0, mY = 0, mZ = 0;

        private bool PadEdit = true;
        private bool rotate = false;

        private SpaceMode SMode = SpaceMode.Global;

        private bool spaceLock = false;

        private bool xLock = false, yLock = false, zLock = false;

        public Editor ( )
        {
            InitializeComponent ( );
            Dis = new VividGL ( ON_Load )
            {
                // Dis.Dock = DockStyle.Fill;
                Size = new Size ( Width, Height ),
                EvPaint = ON_Paint,
                EvResize = ON_Resize,
                EvMouseMoved = ON_MouseMove,
                EvMouseDown = ON_MouseDown,
                EvMouseUp = ON_MouseUp,
                EvKeyDown = ON_KeyDown,
                EvKeyUp = ON_KeyUp
            };
            Controls.Add ( Dis );
            ContextMenuStrip = contextMenuStrip1;
            EditPad = new Vivid3D.XInput.XPad ( 0 );
            // dosize = true;
            PadTimer = new Timer
            {
                Interval = 30
            };
            PadTimer.Tick += ON_PadSync;
            PadTimer.Enabled = true;
        }

        public void AddLight ( Vivid3D.Lighting.GraphLight3D l )
        {
            Graph.Lights.Add ( l );
            l.Selected = ON_LightSelected;
        }

        public SceneGraph3D GetGraph ( )
        {
            return Graph;
        }

        public void InitSize ( )
        {
            ON_Resize ( );
        }

        public void LoadIcons ( )
        {
            if ( iconLoaded )
            {
                return;
            }

            RIcon = new Vivid3D.Texture.VTex2D ( "data\\icon\\iconrot.png", Vivid3D.Texture.LoadMethod.Single, true );
            XIcon = new Vivid3D.Texture.VTex2D ( "data\\icon\\iconx.png", Vivid3D.Texture.LoadMethod.Single, true );
            YIcon = new Vivid3D.Texture.VTex2D ( "data\\icon\\icony.png", Vivid3D.Texture.LoadMethod.Single, true );
            ZIcon = new Vivid3D.Texture.VTex2D ( "data\\icon\\iconz.png", Vivid3D.Texture.LoadMethod.Single, true );
            SIcon = new Vivid3D.Texture.VTex2D ( "data\\icon\\iconscale.png", Vivid3D.Texture.LoadMethod.Single, true );
            LightIcon = new Vivid3D.Texture.VTex2D ( "data\\icon\\iconlight.png", Vivid3D.Texture.LoadMethod.Single, true );
            iconLoaded = true;
        }

        public void ON_LightSelected ( GraphNode3D node )
        {
            CurNode = node;
            ConsoleView.Log ( "Light Selected", "Info" );
            Console.WriteLine ( "Light On :" + ( node == null ).ToString ( ) );
            VividEdit.VividED.Main.DockClassInspect.Inspect ( node as object );
        }

        private void button1_Click ( object sender, EventArgs e )
        {
            Graph.Copy ( );
            Graph.Begin ( );
        }

        private void button2_Click ( object sender, EventArgs e )
        {
            Graph.End ( );
            Graph.Restore ( );
        }

        private void Editor_Resize ( object sender, EventArgs e )
        {
            if ( dosize )
            {
                ON_Resize ( );
            }
            if ( Dis != null )
            {
                //ON_Resize();
            }
        }

        private void EditTimer_Tick ( object sender, EventArgs e )
        {
            if ( moving )
            {
                Cam.Move ( new Vector3 ( mX * moveS, mY * moveS, mZ * moveS ), Space.Local );
                // Light.LocalPos = Cam.LocalPos;
            }

            Dis.Invalidate ( );
        }

        private bool InRect ( int rx, int ry, int rw, int rh )
        {
            int x = msX;
            int y = msY;
            return ( x >= rx && y >= ry && x <= ( rx + rw ) && y <= ( ry + rh ) );
        }

        private void ON_KeyDown ( Keys k )
        {
            switch ( k )
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

        private void ON_KeyUp ( Keys k )
        {
            switch ( k )
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

        public Vivid3D.Composition.Composite Composer;
        public Vivid3D.Composition.Composite SelComposer;

        private void lightmapCurrentSceneToolStripMenuItem_Click ( object sender, EventArgs e )
        {
            ConsoleView.Log ( "Beginning lightmapping of current scene.", "LightMapper" );
            Vivid3D.Lighting.LightMapper.LightMapper lmapper = new Vivid3D.Lighting.LightMapper.LightMapper ( 2048, 2048, Graph );
            ConsoleView.Log ( "Lightmapping succesfull.", "LightMapper" );
        }

        private void ON_Load ( )
        {
            Dis.Size = new Size ( Width, Height );
            VForm.Set ( Width, Height );
            Grid = GeoGen.Quad ( 100, 100 );
            Grid.Rot ( new Vector3 ( -90, 0, 0 ), Space.Local );

            Grid.AlwaysAlpha = true;
            Vivid3D.Material.Material3D mat = new Vivid3D.Material.Material3D();
            Grid.Renderer = new VRNoFx ( );
            Vivid3D.Texture.VTex2D.Lut.Clear ( );
            mat.TCol = new Vivid3D.Texture.VTex2D ( "data\\ui\\grid.png", Vivid3D.Texture.LoadMethod.Single, true );
            Grid.Meshes [ 0 ].Mat = mat;
            Grid.RenderTags.Clear ( );
            Grid.RenderTags.Add ( "Grid" );
            Light = new GraphLight3D ( );
            Light.Pos ( new Vector3 ( 20, 120, 250 ), Space.Local );
            Light.Range = 800;
            Light.On = true;
            Light.Diff = new Vector3 ( 3, 3, 3 );
            Light.CastShadows = true;
            Light.Spec = new Vector3 ( 2, 2, 2 );
            GraphLight3D l2 = new GraphLight3D
            {
                LocalPos = new Vector3(-90, 90, -80),
                Range = 800,
                On = true,
                Diff = new Vector3(0.8f, 1.8f, 1.8f),
                CastShadows = true
            };
            GraphLight3D l3 = new GraphLight3D
            {
                LocalPos = new Vector3(70, 120, 80),
                Diff = new Vector3(1.4f, 2.2f, 2.2f),
                Range = 800,
                On = true,
                CastShadows = true
            };

            Graph = new SceneGraph3D ( );
            // Graph.Add ( Grid ); Graph.Add(Light); Graph.Add(l2); Graph.Add(l3);

            Cam = new GraphCam3D ( );
            Graph.Add ( Cam );
            Vivid3D.Composition.CompositerSet cs1 = new Vivid3D.Composition.CompositerSet (new Vivid3D.Composition.Compositers.BloomCompositer(),new Vivid3D.Composition.Compositers.OutlineCompositer());// new Vivid3D.Composition.Compositers.BloomCompositer(), new Vivid3D.Composition.Compositers.BlurCompositer ( ) );
            Vivid3D.Composition.CompositerSet cs2 = new Vivid3D.Composition.CompositerSet(new Vivid3D.Composition.Compositers.OutlineCompositer());
            //         Composer = new Vivid3D.Composition.Composite
            //{
            //    Graph = Graph
            // };
            Composer = cs1.Get ( );
            Composer.Graph = Graph;
            SelComposer = cs2.Get ( );

            //Composer.AddCompositer ( new Vivid3D.Composition.Compositers.BlurCompositer ( ) );
            // Composer.AddCompositer ( new Vivid3D.Composition.Compositers.BloomCompositer ( ) );

            // Composer.AddCompositer ( new Vivid3D.Composition.Compositers.OutlineCompositer ( ) );
            //Composer.AddCompositer ( new Vivid3D.Composition.Compositers.BlurCompositer ( ) );

            //  Composer.AddCompositer ( new Vivid3D.Composition.Compositers.OutlineCompositer ( ) );
            //PRen = new Vivid3D.PostProcess.PostProcessRender ( Width, Height );
            // Vivid3D.PostProcess.Processes.PPOutLine vo = new Vivid3D.PostProcess.Processes.PPOutLine();
            // Vivid3D.PostProcess.Processes.VPPBlur bpp = new Vivid3D.PostProcess.Processes.VPPBlur
            // {
            //PPBlur.Blur = 0.4f;
            //   Blur = 0.6f
            //};
            // PRen.Scene = Graph;

            // PRen.Add ( new Vivid3D.PostProcess.Processes.PPBloom ( ) ); PRen.Add ( vo );

            // PRen.Add ( bpp )
            ;

            //  vo.SubProcesses.Add(bpp);
            //PRen.Add(bpp);
            Graph.Add ( Grid );
            Graph.Add ( Light );
            Selected = new SceneGraph3D ( );
            Selected.Add ( Cam );
            Selected.Add ( Light );
            Selected.Add ( Grid );
            // vo.OutLineGraph = Selected;
            Vivid3D.ImageProcessing.ImageProcessor.InitIP ( );
            //Cam.Rot(new Vector3(45, 0, 0), Space.Local);
            //    Grid.Rot(new Vector3(-30, 0, 0), Space.Local);
            Cam.Pos ( new Vector3 ( 0, 100, 350 ), Space.Local );
            SelComposer.Graph = Selected;

            //  Cam.Rot(new Vector3(-10, 0, 0), Space.Local);
            //Cam.LookAt(new Vector3(0, 0, 0), Vector3.UnitY);
        }

        private void ON_MouseDown ( MouseButtons b )
        {
            if ( b == MouseButtons.Middle )
            {
                rotate = true;
            }
            if ( b == MouseButtons.Left )
            {
                xLock = false;
                yLock = false;
                zLock = false;
                if ( EMode == EditMode.Scale )
                {
                    if ( InRect ( tX - 16, tY - 16, 32, 32 ) )
                    {
                        allLock = true;
                        xLock = yLock = zLock = false;
                        return;
                    }
                }
                if ( InRect ( tX - 6, tY - 80, 32, 32 ) )
                {
                    yLock = true;
                    xLock = false;
                    zLock = false;
                    Console.WriteLine ( "Left" );
                    return;
                }
                if ( InRect ( tX + 80, tY - 3, 32, 32 ) )
                {
                    xLock = true;
                    yLock = false;
                    zLock = false;
                    return;
                }
                if ( InRect ( tX + 32, tY - 32, 32, 32 ) )
                {
                    zLock = true;
                    yLock = false;
                    xLock = false;
                    return;
                }

                CurNode = null;
                foreach ( GraphLight3D l in Graph.Lights )
                {
                    Vector2 sp = Vivid3D.Pick.Picker.CamTo2D(Cam, l.LocalPos);
                    if ( InRect ( ( int ) sp.X - 16, ( int ) sp.Y - 16, 32, 32 ) )
                    {
                        Console.WriteLine ( "Picked Light" );
                        l.Select ( );
                        return;
                    }
                }

                //Graph.CamPick(mX, mY);
                Vivid3D.Pick.PickResult res = Graph.CamPick(msX, msY);
                if ( res != null )
                {
                    Console.WriteLine ( "Picked:" + res.Node.Name );

                    Picked?.Invoke ( res.Node );
                    CurNode = res.Node;
                    Selected.Root.Sub.Clear ( );
                    Selected.Root.AddProxy ( CurNode );
                    VividEdit.VividED.Main.DockClassInspect.Inspect ( res.Node );
                }
                else
                {
                    // CurNode = null;
                }
            }
            // var l = Graph.GetList(true);
        }

        private void ON_MouseMove ( int x, int y, int dx, int dy )
        {
            float rx = dx * 0.2f;
            float ry = dy * 0.2f;
            if ( rotate )
            {
                Cam.Turn ( new Vector3 ( -ry, -rx, 0 ), Space.Local );
            }
            msX = x;
            msY = y;
            Vector3 mov = Vector3.Zero;
            if ( allLock )
            {
                mov = new Vector3 ( dx, dx, dx );
            }
            if ( xLock )
            {
                mov = new Vector3 ( dx, 0, 0 );
            }
            if ( yLock )
            {
                mov = new Vector3 ( 0, dy, 0 );
            }
            if ( zLock )
            {
                mov = new Vector3 ( 0, 0, dx );
            }
            if ( CurNode != null )
            {
                if ( EMode == EditMode.Rotate )
                {
                }
                if ( EMode == EditMode.Move )
                {
                    if ( xLock || yLock || zLock )
                    {
                        //Console.WriteLine("Moved:" + mov);

                        if ( SMode == SpaceMode.Local || spaceLock == true )
                        {
                            // Console.WriteLine("Moving");
                            CurNode.Move ( mov, Space.Local );
                        }
                        else
                        {
                            // Console.WriteLine("Moving node.");
                            if ( CurNode.Top == null )
                            {
                                mov.Y = -mov.Y;
                                mov = mov * 0.1f;

                                Vector3 mv2 = Vector3.TransformPosition(mov, Cam.World);
                                mv2 = mv2 - Cam.WorldPos;
                                float oy2 = mv2.Y;
                                CurNode.LocalPos = CurNode.LocalPos + mv2;
                                //Console.WriteLine("Rooted!");
                            }

                            mov.Y = -mov.Y;

                            Vector3 mv = Vector3.TransformPosition(mov, Cam.World);
                            mv = mv - Cam.WorldPos;
                            float oy = mv.Y;
                            //mv.Y = mv.Z;
                            //mv.Z = oy;

                            System.Collections.Generic.List<GraphNode3D> tn = CurNode.TopList;

                            if ( tn == null )
                            {
                                return;
                            }

                            Matrix4 nm = Matrix4.Identity;
                            //tn.Remove(tn[0]) ;
                            //tn.Remove(tn[0]);
                            //tn.Remove(tn[tn.Count - 1]);
                            //tn.Remove(tn[tn.Count - 1]);
                            //tn.Remove(tn[0]);
                            Console.WriteLine ( "!!!" );
                            foreach ( GraphNode3D tn2 in tn )
                            {
                                // Console.WriteLine("Tn2:" + tn2.Name);
                                nm = nm * tn2.World.Inverted ( );
                            }
                            if ( zLock )
                            {
                                //mv.Z = -mv.Z;
                            }
                            else
                            {
                                //mv.X = -mv.X;
                            }
                            if ( zLock )
                            {
                            }
                            //mv.Y = -mv.Y;

                            mv = Vector3.TransformVector ( mv, nm );
                            //mv.Y = -mv.Y;
                            //if (zLock)
                            // {
                            //}
                            CurNode.LocalPos = CurNode.LocalPos + mv;
                        }
                    }
                }
                else if ( EMode == EditMode.Rotate )
                {
                    if ( xLock || yLock || zLock )
                    {
                        float y1 = mov.Y;
                        mov.Y = mov.Z;
                        mov.Z = y1;
                        if ( SMode == SpaceMode.Local )
                        {
                            CurNode.Turn ( mov, Space.Local );
                        }
                        else
                        {
                            CurNode.Turn ( mov, Space.Local ); ;
                        }
                    }
                }
                else if ( EMode == EditMode.Scale )
                {
                    mov.X = mov.X * 0.02f;
                    mov.Y = mov.Y * 0.02f;
                    mov.Z = mov.Z * 0.02f;
                    CurNode.LocalScale = CurNode.LocalScale + mov;
                }
            }
        }

        private void ON_MouseUp ( MouseButtons b )
        {
            if ( b == MouseButtons.Middle )
            {
                rotate = false;
            }
            xLock = yLock = zLock = false;
            allLock = false;
        }

        private void ON_PadSync ( object sender, EventArgs e )
        {
            EditPad.Sync ( );
            if ( PadEdit == false )
            {
                return;
            }

            Cam.Turn ( new Vector3 ( -EditPad.RightY, -EditPad.RightX, 0 ), Space.Local );
            Cam.Move ( new Vector3 ( EditPad.LeftX * 2, 0, -EditPad.LeftY * 2 ), Space.Local );
            EditPad.SetFB ( EditPad.LeftTrigger, EditPad.RightTrigger );

            //throw new NotImplementedException ( );
        }

        private void ON_Paint ( )
        {
            if ( fast )
            {
                // Light.LocalPos = Cam.LocalPos;
            }
            GL.ClearColor ( 0, 0, 0, 1.0f );
            GL.Clear ( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );
            Graph?.Root.UpdateNode ( 0.1f );
            // if (run)
            //{
            Graph?.Render ( );
            // }
            Graph?.RenderShadows ( );
            // PRen.Render ( ); Graph?.Render();
            Vivid3D.Effect.EMultiPass3D.LightMod = 0.4f;
            //Selected.Render ( );
            //  Composer.Mix = Vivid3D.Composition.MixMode.Add;
            // Composer.Render ( );
            //SelComposer.Render ( );
            //GL.Disable ( EnableCap.Blend );
            GL.Clear ( ClearBufferMask.DepthBufferBit );
            //Grid.Render ( );-
            System.Collections.Generic.List<string> rt = new System.Collections.Generic.List<string>
            {
                "Grid"
            };
            Graph.RenderByTags ( rt );
            //Selected.Render ( );
            // Graph?.Render ( );
            LoadIcons ( );

            foreach ( GraphLight3D rl in Graph.Lights )
            {
                Vector2 lp = Vivid3D.Pick.Picker.CamTo2D(Cam, rl.LocalPos);
                Vivid3D.Draw.VPen.BlendMod = Vivid3D.Draw.VBlend.Alpha;
                Vivid3D.Draw.VPen.Rect ( lp.X - 16, lp.Y - 16, 32, 32, LightIcon );
            }

            if ( CurNode != null )
            {
                // Console.WriteLine("Node render");

                Vector2 res = Vivid3D.Pick.Picker.CamTo2D(Cam, CurNode.WorldPos);

                tX = ( int ) res.X;
                tY = ( int ) res.Y;

                Vivid3D.Draw.VPen.BlendMod = Vivid3D.Draw.VBlend.Alpha;

                Vivid3D.Draw.VPen.Rect ( tX - 3, tY - 3, 6, 6, new Vector4 ( 1, 1, 1, 1 ) );

                Vivid3D.Draw.VPen.Rect ( tX - 6, tY - 80, 32, 32, YIcon );
                Vivid3D.Draw.VPen.Rect ( tX + 80, tY - 3, 32, 32, XIcon );
                Vivid3D.Draw.VPen.Rect ( tX + 32, tY - 32, 32, 32, ZIcon );
                switch ( EMode )
                {
                    case EditMode.Move:
                        Vivid3D.Draw.VPen.Rect ( 6, 6, 16, 16, YIcon );
                        Vivid3D.Draw.VPen.Rect ( 30, 16, 16, 16, XIcon );
                        Vivid3D.Draw.VPen.Rect ( 20, 8, 16, 16, ZIcon );
                        break;

                    case EditMode.Rotate:
                        Vivid3D.Draw.VPen.Rect ( 6, 6, 64, 64, RIcon );
                        break;

                    case EditMode.Scale:
                        Vivid3D.Draw.VPen.Rect ( 6, 6, 64, 64, SIcon );
                        Vivid3D.Draw.VPen.Rect ( tX - 16, tY - 16, 32, 32, SIcon );
                        break;
                }
            }
            Dis.SwapBuffers ( );
        }

        private void ON_Resize ( )
        {
            Dis.Size = new Size ( Width, Height );
            VForm.SetSize ( Width, Height );
            Dis.Invalidate ( );
        }

        private void rotateToolStripMenuItem_Click ( object sender, EventArgs e )
        {
            EMode = EditMode.Rotate;
        }

        private void scaleToolStripMenuItem_Click ( object sender, EventArgs e )
        {
            EMode = EditMode.Scale;
        }

        private void toolStripMenuItem1_Click ( object sender, EventArgs e )
        {
            EMode = EditMode.Move;
        }
    }
}