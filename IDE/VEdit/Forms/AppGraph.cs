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

        private void appTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var an = e.Node;
            foreach(var key in nodeMap.Keys)
            {
                if (nodeMap[key] == an)
                {
                    Selected?.Invoke(key);
                }
            }
        }

        private void appTree_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {

                

            }
        }
    }
    public delegate void SelectedNode(Vivid3D.Scene.GraphNode3D node);
}
