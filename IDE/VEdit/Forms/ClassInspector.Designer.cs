namespace VividEdit.Forms
{
    partial class ClassInspector
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
            this.inspectBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // inspectBox
            // 
            this.inspectBox.AutoSize = true;
            this.inspectBox.Checked = true;
            this.inspectBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.inspectBox.Location = new System.Drawing.Point(13, 13);
            this.inspectBox.Name = "inspectBox";
            this.inspectBox.Size = new System.Drawing.Size(81, 17);
            this.inspectBox.TabIndex = 0;
            this.inspectBox.Text = "Inspecting?";
            this.inspectBox.UseVisualStyleBackColor = true;
            this.inspectBox.CheckedChanged += new System.EventHandler(this.inspectBox_CheckedChanged);
            this.inspectBox.CheckStateChanged += new System.EventHandler(this.inspectBox_CheckStateChanged);
            // 
            // ClassInspector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.inspectBox);
            this.Name = "ClassInspector";
            this.Text = "ClassInspector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox inspectBox;
    }
}