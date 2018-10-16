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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Nodes");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("States");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("UI");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("App Mode");
            this.appTree = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // appTree
            // 
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
            this.appTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.appTree.Size = new System.Drawing.Size(800, 450);
            this.appTree.TabIndex = 0;
            // 
            // AppGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.appTree);
            this.Name = "AppGraph";
            this.Text = "AppGraph";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView appTree;
    }
}