namespace VividControls
{
    partial class VividGL
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
            this.SuspendLayout();
            // 
            // VividGL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "VividGL";
            this.Size = new System.Drawing.Size(284, 166);
            this.Load += new System.EventHandler(this.VividGL_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.VividGL_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VividGL_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VividGL_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VividGL_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VividGL_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.VividGL_MouseUp);
            this.Resize += new System.EventHandler(this.VividGL_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
