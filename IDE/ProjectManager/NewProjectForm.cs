using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectCore;

namespace ProjectManager
{
    public partial class NewProjectForm : Form
    {
        public NewProjectForm()
        {
            InitializeComponent();
        }

        private void SelectImage_Click(object sender, EventArgs e)
        {
            selectImg.Filter = "Images|*.jpg;*.bmp;*.png;*.tga";
           
            selectImg.ShowDialog();

            Image img = new Bitmap(new Bitmap(selectImg.FileName), 256, 256);



            projImg.Image = img;
            projImg.Invalidate();

        }

        private void CreateProject_Click(object sender, EventArgs e)
        {

            var proj = new Project(projName.Text, projInfo.Text, projAuthor.Text);
            ProjectManager.Main.ScanForProjects();
            this.Close();

        }
    }
}
