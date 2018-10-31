using System.Collections.Generic;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;

namespace VividEdit.Forms
{
    public delegate void OpenFile(ContentFile file);

    public partial class ContentExplorer : DockContent
    {
        public Bitmap IconFile;
        public Bitmap IconFolder;
        public List<ContentBase> Content = new List<ContentBase>();

        public static ContentExplorer Main = null;

        public static OpenFile FileOpen;

        public ContentExplorer()
        {
            InitializeComponent();
            Main = this;
        }

        public void SetFolder(string path)
        {
            contentPane.SetFolder(path);
        }
    }
}