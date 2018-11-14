namespace VividScriptExplorer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose ( );
            }
            base.Dispose ( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Code Structure");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Modules");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Global Funcs");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Global Variables");
            this.codeTree = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // codeTree
            // 
            this.codeTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.codeTree.Location = new System.Drawing.Point(0, 0);
            this.codeTree.Name = "codeTree";
            treeNode1.Name = "Node0";
            treeNode1.Text = "Code Structure";
            treeNode2.Name = "Node1";
            treeNode2.Text = "Modules";
            treeNode3.Name = "Node2";
            treeNode3.Text = "Global Funcs";
            treeNode4.Name = "Node3";
            treeNode4.Text = "Global Variables";
            this.codeTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.codeTree.Size = new System.Drawing.Size(338, 450);
            this.codeTree.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(344, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(186, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Load Code";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.codeTree);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView codeTree;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

