using ProjectCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectManager
{
    public partial class NewProjectForm : Form
    {
        public NewProjectForm ( )
        {
            InitializeComponent ( );
        }

        public string ImgPath = "";

        private void SelectImage_Click ( object sender, EventArgs e )
        {
            selectImg.Filter = "Images|*.jpg;*.bmp;*.png;*.tga";

            selectImg.ShowDialog ( );

            Image img = new Bitmap(new Bitmap(selectImg.FileName), 256, 256);

            ImgPath = selectImg.FileName;

            projImg.Image = img;
            projImg.Invalidate ( );
        }

        private void CreateProject_Click ( object sender, EventArgs e )
        {
            Project proj = new Project(projName.Text, projInfo.Text, projAuthor.Text, new Bitmap(new Bitmap(ImgPath), 256, 256));
            ProjectManager.Main.ScanForProjects ( );
            Close ( );
        }
    }
}