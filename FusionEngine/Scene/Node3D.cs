using OpenTK;
using System;
using System.Collections.Generic;
using Fusion3D.Data;
using Fusion3D.Reflect;
using Fusion3D.Script;

namespace Fusion3D.Scene
{
    public delegate void NodeSelected ( Node3D node );

    public enum Space
    {
        Local, World
    }

    public class Node3D : FusionBase
    {
        public ClassIO ClassCopy
        {
            get;
            set;
        }

        public List<string> RenderTags = new List<string>();

        public bool BreakTop = false;

        public bool CastDepth = true;

        public bool CastShadows = true;

        public bool FaceCamera = false;

        public VInfoMap<string, object> Links = new VInfoMap<string, object>();

        public bool Lit = true;

        public EventHandler NameChanged = null;

        public bool On = true;

        public Matrix4 PrevWorld;

        public NodeSelected Selected = null;

        public List<Node3D> Sub = new List<Node3D>();

        public Node3D Top = null;

        private static int nn = 0;

        private readonly EventHandler PosChanged = null;

        private Vector3 _LocalPos = Vector3.Zero;

        private string _Name="";

        private List<ScriptLink> scripts = new List<ScriptLink> ( );

        public Node3D ( )
        {
            Init ( );
            Rot ( new Vector3 ( 0, 0, 0 ), Space.Local );
            Name = "Node:" + nn;
            nn++;
            LocalPos = new Vector3 ( 0, 0, 0 );
            LocalEular = new Vector3 ( 0, 0, 0 );
            LocalScale = new Vector3 ( 1, 1, 1 );
            LocalTurn = Matrix4.Identity;
            Running = false;
            RenderTags.Add ( "All" );
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
                PosChanged?.Invoke ( this, null );
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

        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                NameChanged?.Invoke ( this, null );
            }
        }

        public bool Running
        {
            get;
            set;
        }

        public ScriptLink Script { get; set; }

        public List<ScriptLink> Scripts { get => scripts; set => scripts = value; }

        public List<Node3D> TopList
        {
            get
            {
                List<Node3D> tl = new List<Node3D>();
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

        public Node3D TopTop
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

        public void Add ( Node3D node )
        {
            Sub.Add ( node );
            node.Top = this;
        }

        public virtual void SetLightmapTex ( Fusion3D.Texture.Texture2D tex )
        {
        }

        public virtual void AddLink ( string name, object obj )
        {
            Links.Add ( name, obj );
        }

        public void AddProxy ( Node3D node )
        {
            Sub.Add ( node );
        }

        public void AddTop ( List<Node3D> l )
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

        public void CopyProps ( )
        {
            ClassCopy = new ClassIO ( this );
            ClassCopy.Copy ( );
        }

        public void RestoreProps ( )
        {
            ClassCopy.Reset ( );
        }

        public void Begin ( )
        {
            foreach ( ScriptLink script in Scripts )
            {
                script.Compile ( this );
                script.Begin ( );
            }
            Running = true;

            foreach ( Node3D ent in Sub )
            {
                ent.Begin ( );
            }
        }

        public void End ( )
        {
            foreach ( ScriptLink script in Scripts )
            {
                script.Compile ( this );
                script.End ( );
            }

            Running = false;

            foreach ( Node3D ent in Sub )
            {
                ent.End ( );
            }
        }

        public virtual void Init ( )
        {
        }

        public void LookAt ( Node3D n )
        {
            LookAt ( n.WorldPos, new Vector3 ( 0, 1, 0 ) );
        }

        public void LookAt ( Vector3 p, Vector3 up )
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

        public void LookAtZero ( Vector3 p, Vector3 up )
        {
            Matrix4 m = Matrix4.LookAt(Vector3.Zero, p, up);
            LocalTurn = m;
        }

        public void Move ( Vector3 v, Space s )
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

        public void Pos ( Vector3 p, Space s )
        {
            if ( s == Space.Local )
            {
                LocalPos = p;
            }
        }

        public virtual void Present ( Cam3D c )
        {
        }

        // public void LookAt(Vector3 t)
        //{
        //   LocalTurn = Matrix4.LookAt(WorldPos, t, Vector3.UnitY);
        // }
        public virtual void PresentDepth ( Cam3D c )
        {
        }

        public virtual void Rot ( Vector3 r, Space s )
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
            if ( this is Entity3D || this is Terrain.Terrain3D )
            {
                dynamic tn = this;
                tn.Renderer = new Visuals.VRLightMap ( );
            }
            foreach ( Node3D n in Sub )
            {
                n.SetLightmap ( );
            }
        }

        public void SetMultiPass ( )
        {
            if ( this is Entity3D || this is Terrain.Terrain3D )
            {
                dynamic tn = this;
                tn.Renderer = new Visuals.RMultiPass ( );
            }
            foreach ( Node3D n in Sub )
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
            return Vector3.TransformPosition ( p, World );
        }

        public virtual void Turn ( Vector3 r, Space s )
        {
            Matrix4 t = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(r.Y)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(r.X)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(r.Z));
            LocalTurn = LocalTurn * t;
        }

        public virtual void Update ( )
        {
            UpdateNode ( 1.0f );
        }

        public virtual void UpdateNode ( float t )
        {
            if ( Running )
            {
                foreach ( ScriptLink s in Scripts )
                {
                    s.Update ( );
                }
            }

            foreach ( Node3D n in Sub )
            {
                n.UpdateNode ( t );
            }
        }

        public void Edit ( EditNode edit )
        {
            edit ( this );
            foreach ( Node3D sub in Sub )
            {
                sub.Edit ( edit );
            }
        }
    }

    public delegate void EditNode ( Node3D node );
}