using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.IO;
using Vivid3D.Lighting;

namespace Vivid3D.Scene
{
    public class SceneGraph3D
    {
        public GraphCam3D CamOverride = null;

        // public List<GraphNode3D> Nodes = new List<GraphNode3D>();
        public List<GraphCam3D> Cams = new List<GraphCam3D>();

        public List<GraphLight3D> Lights = new List<GraphLight3D>();
        public GraphNode3D Root = new GraphEntity3D();
        public SceneGraph3D SubGraph = null;

        public virtual void Add ( GraphCam3D c )
        {
            Cams.Add ( c );
        }

        public virtual void Add ( GraphLight3D l )
        {
            Lights.Add ( l );
        }

        public virtual void Add ( GraphNode3D n )
        {
            Root.Add ( n );
            n.Top = Root;
        }

        public void BeginFrame ( )

        {
            BeginFrameNode ( Root );
            foreach ( GraphCam3D c in Cams )
            {
                c.StartFrame ( );
            }
        }

        public void BeginFrameNode ( GraphNode3D node )
        {
            node.StartFrame ( );
            foreach ( GraphNode3D snode in node.Sub )
            {
                BeginFrameNode ( snode );
            }
        }

        public void Begin ( )
        {
            Root.Begin ( );
        }

        public void End ( )
        {
            Root.End ( );
        }

        public void BeginRun ( )
        {
        }

        public virtual void Bind ( )
        {
        }

        public Vivid3D.Pick.PickResult CamPick ( int x, int y )
        {
            Pick.PickResult res = new Pick.PickResult ( );

            List<GraphNode3D> nl = GetList ( true );

            Pick.Ray mr = Pick.Picker.CamRay ( Cams [ 0 ] , x , y );

            float cd = 0;
            bool firstHit = true;
            float cu = 0, cv = 0;
            GraphNode3D cn = null;

            foreach ( GraphNode3D n in nl )
            {
                GraphEntity3D e = n as GraphEntity3D;

                foreach ( Data.VMesh msh in e.Meshes )
                {
                    for ( int i = 0; i < msh.TriData.Length; i++ )
                    {
                        int v0 = msh.TriData[i].V0;
                        int v1 = msh.TriData[i].V1;
                        int v2 = msh.TriData[i].v2;
                        Vector3 r0, r1, r2;
                        r0 = Rot ( msh.VertexData [ v0 ].Pos, n );
                        r1 = Rot ( msh.VertexData [ v1 ].Pos, n );
                        r2 = Rot ( msh.VertexData [ v2 ].Pos, n );
                        Vector3? pr = Pick.Picker.GetTimeAndUvCoord(mr.pos, mr.dir, r0, r1, r2);
                        if ( pr == null )
                        {
                        }
                        else
                        {
                            Vector3 cr = (Vector3)pr;
                            if ( cr.X < cd || firstHit )
                            {
                                firstHit = false;
                                cd = cr.X;
                                cn = n;
                                cu = cr.Y;
                                cv = cr.Z;
                            }
                        }
                    }
                }
            }

            if ( firstHit )
            {
                return null;
            }

            res.Dist = cd;
            res.Node = cn;
            res.Pos = Pick.Picker.GetTrilinearCoordinateOfTheHit ( cd, mr.pos, mr.dir );
            res.Ray = mr;
            res.UV = new Vector3 ( cu, cv, 0 );

            return res;
        }

        public virtual void Clean ( )
        {
        }

        public void EndRun ( )
        {
        }

        public List<GraphNode3D> GetList ( bool meshesOnly )
        {
            List<GraphNode3D> list = new List<GraphNode3D>();
            NodeToList ( Root, meshesOnly, list );
            return list;
        }

        public void LoadGraph ( string file )
        {
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            Help.IOHelp.r = r;
            int cc = r.ReadInt32();
            for ( int i = 0; i < cc; i++ )
            {
                GraphCam3D nc = new GraphCam3D();
                nc.Read ( );
                Cams.Add ( nc );
            }
            int lc = r.ReadInt32();
            for ( int i = 0; i < lc; i++ )
            {
                GraphLight3D nl = new Vivid3D.Lighting.GraphLight3D();
                nl.Read ( );
                Lights.Add ( nl );
            }
            GraphEntity3D re = new GraphEntity3D();
            Root = re;
            re.Read ( );
            fs.Close ( );
        }

        public void PauseRun ( )
        {
        }

        public virtual void Release ( )
        {
        }

        public virtual void Render ( )
        {
            SubGraph?.Render ( );
            RenderNode ( Root );
        }

        public virtual void RenderDepth ( )
        {
            GL.ClearColor ( new OpenTK.Graphics.Color4 ( 1.0f, 1.0f, 1.0f, 1.0f ) );
            GL.Clear ( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );
            if ( CamOverride != null )
            {
                //foreach (var n in Nodes)
                //{
                RenderNodeDepth ( Root, CamOverride );

                //}
            }
            else

            {
                RenderNodeDepth ( Root, Cams [ 0 ] );
            }
            //foreach (var c in Cams)
        }

