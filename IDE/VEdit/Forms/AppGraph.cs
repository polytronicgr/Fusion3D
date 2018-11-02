using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VividEdit.Forms
{
    public delegate void SelectedNode ( Vivid3D.Scene.GraphNode3D node );

    public partial class AppGraph : DockContent
    {
        public List<Vivid3D.Scene.GraphNode3D> AllNodes = new List<Vivid3D.Scene.GraphNode3D>();
        public Vivid3D.Scene.SceneGraph3D Graph = null;
        public List<Vivid3D.Lighting.GraphLight3D> lights = new List<Vivid3D.Lighting.GraphLight3D>();
        public Dictionary<Vivid3D.Lighting.GraphLight3D, TreeNode> LightTreeMap = new Dictionary<Vivid3D.Lighting.GraphLight3D, TreeNode>();
        public Dictionary<Vivid3D.Scene.GraphNode3D, TreeNode> nodeMap = new Dictionary<Vivid3D.Scene.GraphNode3D, TreeNode>();
        public SelectedNode Selected = null;

        public Vivid3D.Scene.GraphNode3D SelectedNode = null;

        public Dictionary<TreeNode, Vivid3D.Lighting.GraphLight3D> TreeLightMap = new Dictionary<TreeNode, Vivid3D.Lighting.GraphLight3D>();

        public AppGraph ( )
        {
            InitializeComponent ( );
        }

        public void AddLight ( Vivid3D.Lighting.GraphLight3D l )
        {
            TreeNode ltn = new TreeNode("Light:" + Vivid3D.Lighting.GraphLight3D.LightNum);
            TreeLightMap.Add ( ltn, l );
            LightTreeMap.Add ( l, ltn );
            lights.Add ( l );
            nodeMap.Add ( l, ltn );
            RebuildGraph ( );
        }

        public void Rebuild ( )
        {
            nodeMap.Clear ( );
            AllNodes.Clear ( );
            appTree.Nodes [ 0 ].Nodes.Clear ( );

            TreeNode nb = appTree.Nodes[0];

            RebuildGraph ( );
            AddNode ( Graph.Root, nb );
            foreach ( Vivid3D.Lighting.GraphLight3D l in lights )
            {
                if ( !LightTreeMap.ContainsKey ( l ) )
                {
                    LightTreeMap.Add ( l, new TreeNode ( "Light" ) );
                }
                TreeNode tn = LightTreeMap[l];
                nodeMap [ l ] = tn;
            }
        }

        public void RebuildGraph ( )
        {
            appTree.Nodes [ 4 ].Nodes.Clear ( );
            foreach ( Vivid3D.Lighting.GraphLight3D l in lights )
            {
                if ( LightTreeMap.ContainsKey ( l ) == false )
                {
                    LightTreeMap [ l ] = new TreeNode ( "Light:" );
                }
                TreeNode tn = LightTreeMap[l];
                appTree.Nodes [ 4 ].Nodes.Add ( tn );
            }
        }

        public void Select ( Vivid3D.Scene.GraphNode3D n )
        {
            if ( nodeMap.ContainsKey ( n ) == false )
            {
                return;
            }

            TreeNode tn = nodeMap[n];
            tn.TreeView.SelectedNode = tn;
            return;
            foreach ( Vivid3D.Scene.GraphNode3D cn in AllNodes )
            {
                if ( cn == n )
                {
                }
            }
        }

        private void ON_NameChange ( object o, EventArgs e )
        {
            Rebuild ( );
        }

        private void AddNode ( Vivid3D.Scene.GraphNode3D n, TreeNode t )
        {
            AllNodes.Add ( n );
            TreeNode nn = new TreeNode(n.Name);
            n.NameChanged = ON_NameChange;
            nodeMap.Add ( n, nn );

            t.Nodes.Add ( nn );

            Vivid3D.Scene.GraphEntity3D gen = n as Vivid3D.Scene.GraphEntity3D;

            foreach ( Vivid3D.Data.VMesh n4 in gen.Meshes )
            {
                // TreeNode g = new TreeNode(n4.Mat.TCol.W + " H:" + n4.Mat.TCol.H); t.Nodes.Add(g);

                for ( int i = 0; i < n4.NumVertices * 3; i += 3 )
                {
                    // TreeNode vn = new TreeNode("(" + n4.Vertices[i] + "," + n4.Vertices[i + 1] +
                    // "," + n4.Vertices[i + 2]);

                    // t.Nodes[t.Nodes.Count - 1].Nodes.Add(vn);
                }
            }
            foreach ( Vivid3D.Scene.GraphNode3D n2 in n.Sub )
            {
                AddNode ( n2, nn );
            }
        }

        private void alignToCameraToolStripMenuItem_Click ( object sender, EventArgs e )
        {
            if ( SelectedNode != null )
            {
                SelectedNode.LocalPos = VividEdit.VividED.Main.DockEdit3D.Cam.LocalPos;
                SelectedNode.LocalTurn = VividEdit.VividED.Main.DockEdit3D.Cam.LocalTurn;
                Console.WriteLine ( "Align To Camera" );
            }
        }

        private void appTree_AfterSelect ( object sender, TreeViewEventArgs e )
        {
            TreeNode an = e.Node;
            foreach ( Vivid3D.Scene.GraphNode3D key in nodeMap.Keys )
            {
                if ( nodeMap [ key ] == an )
                {
                    SelectedNode = key;
                    Selected?.Invoke ( key );
                }
            }
        }

        private void appTree_MouseClick ( object sender, MouseEventArgs e )
        {
            if ( e.Button == MouseButtons.Right )
            {
                //contextMenuStrip1.Show();
            }
        }

        private void fromHeightMapToolStripMenuItem_Click ( object sender, EventArgs e )
        {
            Vivid3D.Terrain.GraphTerrain nt = new Vivid3D.Terrain.GraphTerrain ( 1000 , 1000 , -1 , 64 , 64 , new Vivid3D.Texture.VTex2D ( VividEdit.VividED.CurProject.ContentPath + "Maps\\testMap1.png" , Vivid3D.Texture.LoadMethod.Single , false ) );
            Vivid3D.Material.Material3D tmat = nt.Meshes [ 0 ].Mat;

            tmat.TCol = new Vivid3D.Texture.VTex2D ( VividEdit.VividED.CurProject.ContentPath + "Texture\\Terrain\\rockCol2.png", Vivid3D.Texture.LoadMethod.Single, false );
            tmat.TNorm = new Vivid3D.Texture.VTex2D ( VividEdit.VividED.CurProject.ContentPath + "Texture\\Terrain\\rockNorm2.png", Vivid3D.Texture.LoadMethod.Single, false );
            tmat.TSpec = new Vivid3D.Texture.VTex2D ( VividEdit.VividED.CurProject.ContentPath + "Texture\\Terrain\\specnone.png", Vivid3D.Texture.LoadMethod.Single );

            VividEdit.VividED.Main.AddEnt ( nt );
        }

        private void loadRootGraphToolStripMenuItem_Click ( object sender, EventArgs e )
        {
            openFileDialog1.Filter = "VividGraph|*.vg";
            openFileDialog1.ShowDialog ( );
            VividEdit.VividED.Main.DockEdit3D.Graph = new Vivid3D.Scene.SceneGraph3D ( );
            VividEdit.VividED.Main.DockEdit3D.Graph.LoadGraph ( openFileDialog1.FileName );
            VividEdit.VividED.Main.DockEdit3D.Graph.Root.SetMultiPass ( );
            VividEdit.VividED.Main.DockEdit3D.PRen.SetScene ( VividEdit.VividED.Main.DockEdit3D.Graph );
            Graph = VividEdit.VividED.Main.DockEdit3D.Graph;
            lights = Graph.Lights;
            foreach ( Vivid3D.Lighting.GraphLight3D l in lights )
            {
                l.Selected = VividEdit.VividED.Main.DockEdit3D.ON_LightSelected;
            }
            VividEdit.VividED.Main.DockEdit3D.Selected.Root.Sub.Clear ( );
            VividEdit.VividED.Main.DockEdit3D.Cam = VividEdit.VividED.Main.DockEdit3D.Graph.Cams [ 0 ];
            VividEdit.VividED.Main.DockEdit3D.Selected.Cams.Clear ( );
            VividEdit.VividED.Main.DockEdit3D.Selected.Cams.Add ( VividEdit.VividED.Main.DockEdit3D.Cam );

            VividEdit.VividED.Main.DockEdit3D.Dis.Reset ( );
            Rebuild ( );
        }

        private void newFlatToolStripMenuItem_Click ( object sender, EventArgs e )
        {
            Vivid3D.Terrain.GraphTerrain nt = new Vivid3D.Terrain.GraphTerrain ( 1000 , 1000 , -1 , 64 , 64 );
            Vivid3D.Material.Material3D tmat = nt.Meshes [ 0 ].Mat;

            tmat.TCol = new Vivid3D.Texture.VTex2D ( VividEdit.VividED.CurProject.ContentPath + "Texture\\Terrain\\rockCol2.png", Vivid3D.Texture.LoadMethod.Single, false );
            tmat.TNorm = new Vivid3D.Texture.VTex2D ( VividEdit.VividED.CurProject.ContentPath + "Texture\\Terrain\\rockNorm2.png", Vivid3D.Texture.LoadMethod.Single, false );
            tmat.TSpec = new Vivid3D.Texture.VTex2D ( VividEdit.VividED.CurProject.ContentPath + "Texture\\Terrain\\specnone.png", Vivid3D.Texture.LoadMethod.Single );

            VividEdit.VividED.Main.AddEnt ( nt );
        }

        private void placeInFrontOfCameraToolStripMenuItem_Click ( object sender, EventArgs e )
        {
            if ( SelectedNode != null )
            {
                OpenTK.Vector3 np = VividEdit.VividED.Main.DockEdit3D.Cam.Transform ( new OpenTK.Vector3 ( 0 , 0 , -80 ) );
                SelectedNode.LocalPos = np;
            }
        }

        private void pointLightToolStripMenuItem_Click ( object sender, EventArgs e )
        {
            OpenTK.Vector3 p = VividEdit.VividED.Main.DockEdit3D.Cam.Transform ( new OpenTK.Vector3 ( 0 , 0 , -150 ) );
            Vivid3D.Lighting.GraphLight3D nl = new Vivid3D.Lighting.GraphLight3D
            {
                LocalPos = p ,

                Diff = new OpenTK.Vector3 ( 2 , 2 , 2 ) ,
                CastShadows = true
            };
            VividEdit.VividED.Main.AddLight ( nl );
            VividEdit.Forms.ConsoleView.Log ( "Added point light at (" + p + ")", "Editor" );
        }

        private void saveGraphToolStripMenuItem_Click ( object sender, EventArgs e )
        {
            saveFileDialog1.Filter = "VividGraph|*.vg";
            saveFileDialog1.ShowDialog ( );

            VividEdit.VividED.Main.DockEdit3D.Graph.SaveGraph ( saveFileDialog1.FileName );
        }
    }
}