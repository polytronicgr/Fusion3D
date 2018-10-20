namespace VividEdit.Forms.Inspectors.ValueTypes
{
    partial class InspectVec3
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.yBox = new System.Windows.Forms.TextBox();
            this.xBox = new System.Windows.Forms.TextBox();
            this.zBox = new System.Windows.Forms.TextBox();
            this.ValueName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(264, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Z";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(134, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Y";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // yBox
            // 
            this.yBox.Location = new System.Drawing.Point(154, 24);
            this.yBox.Name = "yBox";
            this.yBox.Size = new System.Drawing.Size(100, 20);
            this.yBox.TabIndex = 1;
            this.yBox.TextChanged += new System.EventHandler(this.yBox_TextChanged);
            // 
            // xBox
            // 
            this.xBox.Location = new System.Drawing.Point(28, 24);
            this.xBox.Name = "xBox";
            this.xBox.Size = new System.Drawing.Size(100, 20);
            this.xBox.TabIndex = 1;
            this.xBox.TextChanged += new System.EventHandler(this.xBox_TextChanged);
            // 
            // zBox
            // 
            this.zBox.Location = new System.Drawing.Point(284, 24);
            this.zBox.Name = "zBox";
            this.zBox.Size = new System.Drawing.Size(100, 20);
            this.zBox.TabIndex = 1;
            this.zBox.TextChanged += new System.EventHandler(this.zBox_TextChanged);
            // 
            // ValueName
            // 
            this.ValueName.AutoSize = true;
            this.ValueName.Location = new System.Drawing.Point(3, 8);
            this.ValueName.Name = "ValueName";
            this.ValueName.Size = new System.Drawing.Size(44, 13);
            this.ValueName.TabIndex = 2;
            this.ValueName.Text = "Vector3";
            // 
            // InspectVec3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ValueName);
            this.Controls.Add(this.xBox);
            this.Controls.Add(this.zBox);
            this.Controls.Add(this.yBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "InspectVec3";
            this.Size = new System.Drawing.Size(439, 51);
            this.Load += new System.EventHandler(this.InspectVec3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox yBox;
        private System.Windows.Forms.TextBox xBox;
        private System.Windows.Forms.TextBox zBox;
        public System.Windows.Forms.Label ValueName;
    }
}
