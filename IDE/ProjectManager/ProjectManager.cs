using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ProjectManager
{
    public partial class ProjectManager : Form
    {
        public static ProjectManager Main = null;
        public NewProjectForm NewProject;
        public Dictionary<TreeNode, string> ProjMap = new Dictionary<TreeNode, string>();
        public ProjectCore.Project ActiveProject;

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
            foreach (var dir in pd.GetDirectories())
            {
                var name = dir.Name;
                var node = new TreeNode(name);
                projTree.Nodes[0].Nodes.Add(node);
                ProjMap.Add(node, name);

                //    P
            }
            projTree.Nodes[0].Expand();
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
            ActiveProject = np;
            iconImg.Image = np.Icon;
            iconImg.Invalidate();
        }

        private void LoadProject_Click(object sender, EventArgs e)
        {
            if (ActiveProject == null)
            {
                MessageBox.Show("No project selected.", "Vivid3D");
                return;
            }
            RunProject(ActiveProject);
        }

        public void RunProject(ProjectCore.Project proj)
        {
            var proc = new Process();
            Console.WriteLine("Starting:" + proj.IDEPath);

            proc.StartInfo = new ProcessStartInfo("VividEdit.exe", proj.IDEPath);
            proc.Start();
            Environment.Exit(2);
        }
    }
}