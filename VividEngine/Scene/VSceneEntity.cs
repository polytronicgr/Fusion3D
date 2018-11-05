using OpenTK;
using System.Collections.Generic;
using Vivid3D.Data;
using Vivid3D.Material;
using Vivid3D.Visuals;

namespace Vivid3D.Scene
{
    public class LightMapOptions
    {
        public bool ReceiveLight = true;
        public bool CastShadows = true;

        public LightMapOptions ( )
        {
        }
    }

    public class GraphEntity3D : GraphNode3D
    {
        public Matrix4 GlobalInverse;
        public List<VMesh> Meshes = new List<VMesh>();
        public Physics.PyObject PO = null;
        public Physics.PyType PyType;
        public VRenderer Renderer = null;
        private float bw, bh, bd;

        private float sw, sh, sd;

        public LightMapOptions LightMapInfo = new LightMapOptions();

        public Bounds Bounds
        {
            get
            {
                sw = sh = sd = 20000;
                bw = bh = bd = -20000;
                GetBounds ( this );
                Bounds res = new Bounds
                {
                    W = bw - sw ,
                    H = bh - sh ,
                    D = bd - sd ,
                    MinX = sw ,
                    MaxX = bw ,
                    MinY = sh ,
                    MaxY = bh ,
                    MinZ = sd ,
                    MaxZ = bd
                };
                return res;
            }
        }

        public List<Vector3> GetAllVerts
        {
            get
            {
                List<Vector3> verts = new List<Vector3>();
                GetVerts ( verts, this );
                return verts;
            }
        }

        public void AddMesh ( VMesh mesh )
        {
            Meshes.Add ( mesh );
        }

        public virtual void Bind ( )
        {
        }

        public void Clean ( )
        {
            Meshes = new List<VMesh> ( );
            Renderer = null;
        }

        public void EnablePy ( Physics.PyType type )
        {
            PyType = type;
            switch ( PyType )
            {
                case Physics.PyType.Box:
                    PO = new Physics.PyDynamic ( type, this );
                    break;

                case Physics.PyType.Mesh:
                    PO = new Physics.PyStatic ( this );
                    break;
            }
        }

        public void GetBounds ( GraphEntity3D node )
        {
            foreach ( VMesh m in node.Meshes )
            {
                for ( int i = 0; i < m.NumVertices; i++ )
                {
                    int vid = i * 3;
                    if ( m.Vertices [ vid ] < sw )
                    {
                        sw = m.Vertices [ vid ];
                    }
                    if ( m.Vertices [ vid ] > bw )
                    {
                        bw = m.Vertices [ vid ];
                    }
                    if ( m.Vertices [ vid + 1 ] < sh )
                    {
                        sh = m.Vertices [ vid + 1 ];
                    }
                    if ( m.Vertices [ vid + 1 ] > bh )
                    {
                        bh = m.Vertices [ vid + 1 ];
                    }
                    if ( m.Vertices [ vid + 2 ] < sd )
                    {
                        sd = m.Vertices [ vid + 2 ];
                    }
                    if ( m.Vertices [ vid + 2 ] > bd )
                    {
                        bd = m.Vertices [ vid + 2 ];
                    }
                }
            }
            foreach ( GraphNode3D snode in node.Sub )
            {
                if ( snode is GraphEntity3D || snode is Terrain.GraphTerrain )
                {
                    GetBounds ( snode as GraphEntity3D );
                }
            }
        }

        public void GetVerts ( List<Vector3> verts, GraphEntity3D node )
        {
            foreach ( VMesh m in node.Meshes )
            {
                for ( int i = 0; i < m.NumVertices; i++ )
                {
                    int vid = i * 3;
                    Vector3 nv = new Vector3(m.Vertices[vid], m.Vertices[vid + 1], m.Vertices[vid + 2]);
                    nv = Vector3.TransformPosition ( nv, node.World );
                    verts.Add ( nv );
                    // verts.Add(m.Vertices[vid]); verts.Add(m.Vertices[vid + 1]);
                    // verts.Add(m.Vertices[vid + 2]);
                }
            }
            foreach ( GraphNode3D snode in node.Sub )
            {
                GetVerts ( verts, snode as GraphEntity3D );
            }
        }

        public override void Init ( )
        {
            // Renderer = new VRMultiPass();
        }

        public virtual void PostRender ( )
        {
        }

        /// <summary> To be called AFTER data asscoiation.
        public virtual void PreRender ( )
        {
        }

        public override void Present ( GraphCam3D c )
        {
            // GL.MatrixMode(MatrixMode.Projection); GL.LoadMatrix(ref c.ProjMat);
            SetMats ( c );
            Bind ( );
            PreRender ( );
            Render ( );
            PostRender ( );
            Release ( );
            // foreach (var s in Sub) { s.Present(c); }
        }

