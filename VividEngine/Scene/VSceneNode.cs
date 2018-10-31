using OpenTK;
using System;
using System.Collections.Generic;
using Vivid3D.Data;
using Vivid3D.Script;

namespace Vivid3D.Scene
{
    public delegate void NodeSelected ( GraphNode3D node );

    public enum Space
    {
        Local, World
    }

    public class GraphNode3D : VividBase
    {
        public bool BreakTop = false;
        public bool CastDepth = true;
        public bool CastShadows = true;
        public bool FaceCamera = false;
        public VInfoMap<string, object> Links = new VInfoMap<string, object>();
        public bool Lit = true;
        public string Name = "";
        public bool On = true;
        public Matrix4 PrevWorld;
        public NodeSelected Selected = null;
        public List<GraphNode3D> Sub = new List<GraphNode3D>();
        public GraphNode3D Top = null;
        private static int nn = 0;
        private Vector3 _LocalPos = Vector3.Zero;
        private readonly EventHandler PosChanged = null;

        public GraphNode3D ( )
        {
            Init ( );
            Rot ( new Vector3 ( 0 , 0 , 0 ) , Space.Local );
            Name = "Node:" + nn;
            nn++;
            LocalPos = new Vector3 ( 0 , 0 , 0 );
            LocalEular = new Vector3 ( 0 , 0 , 0 );
            LocalScale = new Vector3 ( 1 , 1 , 1 );
            LocalTurn = Matrix4.Identity;
        }

        public bool AlwaysAlpha
        {
            get
                  ;
            set;
        }

        public Vector3 LocalEular
        {
            get;
            set;
        }

        public Vector3 LocalPos
        {
            get => _LocalPos;
            set
            {
                _LocalPos = value;
                PosChanged?.Invoke ( this , null );
            }
        }

        public Vector3 LocalScale
        {
            get;
            set;
        }

        public Matrix4 LocalTurn
        {
            get;
            set;
        }

        public ScriptBase Script { get; set; }

        public List<GraphNode3D> TopList
        {
            get
            {
                List<GraphNode3D> tl = new List<GraphNode3D>();
                if ( Top != null )
                {
                    Top.AddTop ( tl );
                    return tl;
                }
                else
                {
                    return null;
                }
                return null;
            }
        }

        public GraphNode3D TopTop
        {
            get
            {
                if ( Top != null )
                {
                    return Top.TopTop;
                }
                else
                {
                    return this;
                }
            }
        }

        public Matrix4 World
        {
            get
            {
                Matrix4 r = Matrix4.Identity;
                if ( Top != null )
                {
                    r = Top.World;
                }
                r = ( Matrix4.CreateScale ( LocalScale ) * LocalTurn * Matrix4.CreateTranslation ( LocalPos ) ) * r;

                return r;
            }
        }

        public Vector3 WorldPos
        {
            get
            {
                Matrix4 v = World;

                return v.ExtractTranslation ( );
            }
        }

        public void Add ( GraphNode3D node )
        {
            Sub.Add ( node );
            node.Top = this;
        }

        public virtual void AddLink ( string name , object obj )
        {
            Links.Add ( name , obj );
        }

        public void AddProxy ( GraphNode3D node )
        {
            Sub.Add ( node );
        }

        public void AddTop ( List<GraphNode3D> l )
        {
            l.Add ( this );
            if ( BreakTop )
            {
                return;
            }

            if ( Top != null )
            {
                Top.AddTop ( l );
            }
        }

        public virtual void Init ( )
        {
        }

        public void LookAt ( GraphNode3D n )
        {
            LookAt ( n.WorldPos , new Vector3 ( 0 , 1 , 0 ) );
        }

        public void LookAt ( Vector3 p , Vector3 up )
        {
            Matrix4 m = Matrix4.LookAt(Vector3.Zero, p - LocalPos, up);
            //Console.WriteLine("Local:" + LocalPos.ToString() + " TO:" + p.ToString());
            //m=m.ClearTranslation();

            //   m = m.Inverted();
            //m = m.ClearScale();
            //m = m.ClearProjection();
            m = m.Inverted ( );

            LocalTurn = m;
        }

        public void LookAtZero ( Vector3 p , Vector3 up )
        {
            Matrix4 m = Matrix4.LookAt(Vector3.Zero, p, up);
            LocalTurn = m;
        }

        public void Move ( Vector3 v , Space s )
        {
            // v.X = -v.X;
            if ( s == Space.Local )
            {
                //Console.WriteLine("NV:" + v);
                Vector3 ov = WorldPos;
                Vector3 nv = Vector3.TransformPosition(v, World);
                Vector3 mm = nv - ov;

                LocalPos = LocalPos + mm;//Matrix4.Invert(nv);
                //LocalPos = LocalPos + new Vector3(nv.X, nv.Y, nv.Z);
            }
        }

        public void Pos ( Vector3 p , Space s )
        {
            if ( s == Space.Local )
            {
                LocalPos = p;
            }
        }

        public virtual void Present ( GraphCam3D c )
        {
        }

        // public void LookAt(Vector3 t)
        //{
        //   LocalTurn = Matrix4.LookAt(WorldPos, t, Vector3.UnitY);
        // }
        public virtual void PresentDepth ( GraphCam3D c )
        {
        }

        public virtual void Rot ( Vector3 r , Space s )
        {
            if ( s == Space.Local )
            {
                LocalTurn = Matrix4.CreateRotationY ( MathHelper.DegreesToRadians ( r.Y ) ) * Matrix4.CreateRotationX ( MathHelper.DegreesToRadians ( r.X ) ) * Matrix4.CreateRotationZ ( MathHelper.DegreesToRadians ( r.Z ) );
            }
        }

        public void Select ( )
        {
            Selected?.Invoke ( this );
        }

        public void SetLightmap ( )
        {
            if ( this is GraphEntity3D || this is Terrain.GraphTerrain )
            {
                dynamic tn = this;
                tn.Renderer = new Visuals.VRLightMap ( );
            }
            foreach ( GraphNode3D n in Sub )
            {
                n.SetLightmap ( );
            }
        }

        public void SetMultiPass ( )
        {
            if ( this is GraphEntity3D || this is Terrain.GraphTerrain )
            {
                dynamic tn = this;
                tn.Renderer = new Visuals.VRMultiPass ( );
            }
            foreach ( GraphNode3D n in Sub )
            {
                n.SetMultiPass ( );
            }
        }

        public void StartFrame ( )
        {
            PrevWorld = World;
        }

        public Vector3 Transform ( Vector3 p )
        {
            return Vector3.TransformPosition ( p , World );
        }

        public virtual void Turn ( Vector3 r , Space s )
        {
            Matrix4 t = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(r.Y)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(r.X)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(r.Z));
            LocalTurn = LocalTurn * t;
        }

        public virtual void Update ( )
        {
        }

        public virtual void UpdateNode ( float t )
        {
            foreach ( GraphNode3D n in Sub )
            {
                n.UpdateNode ( t );
            }
        }
    }
}