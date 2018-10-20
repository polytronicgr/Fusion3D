namespace VividEdit.Forms.Inspectors
{
    partial class InspectorLightControl
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
            this.components = new System.ComponentModel.Container();
            this.updateInspector = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // updateInspector
            // 
            this.updateInspector.Enabled = true;
            this.updateInspector.Tick += new System.EventHandler(this.updateInspector_Tick);
            // 
            // InspectorLightControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "InspectorLightControl";
            this.Size = new System.Drawing.Size(357, 277);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer updateInspector;
    }
}
