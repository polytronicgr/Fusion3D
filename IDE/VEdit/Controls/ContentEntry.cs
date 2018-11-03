using System.Drawing;
using System.Windows.Forms;

namespace VividEdit.Controls
{
    public partial class ContentEntry : UserControl
    {
        public Bitmap Look
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public bool Highlight
        {
            get;
            set;
        }

        public Forms.ContentBase Content
        {
            get;
            set;
        }

        public ContentEntry ( )
        {
            InitializeComponent ( );
            Content = null;
        }

        private void ContentEntry_Paint ( object sender, PaintEventArgs e )
        {
            if ( Look == null )
            {
                return;
            }

            if ( Highlight )
            {
                e.Graphics.FillRectangle ( Brushes.DarkBlue, new RectangleF ( 0, 0, 64, 64 ) );
            }

            e.Graphics.DrawImage ( Look, new Rectangle ( 0, 0, Width - 20, Height - 20 ) );

            e.Graphics.DrawString ( Text, SystemFonts.DefaultFont, Brushes.White, 10, Height - 15 );
        }
    }
}