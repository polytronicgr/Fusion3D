using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace VividEdit.Forms
{
    public partial class ContentPane : UserControl
    {
        public string CurPath;
        public Bitmap IconFile;
        public Bitmap IconFolder;
        public List<ContentBase> Content = new List<ContentBase>();
        public Stack<string> Paths = new Stack<string>();

        public ContentPane ( )
        {
            InitializeComponent ( );
            DoubleBuffered = true;

            IconFile = new Bitmap ( "data\\icon\\file1.png" );
            IconFolder = new Bitmap ( "data\\icon\\folder1.png" );
            DoubleBuffered = true;
        }

        public void SetFolder ( string path )
        {
            LastPath = CurPath;
            CurPath = path;
            Paths.Push ( path );
            DirectoryInfo dir = new DirectoryInfo(path);

            Content.Clear ( );

            foreach ( DirectoryInfo fold in dir.GetDirectories ( ) )
            {
                ContentFolder cfold = new ContentFolder
                {
                    Icon = IconFolder ,
                    Name = fold.Name ,
                    Path = fold.FullName
                };
                //cfold.DetermineType();

                Content.Add ( cfold );

                ConsoleView.Log ( "Scanned Folder:" + fold.Name, "Content" );
            }

            foreach ( FileInfo file in dir.GetFiles ( ) )
            {
                ContentFile cfile = new ContentFile
                {
                    Icon = IconFile ,
                    Name = file.Name ,
                    Path = file.FullName
                };
                cfile.DetermineType ( );
                cfile.BasePath = Paths.Peek ( );
                Content.Add ( cfile );
                ConsoleView.Log ( "Scanned File:" + file.Name + " Type:" + cfile.Type, "Content" );
            }

            CleanUI ( );
        }

        public void CleanUI ( )
        {
            int x = 5;
            int y = 5;
            bool skip = false;
            foreach ( ContentBase con in Content )
            {
                con.X = x;
                con.Y = y;

                x = x + 128;

                if ( x > Width - 64 )
                {
                    x = 5;
                    y = y + 96;
                }
                if ( y > Height - 64 )
                {
                    skip = true;
                }
                // if(x+64>=)
            }

            if ( !skip )
            {
                //Console.WriteLine("Skipped!");
                y = 0;
            }
            contentScroll.Maximum = y;
            contentScroll.Minimum = 0;
            contentScroll.Value = 0;

            Invalidate ( );
        }

        public int X = 0, Y = 0;

        private void contentPane_Resize ( object sender, EventArgs e )
        {
            CleanUI ( );
        }

        public ContentBase GetContentAt ( int x, int y )
        {
            foreach ( ContentBase con in Content )
            {
                int cx = con.X;
                int cy = con.Y - Y;

                if ( cy > Height - 32 )
                {
                    break;
                }

                if ( x >= cx && x <= cx + 128 && y >= cy && y <= cy + 128 )
                {
                    return con;
                }
            }
            return null;
        }

        public ContentBase ActiveContent = null;
        public int lastMD = 0;
        public string LastPath = "";

        private void ContentPane_MouseDown_1 ( object sender, MouseEventArgs e )
        {
            if ( e.Button == MouseButtons.Right )
            {
                Console.WriteLine ( "Paths:" + Paths.Count );
                if ( Paths.Count > 1 )
                {
                    Paths.Pop ( );
                    ContentExplorer.Main.SetFolder ( Paths.Pop ( ) );
                }
                return;
            }

            int td = Environment.TickCount;

            if ( td - lastMD < 300 )
            {
                ContentBase dc = GetContentAt(e.X, e.Y);
                if ( dc != null )
                {
                    if ( dc is ContentFolder )
                    {
                        ContentExplorer.Main.SetFolder ( dc.Path );
                    }
                    if ( dc is ContentFile )
                    {
                        Console.WriteLine ( "Opened:" + dc.Name );
                        ContentExplorer.FileOpen?.Invoke ( dc as ContentFile );
                    }
                    return;
                }
            }

            lastMD = td;

            ContentBase con = GetContentAt(e.X, e.Y);
            if ( con == ActiveContent )
            {
                ActiveContent = null;
            }
            else
            {
                ActiveContent = con;
            }
            Invalidate ( );
        }

        private void contentScroll_ValueChanged ( object sender, EventArgs e )
        {
            Y = contentScroll.Value;
            Invalidate ( );
        }

        private void ContentPane_Resize_1 ( object sender, EventArgs e )
        {
            CleanUI ( );
        }

        private void ContentPane_Paint ( object sender, PaintEventArgs e )
        {
            foreach ( ContentBase con in Content )
            {
                int dx = con.X;
                int dy = con.Y - Y;

                if ( dy > Height - 32 )
                {
                    break;
                }

                if ( con == ActiveContent )
                {
                    e.Graphics.FillRectangle ( Brushes.LightBlue, new RectangleF ( dx, dy + 32, 128, 128 - 32 ) );
                }

                e.Graphics.DrawImage ( con.Icon, new Rectangle ( dx + 32, dy + 32, 64, 64 ) );
                e.Graphics.DrawRectangle ( Pens.Black, new Rectangle ( dx + 8, dy + 100, 128 - 16, 16 ) );
                e.Graphics.DrawString ( con.Name, SystemFonts.IconTitleFont, Brushes.Black, new RectangleF ( dx + 8, dy + 100, 128 - 16, 16 ) );
            }

            e.Graphics.DrawString ( "Path:" + CurPath, SystemFonts.SmallCaptionFont, Brushes.Black, 5, 5 );
        }
    }
}