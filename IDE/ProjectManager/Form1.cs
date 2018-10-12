using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace ProjectManager
{
    public partial class ProjectManager : Form
    {

        public static ProjectManager Main = null;
        public NewProjectForm NewProject;
        public Dictionary<TreeNode, string> ProjMap = new Dictionary<TreeNode, string>();

        public ProjectManager()
        {
            InitializeComponent();
            Main = this;

            ScanForProjects();

        }

        public void ScanForProjects()
        {

            ProjMap.Clear();
            projTree.Nodes[0].Nodes.Clear();
            var pd = new DirectoryInfo(ProjectCore.Project.ProjectPath);
            foreach(var dir in pd.GetDirectories())
            {
                var name = dir.Name;
                var node = new TreeNode(name);
                projTree.Nodes[0].Nodes.Add(node);
                ProjMap.Add(node, name);

            //    P
            }

        }



        private void NewProject_Click(object sender, EventArgs e)
        {
            NewProject = new NewProjectForm();
            NewProject.Show();
        }

        private void projTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
           
            var node = e.Node;

            if (node == null) return;
            if (ProjMap.ContainsKey(node) == false) return;
            var path = ProjMap[node];

            var np = new ProjectCore.Project(path);

            var txt = "Project:" + np.Name + "\n";
            txt = txt + "Author:" + np.Author + "\n";
            txt = txt + "Info:" + np.Info;
            projInfo.Text = txt;



        }
    }
}
