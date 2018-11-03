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
            splitContainer1.Panel1.BackColor = Color.FromArgb ( 53, 53, 53 );
            splitContainer1.Panel2.BackColor = Color.FromArgb ( 53, 53, 53 );
            splitContainer1.Panel1.Controls [ 0 ].BackColor = Color.FromArgb ( 53, 53, 53 );
            splitContainer1.Invalidate ( );

            splitContainer1.Panel1.Invalidate ( );
            splitContainer1.Panel2.Invalidate ( );
        }

        public void SetFolder ( string path )
        {
            contentPane.SetFolder ( path );
        }
    }
}