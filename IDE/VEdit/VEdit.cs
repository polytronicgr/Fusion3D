using System;
using System.Windows.Forms;
using Vivid3D.Import;
using VividEdit.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VividEdit
{
    public partial class VividED : Form
    {
        public static string BeginProject = "";
        public static ProjectCore.Project CurProject;
        public static VividED Main = null;
        public AppGraph DockAppGraph;
        public ClassInspector DockClassInspect;
        public ConsoleView DockConsoleView;
        public ContentExplorer DockContentExplorer;
        public Editor DockEdit3D;
        public DockPanel MainDock;

        public VividED ( )
        {
            InitializeComponent ( );
            Main = this;
            MainDock = new DockPanel
            {
                Dock = DockStyle.Fill
            };
            Controls.Add ( MainDock );
        }

        public void AddEnt ( Vivid3D.Scene.GraphEntity3D e )
        {
            DockEdit3D.Graph.Add ( e );
            DockEdit3D.Selected.Root.Sub.Clear ( );
            // DockEdit3D.Selected.Add ( e );
            rebuildUI ( );
        }

        public void AddLight ( Vivid3D.Lighting.GraphLight3D l )
        {
            DockAppGraph.AddLight ( l );
            DockEdit3D.AddLight ( l );
        }

        public void BeginInspect ( )
        {
            DockClassInspect.BeginInspect ( );
            Console.WriteLine ( "Begun inspec." );
        }

        public void EndInspect ( )
        {
            Console.WriteLine ( "End Inspect." );
            DockClassInspect.EndInspect ( );
        }

        public void FileOpen ( ContentFile file )
        {
            switch ( file.Type )
            {
                case "Code":
                    System.Diagnostics.Process.Start ( file.Path );
                    break;

                case "V3D":
                    Console.WriteLine ( "Importing V3D:" + file.Path );

                    V3DM.ImportV3D vi = new V3DM.ImportV3D ( );

                    Vivid3D.Scene.GraphNode3D root = vi.ImportMesh ( file.Path );
                    root.SetMultiPass ( );
                    Vivid3D.Material.Material3D mat = new Vivid3D.Material.Material3D ( );
                    DockEdit3D.Graph.Add ( root );
                    DockEdit3D.Selected.Root.Sub.Clear ( );
                    rebuildUI ( );

                    break;

                case "3D":

                    AssImpImport.IPath = file.BasePath;
                    Console.WriteLine ( AssImpImport.IPath );
                    //while (true) { }

                    Vivid3D.Scene.GraphNode3D ent = Import.ImportNode ( file.Path );
                    ent.SetMultiPass ( );
                    Vivid3D.Scene.GraphEntity3D e = ent as Vivid3D.Scene.GraphEntity3D;
                    Vivid3D.Material.Material3D tm = new Vivid3D.Material.Material3D ( );
                    // tm.TCol = new Vivid3D.Texture.VTex2D(CurProject.ContentPath +
                    // "\\2D\\Texture\\tex1.jpg", Vivid3D.Texture.LoadMethod.Single, true); tm.TNorm
                    // = new Vivid3D.Texture.VTex2D(CurProject.ContentPath +
                    // "\\2D\\Texture\\tex1_nrm.png", Vivid3D.Texture.LoadMethod.Single,true); e.SetMat(tm);
                    ent.LocalScale = new OpenTK.Vector3 ( 1, 1, 1 );
                    Console.WriteLine ( "Imported:" + file.Path );
                    ConsoleView.Log ( "Imported:" + file.Path + "-3D Data", "Content" );
                    Vivid3D.Scene.SceneGraph3D g = DockEdit3D.GetGraph ( );
                    g.Add ( ent );
                    DockEdit3D.Selected.Root.Sub.Clear ( );
                    DockEdit3D.Selected.Add ( ent );
                    rebuildUI ( );

                    break;
            }
        }

        private void ON_Picked ( Vivid3D.Scene.GraphNode3D n )
        {
            Console.WriteLine ( "Selected:" + n.Name );
            DockAppGraph.Select ( n );

            OpenTK.Vector2 pos = Vivid3D.Pick.Picker.CamTo2D(DockEdit3D.Graph.Cams[0], n.WorldPos);

            DockEdit3D.tX = ( int ) pos.X;
            DockEdit3D.tY = ( int ) pos.Y;
        }

        private void ON_SelectNode ( Vivid3D.Scene.GraphNode3D n )
        {
            if ( n == null )
            {
                return;
            }

            DockEdit3D.Selected.Root.Sub.Clear ( );
            DockEdit3D.Selected.Root.AddProxy ( n );
            DockEdit3D.CurNode = n;
            if ( n is Vivid3D.Lighting.GraphLight3D )
            {
                Console.WriteLine ( "SelLight" );
            }
        }

        private void rebuildUI ( )

        {
            DockAppGraph.Graph = DockEdit3D.GetGraph ( );
            DockAppGraph.Rebuild ( );
        }

        private void VEdit_Load ( object sender, EventArgs e )
        {
            DockAppGraph = new AppGraph ( );
            DockEdit3D = new Editor ( );
            DockContentExplorer = new ContentExplorer ( );
            DockConsoleView = new ConsoleView ( );
            DockClassInspect = new ClassInspector ( );
            DockAppGraph.Show ( MainDock, DockState.DockLeft );
            DockConsoleView.Show ( MainDock, DockState.DockBottom );
            DockContentExplorer.Show ( MainDock, DockState.DockBottom );
            DockClassInspect.Show ( MainDock, DockState.DockRightAutoHide );
            // DockContentExplorer.Show(MainDock, DockState.Document);

            //DockEdit3D.Show(MainDock, DockState.Document);
            DockClassInspect.Show ( MainDock, DockState.DockRight );

            DockEdit3D.Show ( MainDock, DockState.Document );

            DockEdit3D.dosize = true;
            DockEdit3D.InitSize ( );

            ConsoleView.Log ( "Vivid Editor started up.", "IDE" );

            ConsoleView.Log ( "Loading project:" + BeginProject, "IDE" );
            CurProject = new ProjectCore.Project ( BeginProject );
            AssImpImport.IPath = CurProject.ContentPath;
            ConsoleView.Log ( "Project loaded.", "IDE" );

            ConsoleView.Log ( "Scanning default Content folder:" + CurProject.ContentPath, "IDE" );
            DockContentExplorer.SetFolder ( CurProject.ContentPath );
            ConsoleView.Log ( "IDE initialized.", "IDE" );

            ContentExplorer.FileOpen = FileOpen;
            DockEdit3D.Picked = ON_Picked;
            DockAppGraph.Selected = ON_SelectNode;
        }
    }
}