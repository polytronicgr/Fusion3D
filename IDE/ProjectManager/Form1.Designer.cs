namespace ProjectManager
{
    partial class ProjectManager
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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Projects");
            this.projTree = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.projInfo = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // projTree
            // 
            this.projTree.Location = new System.Drawing.Point(12, 12);
            this.projTree.Name = "projTree";
            treeNode2.Name = "ProjRoot";
            treeNode2.Text = "Projects";
            this.projTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.projTree.Size = new System.Drawing.Size(191, 391);
            this.projTree.TabIndex = 0;
            this.projTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.projTree_AfterSelect);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 410);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(191, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "New Project";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.NewProject_Click);
            // 
            // projInfo
            // 
            this.projInfo.Location = new System.Drawing.Point(209, 184);
            this.projInfo.Name = "projInfo";
            this.projInfo.Size = new System.Drawing.Size(524, 219);
            this.projInfo.TabIndex = 2;
            this.projInfo.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(209, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Project Description";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(223, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 132);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Project Icon";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(541, 410);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(192, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Load Project";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // ProjectManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 447);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.projInfo);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.projTree);
            this.Name = "ProjectManager";
            this.Text = "Vivid - Project Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox projInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TreeView projTree;
    }
}

