using System.Collections.Generic;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;

namespace VividEdit.Forms
{
    public delegate void OpenFile ( ContentFile file );

    public partial class ContentExplorer : DockContent
    {
        public static OpenFile FileOpen;
        public static ContentExplorer Main = null;
        public List<ContentBase> Content = new List<ContentBase>();
        public Bitmap IconFile;
        public Bitmap IconFolder;

        public ContentExplorer ( )
        {
            InitializeComponent ( );
            Main = this;
        }

        public void SetFolder ( string path )
        {
            contentPane.SetFolder ( path );
        }
    }
}