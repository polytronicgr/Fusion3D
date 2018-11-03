namespace VividEdit.Forms
{
    partial class ContentPane
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
            this.contentScroll = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // contentScroll
            // 
            this.contentScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.contentScroll.Location = new System.Drawing.Point(133, 0);
            this.contentScroll.Name = "contentScroll";
            this.contentScroll.Size = new System.Drawing.Size(17, 150);
            this.contentScroll.TabIndex = 0;
            this.contentScroll.ValueChanged += new System.EventHandler(this.contentScroll_ValueChanged);
            // 
            // ContentPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
            this.Controls.Add(this.contentScroll);
            this.Name = "ContentPane";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ContentPane_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContentPane_MouseDown_1);
            this.Resize += new System.EventHandler(this.ContentPane_Resize_1);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar contentScroll;
    }
}
