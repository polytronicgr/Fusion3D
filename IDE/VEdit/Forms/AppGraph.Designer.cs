namespace VividEdit.Forms
{
    partial class AppGraph
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Nodes");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("States");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("UI");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("App Mode");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Lights");
            this.appTree = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointLightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alignToCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.placeInFrontOfCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadRootGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addOtherGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.terrainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFlatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // appTree
            // 
            this.appTree.ContextMenuStrip = this.contextMenuStrip1;
            this.appTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appTree.Location = new System.Drawing.Point(0, 0);
            this.appTree.Name = "appTree";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Nodes";
            treeNode2.Name = "Node1";
            treeNode2.Text = "States";
            treeNode3.Name = "Node2";
            treeNode3.Text = "UI";
            treeNode4.Name = "Node3";
            treeNode4.Text = "App Mode";
            treeNode5.Name = "Node0";
            treeNode5.Text = "Lights";
            this.appTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            this.appTree.Size = new System.Drawing.Size(800, 450);
            this.appTree.TabIndex = 0;
            this.appTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.appTree_AfterSelect);
            this.appTree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.appTree_MouseClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.nodeToolStripMenuItem,
            this.graphToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 92);
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointLightToolStripMenuItem,
            this.terrainToolStripMenuItem});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createToolStripMenuItem.Text = "Create";
            // 
            // pointLightToolStripMenuItem
            // 
            this.pointLightToolStripMenuItem.Name = "pointLightToolStripMenuItem";
            this.pointLightToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.pointLightToolStripMenuItem.Text = "Point Light";
            this.pointLightToolStripMenuItem.Click += new System.EventHandler(this.pointLightToolStripMenuItem_Click);
            // 
            // nodeToolStripMenuItem
            // 
            this.nodeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alignToCameraToolStripMenuItem,
            this.placeInFrontOfCameraToolStripMenuItem});
            this.nodeToolStripMenuItem.Name = "nodeToolStripMenuItem";
            this.nodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.nodeToolStripMenuItem.Text = "Node";
            // 
            // alignToCameraToolStripMenuItem
            // 
            this.alignToCameraToolStripMenuItem.Name = "alignToCameraToolStripMenuItem";
            this.alignToCameraToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.alignToCameraToolStripMenuItem.Text = "Align To Camera";
            this.alignToCameraToolStripMenuItem.Click += new System.EventHandler(this.alignToCameraToolStripMenuItem_Click);
            // 
            // placeInFrontOfCameraToolStripMenuItem
            // 
            this.placeInFrontOfCameraToolStripMenuItem.Name = "placeInFrontOfCameraToolStripMenuItem";
            this.placeInFrontOfCameraToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.placeInFrontOfCameraToolStripMenuItem.Text = "Place In Front Of Camera";
            this.placeInFrontOfCameraToolStripMenuItem.Click += new System.EventHandler(this.placeInFrontOfCameraToolStripMenuItem_Click);
            // 
            // graphToolStripMenuItem
            // 
            this.graphToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveGraphToolStripMenuItem,
            this.loadToolStripMenuItem});
            this.graphToolStripMenuItem.Name = "graphToolStripMenuItem";
            this.graphToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.graphToolStripMenuItem.Text = "Graph";
            // 
            // saveGraphToolStripMenuItem
            // 
            this.saveGraphToolStripMenuItem.Name = "saveGraphToolStripMenuItem";
            this.saveGraphToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.saveGraphToolStripMenuItem.Text = "Save Graph";
            this.saveGraphToolStripMenuItem.Click += new System.EventHandler(this.saveGraphToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadRootGraphToolStripMenuItem,
            this.addOtherGraphToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // loadRootGraphToolStripMenuItem
            // 
            this.loadRootGraphToolStripMenuItem.Name = "loadRootGraphToolStripMenuItem";
            this.loadRootGraphToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.loadRootGraphToolStripMenuItem.Text = "Load Root Graph";
            this.loadRootGraphToolStripMenuItem.Click += new System.EventHandler(this.loadRootGraphToolStripMenuItem_Click);
            // 
            // addOtherGraphToolStripMenuItem
            // 
            this.addOtherGraphToolStripMenuItem.Name = "addOtherGraphToolStripMenuItem";
            this.addOtherGraphToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.addOtherGraphToolStripMenuItem.Text = "Add Other Graph";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // terrainToolStripMenuItem
            // 
            this.terrainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFlatToolStripMenuItem});
            this.terrainToolStripMenuItem.Name = "terrainToolStripMenuItem";
            this.terrainToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.terrainToolStripMenuItem.Text = "Terrain";
            // 
            // newFlatToolStripMenuItem
            // 
            this.newFlatToolStripMenuItem.Name = "newFlatToolStripMenuItem";
            this.newFlatToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newFlatToolStripMenuItem.Text = "New Flat";
            this.newFlatToolStripMenuItem.Click += new System.EventHandler(this.newFlatToolStripMenuItem_Click);
            // 
            // AppGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.appTree);
            this.Name = "AppGraph";
            this.Text = "AppGraph";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView appTree;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointLightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alignToCameraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem placeInFrontOfCameraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveGraphToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadRootGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addOtherGraphToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem terrainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFlatToolStripMenuItem;
    }
}