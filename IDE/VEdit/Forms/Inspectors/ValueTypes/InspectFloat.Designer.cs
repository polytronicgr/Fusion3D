namespace VividEdit.Forms.Inspectors.ValueTypes
{
    partial class InspectFloat
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
            this.ValueName = new System.Windows.Forms.Label();
            this.floatBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ValueName
            // 
            this.ValueName.AutoSize = true;
            this.ValueName.Location = new System.Drawing.Point(4, 4);
            this.ValueName.Name = "ValueName";
            this.ValueName.Size = new System.Drawing.Size(30, 13);
            this.ValueName.TabIndex = 0;
            this.ValueName.Text = "Float";
            // 
            // floatBox
            // 
            this.floatBox.Location = new System.Drawing.Point(45, 1);
            this.floatBox.Name = "floatBox";
            this.floatBox.Size = new System.Drawing.Size(165, 20);
            this.floatBox.TabIndex = 1;
            this.floatBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // InspectFloat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.floatBox);
            this.Controls.Add(this.ValueName);
            this.Name = "InspectFloat";
            this.Size = new System.Drawing.Size(227, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox floatBox;
        public System.Windows.Forms.Label ValueName;
    }
}