        public override void PresentDepth ( GraphCam3D c )
        {
            SetMats ( c );
            Bind ( );
            PreRender ( );
            RenderDepth ( );
            PostRender ( );
            Release ( );
            foreach ( GraphNode3D s in Sub )
            {
                s.PresentDepth ( c );
            }
        }

        public void Read ( )
        {
            int ns1 = Help.IOHelp.ReadInt();
            for ( int i = 0; i < ns1; i++ )
            {
                Script.ScriptLink sb = new Script.ScriptLink
                {
                    Name = Help.IOHelp.ReadString ( ) ,
                    FilePath = Help.IOHelp.ReadString ( )
                };
                Scripts.Add ( sb );
            }
            LocalTurn = Help.IOHelp.ReadMatrix ( );
            LocalPos = Help.IOHelp.ReadVec3 ( );
            LocalScale = Help.IOHelp.ReadVec3 ( );
            Name = Help.IOHelp.ReadString ( );
            AlwaysAlpha = Help.IOHelp.ReadBool ( );
            On = Help.IOHelp.ReadBool ( );
            int ns = Help.IOHelp.ReadInt();
            int mc = Help.IOHelp.ReadInt();
            for ( int m = 0; m < mc; m++ )
            {
                VMesh msh = new VMesh();
                msh.Read ( );
                Meshes.Add ( msh );
            }
            for ( int i = 0; i < ns; i++ )
            {
                GraphEntity3D gn = new GraphEntity3D();
                Sub.Add ( gn );
                gn.Top = this;
                gn.Read ( );
            }
            SetMultiPass ( );
        }

        public virtual void Release ( )
        {
        }

        public virtual void Render ( )
        {
            Effect.FXG.Ent = this;
            foreach ( VMesh m in Meshes )
            {
                Effect.FXG.Mesh = m;
                Renderer.Render ( m );
            }
        }

        public virtual void RenderDepth ( )
        {
            Effect.FXG.Ent = this;
            foreach ( VMesh m in Meshes )
            {
                Effect.FXG.Mesh = m;
                Renderer.RenderDepth ( m );
            }
        }

        public void ScaleMeshes ( float x, float y, float z )
        {
            DScale ( x, y, z, this );
        }

        public void SetMat ( Material3D mat )
        {
            foreach ( VMesh m in Meshes )
            {
                m.Mat = mat;
            }
            foreach ( GraphNode3D n in Sub )
            {
                if ( n is GraphEntity3D || n is Terrain.GraphTerrain )
                {
                    ;
                }

                {
                    GraphEntity3D ge = n as GraphEntity3D;
                    ge.SetMat ( mat );
                }
            }
        }

        public void SetMats ( GraphCam3D c )
        {
            Effect.FXG.Proj = c.ProjMat;
            Effect.FXG.Cam = c;
            // GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 mm = Matrix4.Identity;
            // mm = c.CamWorld;
            //mm = mm * Matrix4.Invert(Matrix4.CreateTranslation(c.WorldPos));

            mm = World;
            //var wp = LocalPos;
            //mm = mm*Matrix4.CreateTranslation(wp);
            //GL.LoadMatrix(ref mm);
            Effect.FXG.Local = mm;
            Effect.FXG.PrevLocal = PrevWorld;
        }

        public void Write ( )
        {
            Help.IOHelp.WriteInt ( Scripts.Count );
            foreach ( Script.ScriptLink s in Scripts )
            {
                Help.IOHelp.WriteString ( s.Name );
                Help.IOHelp.WriteString ( s.FilePath );
            }
            Help.IOHelp.WriteMatrix ( LocalTurn );
            Help.IOHelp.WriteVec ( LocalPos );
            Help.IOHelp.WriteVec ( LocalScale );
            Help.IOHelp.WriteString ( Name );
            Help.IOHelp.WriteBool ( AlwaysAlpha );
            Help.IOHelp.WriteBool ( On );
            Help.IOHelp.WriteInt ( Sub.Count );

            int mc = Meshes.Count;
            Help.IOHelp.WriteInt ( mc );
            foreach ( VMesh msh in Meshes )
            {
                msh.Write ( );
            }
            foreach ( GraphNode3D sn in Sub )
            {
                GraphEntity3D e = sn as GraphEntity3D;
                e.Write ( );
            }
        }

        private void DScale ( float x, float y, float z, GraphEntity3D node )
        {
            foreach ( VMesh m in node.Meshes )
            {
                m.Scale ( x, y, z );
            }
            foreach ( GraphNode3D snode in node.Sub )
            {
                DScale ( x, y, z, snode as GraphEntity3D );
            }
        }
    }
}