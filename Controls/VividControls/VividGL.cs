using System;
using System.Windows.Forms;

namespace VividControls
{
    public delegate void OnKeyDown ( Keys k );

    public delegate void OnKeyUp ( Keys k );

    public delegate void OnLoad ( );

    public delegate void OnMousedown ( MouseButtons b );

    public delegate void OnMouseMove ( int x, int y, int dx, int dy );

    public delegate void OnMouseUp ( MouseButtons b );

    public delegate void OnPaint ( );

    public delegate void OnResize ( );

    public partial class VividGL : OpenTK.GLControl
    {
        public OnKeyDown EvKeyDown = null;
        public OnKeyUp EvKeyUp = null;
        public OnLoad EvLoad = null;
        public OnMousedown EvMouseDown = null;
        public OnMouseMove EvMouseMoved = null;
        public OnMouseUp EvMouseUp = null;
        public OnPaint EvPaint = null;
        public OnResize EvResize = null;
        private int lX = -1, lY = -1;

        public VividGL ( OnLoad load ) : base ( new OpenTK.Graphics.GraphicsMode ( 32, 24, 0, 8 ) )
        {
            EvLoad = load;
            InitializeComponent ( );
        }

        public void Reset ( )
        {
            lX = -1;
            lY = -1;
        }

        private void VividGL_KeyDown ( object sender, KeyEventArgs e )
        {
            EvKeyDown?.Invoke ( e.KeyCode );
        }

        private void VividGL_KeyUp ( object sender, KeyEventArgs e )
        {
            EvKeyUp?.Invoke ( e.KeyCode );
        }

        private void VividGL_Load ( object sender, EventArgs e )
        {
            EvLoad?.Invoke ( );
        }

        private void VividGL_MouseDown ( object sender, MouseEventArgs e )
        {
            EvMouseDown?.Invoke ( e.Button );
        }

        private void VividGL_MouseMove ( object sender, MouseEventArgs e )
        {
            if ( lX == -1 )
            {
                lX = e.X;
                lY = e.Y;
            }
            int dx = e.X - lX;
            int dy = e.Y - lY;
            EvMouseMoved?.Invoke ( e.X, e.Y, dx, dy );
            lX = e.X;
            lY = e.Y;
        }

        private void VividGL_MouseUp ( object sender, MouseEventArgs e )
        {
            EvMouseUp?.Invoke ( e.Button );
        }

        private void VividGL_Paint ( object sender, PaintEventArgs e )
        {
            EvPaint?.Invoke ( );
        }

        private void VividGL_Resize ( object sender, EventArgs e )
        {
            EvResize?.Invoke ( );
        }
    }
}