        public virtual void RenderNode ( GraphNode3D node )
        {
            // Console.WriteLine("RenderNode:" + node.Name);
            if ( node.Name == "Terrain" )
            {
            }
            if ( CamOverride != null )
            {
                foreach ( GraphLight3D l in Lights )
                {
                    GraphLight3D.Active = l;

                    node.Present ( CamOverride );
                }
            }
            else
            {
                foreach ( GraphCam3D c in Cams )
                {
                    if ( node.AlwaysAlpha )
                    {
                        GraphEntity3D ge = node as GraphEntity3D;
                        if ( ge.Renderer is Visuals.VRMultiPass )
                        {
                            ge.Renderer = new Visuals.VRNoFx ( );
                        }
                        GL.Enable ( EnableCap.Blend );
                        GL.BlendFunc ( BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha );
                        node.Present ( c );
                        continue;
                    }
                    if ( node.Lit )
                    {
                        bool first = true;
                        foreach ( GraphLight3D l in Lights )
                        {
                            GraphLight3D.Active = l;

                            if ( first )
                            {
                                first = false;
                                GL.Disable ( EnableCap.Blend );
                            }
                            else
                            {
                                GL.Enable ( EnableCap.Blend );
                                GL.BlendFunc ( BlendingFactor.One, BlendingFactor.One );
                            }
                            // Console.WriteLine("Presenting:" + node.Name);
                            if ( node.Name.Contains ( "Merged" ) )
                            {
                            }
                            node.Present ( c );

                            // foreach (var n in Nodes) { n.Present(c); }
                        }
                    }
                    else
                    {
                        if ( node.FaceCamera )
                        {
                            node.LookAt ( c.LocalPos, new Vector3 ( 0, 1, 0 ) );
                        }
                        GL.Enable ( EnableCap.Blend );
                        GL.BlendFunc ( BlendingFactor.Src1Alpha, BlendingFactor.OneMinusSrcAlpha );
                        GL.DepthMask ( false );
                        node.Present ( c );
                        GL.DepthMask ( true );
                    }
                }
            }
            foreach ( GraphNode3D snode in node.Sub )
            {
                // Console.WriteLine("Rendering Node:" + snode.Name);
                RenderNode ( snode );
            }
        }

        public virtual void RenderNodeDepth ( GraphNode3D node, GraphCam3D c )
        {
            if ( node.CastDepth )
            {
                node.PresentDepth ( c );
            }
            foreach ( GraphNode3D snode in node.Sub )
            {
                RenderNodeDepth ( snode, c );
            }
        }

        public virtual void RenderNodeNoLights ( GraphNode3D node )
        {
            if ( CamOverride != null )
            {
                foreach ( GraphLight3D l in Lights )
                {
                    GraphLight3D.Active = l;

                    node.Present ( CamOverride );
                }
            }
            else
            {
                foreach ( GraphCam3D c in Cams )
                {
                    GL.Disable ( EnableCap.Blend );

                    // Console.WriteLine("Presenting:" + node.Name);
                    node.Present ( c );

                    // foreach (var n in Nodes) { n.Present(c); }
                }
            }
            foreach ( GraphNode3D snode in node.Sub )
            {
                // Console.WriteLine("Rendering Node:" + snode.Name);
                RenderNodeNoLights ( snode );
            }
        }

        public virtual void RenderNoLights ( )
        {
            Lighting.GraphLight3D.Active = null;
            RenderNodeNoLights ( Root );
        }

        public virtual void RenderShadows ( )
        {
            int ls = 0;
            GL.Disable ( EnableCap.Blend );
            foreach ( GraphLight3D l in Lights )
            {
                ls++;
                l.DrawShadowMap ( this );
                // Console.WriteLine("LightShadows:" + ls);
            }
        }

        public Vector3 Rot ( Vector3 p, GraphNode3D n )
        {
            return Vector3.TransformPosition ( p, n.World );
        }

        public void SaveGraph ( string file )
        {
            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            Help.IOHelp.w = bw;

            bw.Write ( Cams.Count );
            foreach ( GraphCam3D c in Cams )
            {
                c.Write ( );
            }
            bw.Write ( Lights.Count );
            foreach ( GraphLight3D c in Lights )
            {
                c.Write ( );
            }
            GraphEntity3D r = Root as GraphEntity3D;
            if ( Root != null )
            {
                r.Write ( );
            }

            fs.Flush ( );
            fs.Close ( );
        }

        private XInput.XPad Pad0=null;

        public virtual void Update ( )
        {
            if ( Pad0 == null )
            {
                Pad0 = new XInput.XPad ( 0 );
                //Pad0.Update ( );
            }

            //var tp = new XInput.XPad(0);

            UpdateNode ( Root );
        }

        public virtual void UpdateNode ( GraphNode3D node )
        {
            node.Update ( );
            foreach ( GraphNode3D snode in node.Sub )
            {
                UpdateNode ( snode );
            }
        }

        private void NodeToList ( GraphNode3D node, bool meshes, List<GraphNode3D> list )
        {
            if ( meshes )
            {
                if ( node is GraphEntity3D )
                {
                    list.Add ( node );
                }
            }
            else
            {
                list.Add ( node );
            }
            foreach ( GraphNode3D n2 in node.Sub )
            {
                NodeToList ( n2, meshes, list );
            }
        }
    }
}