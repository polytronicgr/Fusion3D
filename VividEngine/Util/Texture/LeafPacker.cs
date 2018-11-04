using System.Collections.Generic;

namespace Vivid3D.Util.Texture
{
    public class TexTree
    {
        public static List<TreeLeaf> Leafs = new List<TreeLeaf>();

        public TreeLeaf Root
        {
            get;
            set;
        }

        public Rect RC;

        public TexTree ( int w, int h )
        {
            RC = new Rect ( 0, 0, w, h );
            //Root = ne
            //w TreeLeaf (new Rect(0,0,w,h));
        }

        public TreeLeaf Insert ( int w, int h, int id = -1 )
        {
            //return Root.Insert ( w, h );
            if ( Root == null )
            {
                Root = new TreeLeaf ( new Rect ( 0, 0, w, h ), id )
                {
                    Used = true
                };
                Root.Child [ 0 ] = new TreeLeaf ( new Rect ( 0, h, RC.W, RC.H - h ) );
                Root.Child [ 1 ] = new TreeLeaf ( new Rect ( w, 0, RC.W - w, h ) );
            }
            else
            {
                return Root.Insert ( w, h );
            }
            return Root;
        }
    }

    public class TreeLeaf
    {
        public TreeLeaf[] Child = new TreeLeaf[2];
        public Rect RC = new Rect();
        public int TexID = 0;
        public bool Used = false;

        public TreeLeaf ( Rect s, int id = -1 )
        {
            RC = s;
            TexID = -1;
            Child [ 0 ] = Child [ 1 ] = null;
            TexTree.Leafs.Add ( this );
        }

        public TreeLeaf Insert ( int w, int h )
        {
            if ( Used )
            {
                TreeLeaf rn =  Child [ 1 ].Insert ( w, h );
                if ( rn != null )
                {
                    return rn;
                }
                rn = Child [ 0 ].Insert ( w, h );
                if ( rn != null )
                {
                    return rn;
                }
            }
            else
            {
                if ( w < RC.W && h < RC.H )
                {
                    Used = true;

                    Child [ 0 ] = new TreeLeaf ( new Rect ( RC.X, RC.Y + h, RC.W, RC.H - h ) );
                    Child [ 1 ] = new TreeLeaf ( new Rect ( RC.X + w, RC.Y, RC.W - w, h ) );
                    RC.W = w;
                    RC.H = h;
                    return this;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public bool Fits ( int w, int h )
        {
            return w < RC.W && h < RC.H;
        }
    }

    public class Rect
    {
        public float X,Y,W,H;

        public Rect ( )
        {
            X = Y = W = H = 0;
        }

        public Rect ( float x, float y, float w, float h )
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }
    }
}