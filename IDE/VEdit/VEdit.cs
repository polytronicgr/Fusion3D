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
using WeifenLuo.WinFormsUI;
using WeifenLuo.WinFormsUI.Docking;
using VividEdit.Forms;
using Vivid3D.Import;
namespace VEdit
{
    public partial class VEdit : Form
    {

        public static string BeginProject = "";
        public static ProjectCore.Project CurProject;
        public DockPanel MainDock;
        public Editor DockEdit3D;
        public AppGraph DockAppGraph;
        public ContentExplorer DockContentExplorer;
        public ConsoleView DockConsoleView;

        public VEdit()
        {
            InitializeComponent();

            MainDock = new DockPanel();
            MainDock.Dock = DockStyle.Fill;
            this.Controls.Add(MainDock);

        }

        private void VEdit_Load(object sender, EventArgs e)
        {
            DockAppGraph = new AppGraph();
            DockEdit3D = new Editor();
            DockContentExplorer = new ContentExplorer();
            DockConsoleView = new ConsoleView();
            DockAppGraph.Show(MainDock, DockState.DockLeft);
            DockConsoleView.Show(MainDock, DockState.DockBottom);
            DockContentExplorer.Show(MainDock, DockState.DockBottom);

            DockEdit3D.Show(MainDock, DockState.Document);
            DockEdit3D.dosize = true;
            DockEdit3D.InitSize();

            ConsoleView.Log("Vivid Editor started up.", "IDE");

            ConsoleView.Log("Loading project:" + BeginProject, "IDE");
            CurProject = new ProjectCore.Project(BeginProject);
            ConsoleView.Log("Project loaded.", "IDE");

            ConsoleView.Log("Scanning default Content folder:" + CurProject.ContentPath, "IDE");
            DockContentExplorer.SetFolder(CurProject.ContentPath);
            ConsoleView.Log("IDE initialized.", "IDE");

            ContentExplorer.FileOpen = FileOpen;
        }

        public void FileOpen(ContentFile file)
        {


            switch (file.Type)
            {
                case "3D":

                    var ent = Import.ImportNode(file.Path);
                    ent.SetMultiPass();
                    var e = ent as Vivid3D.Scene.GraphEntity3D;
                    var tm = new Vivid3D.Material.Material3D();
                    tm.TCol = new Vivid3D.Texture.VTex2D(CurProject.ContentPath + "\\2D\\Texture\\tex1.jpg", Vivid3D.Texture.LoadMethod.Single, true);
                    tm.TNorm = new Vivid3D.Texture.VTex2D(CurProject.ContentPath + "\\2D\\Texture\\tex1_nrm.png", Vivid3D.Texture.LoadMethod.Single,true);
                    e.SetMat(tm);

                    Console.WriteLine("Imported:" + file.Path);
                    ConsoleView.Log("Imported:" + file.Path + "-3D Data", "Content");
                    var g=DockEdit3D.GetGraph();
                    g.Add(ent);

                    break;
            }


        }

    }

}
