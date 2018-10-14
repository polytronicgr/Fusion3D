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

        }

    }

}
