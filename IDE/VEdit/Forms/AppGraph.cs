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
namespace VividEdit.Forms
{
    public partial class AppGraph : DockContent
    {
        public Vivid3D.Scene.SceneGraph3D Graph = null;
        public Dictionary<Vivid3D.Scene.GraphNode3D, TreeNode> nodeMap = new Dictionary<Vivid3D.Scene.GraphNode3D, TreeNode>();
        public List<Vivid3D.Scene.GraphNode3D> AllNodes = new List<Vivid3D.Scene.GraphNode3D>();
        public SelectedNode Selected = null;
        public AppGraph()
        {
            InitializeComponent();
        }
        public void Rebuild()
        {

            nodeMap.Clear();
            AllNodes.Clear();
            appTree.Nodes[0].Nodes.Clear();

            var nb = appTree.Nodes[0];

            AddNode(Graph.Root,nb);
            foreach(var l in lights)
            {
                var tn = LightTreeMap[l];
                nodeMap[l] = tn;
            }

        }
        public void Select(Vivid3D.Scene.GraphNode3D n)
        {
            if (nodeMap.ContainsKey(n) == false) return;
            var tn = nodeMap[n];
            tn.TreeView.SelectedNode = tn;
            return;
            foreach(var cn in AllNodes)
            {
                if(cn == n)
                {

                }
            }
        }
        void AddNode(Vivid3D.Scene.GraphNode3D n,TreeNode t)
        {


            AllNodes.Add(n);
            var nn = new TreeNode(n.Name);

            nodeMap.Add(n, nn);

            t.Nodes.Add(nn);

            foreach(var n2 in n.Sub)
            {
                AddNode(n2, nn);
            }

        }
        public Vivid3D.Scene.GraphNode3D SelectedNode = null;
        private void appTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var an = e.Node;
            foreach(var key in nodeMap.Keys)
            {
                if (nodeMap[key] == an)
                {
                    SelectedNode = key;
                    Selected?.Invoke(key);
                }
            }
        }

        private void appTree_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {

                //contextMenuStrip1.Show();
                 

            }
        }

        private void pointLightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var p = VEdit.VEdit.Main.DockEdit3D.Cam.Transform(new OpenTK.Vector3(0, 0, -150));
            var nl = new Vivid3D.Lighting.GraphLight3D();
            nl.LocalPos = p;
            nl.Range = 800;
            nl.Diff = new OpenTK.Vector3(2, 2, 2);
            nl.CastShadows = true;
            VEdit.VEdit.Main.AddLight(nl);
            VividEdit.Forms.ConsoleView.Log("Added point light at (" + p + ")", "Editor");
        }
        public Dictionary<TreeNode, Vivid3D.Lighting.GraphLight3D> TreeLightMap = new Dictionary<TreeNode, Vivid3D.Lighting.GraphLight3D>();
        public Dictionary<Vivid3D.Lighting.GraphLight3D,TreeNode> LightTreeMap = new Dictionary< Vivid3D.Lighting.GraphLight3D,TreeNode>();
        public void AddLight(Vivid3D.Lighting.GraphLight3D l)
        {
            var ltn = new TreeNode("Light:" + Vivid3D.Lighting.GraphLight3D.LightNum);
            TreeLightMap.Add(ltn, l);
            LightTreeMap.Add(l, ltn);
            lights.Add(l);
            nodeMap.Add(l, ltn);
            RebuildGraph();
            
        }
        public void RebuildGraph()
        {
            appTree.Nodes[4].Nodes.Clear();
            foreach(var l in lights)
            {
                var tn = LightTreeMap[l];
                appTree.Nodes[4].Nodes.Add(tn);
            }
        }

        public List<Vivid3D.Lighting.GraphLight3D> lights = new List<Vivid3D.Lighting.GraphLight3D>();

        private void alignToCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedNode != null)
            {
                SelectedNode.LocalPos = VEdit.VEdit.Main.DockEdit3D.Cam.LocalPos;
                SelectedNode.LocalTurn = VEdit.VEdit.Main.DockEdit3D.Cam.LocalTurn;
                Console.WriteLine("Align To Camera");

            }
        }

        private void placeInFrontOfCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedNode != null)
            {
                var np = VEdit.VEdit.Main.DockEdit3D.Cam.Transform(new OpenTK.Vector3(0, 0, -80));
                SelectedNode.LocalPos = np;

            }
        }

        private void saveGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "VividGraph|*.vg";
            saveFileDialog1.ShowDialog();

            VEdit.VEdit.Main.DockEdit3D.Graph.SaveGraph(saveFileDialog1.FileName);
        }
    }
    public delegate void SelectedNode(Vivid3D.Scene.GraphNode3D node);
}
