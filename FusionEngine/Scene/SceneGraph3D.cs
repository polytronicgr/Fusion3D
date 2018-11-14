using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.IO;
using Fusion3D.Lighting;
using Fusion3D.Reflect;

namespace Fusion3D.Scene
{
    public class SceneGraph3D
    {
        public Cam3D CamOverride = null;

        // public List<GraphNode3D> Nodes = new List<GraphNode3D>();
        public List<Cam3D> Cams = new List<Cam3D>();

        public List<Light3D> Lights = new List<Light3D>();

        public Node3D Root = new Entity3D();

        public SceneGraph3D SubGraph = null;

   

        public ClassIO ClassCopy
        {
            get;
            set;
        }

        public virtual void Add ( Cam3D c )
        {
            Cams.Add ( c );
        }

        public virtual void Add ( Light3D l )
        {
            Lights.Add ( l );
        }

        public virtual void Add ( Node3D n )
        {
            Root.Add ( n );
            n.Top = Root;
        }

        public void Begin ( )
        {
            Root.Begin ( );
        }

        public void BeginFrame ( )

        {
            BeginFrameNode ( Root );
            foreach ( Cam3D c in Cams )
            {
                c.StartFrame ( );
            }
        }

        public void BeginFrameNode ( Node3D node )
        {
            node.StartFrame ( );
            foreach ( Node3D snode in node.Sub )
            {
                BeginFrameNode ( snode );
            }
        }

        public void BeginRun ( )
        {
        }

        public virtual void Bind ( )
        {
        }

        public Fusion3D.Pick.PickResult CamPick ( int x, int y )
        {
            Pick.PickResult res = new Pick.PickResult ( );

            List<Node3D> nl = GetList ( true );

            Pick.Ray mr = Pick.Picker.CamRay ( Cams [ 0 ] , x , y );

            float cd = 0;
            bool firstHit = true;
            float cu = 0, cv = 0;
            Node3D cn = null;

            foreach ( Node3D n in nl )
            {
                Entity3D e = n as Entity3D;

                foreach ( Data.Mesh3D msh in e.Meshes )
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

        public void Copy ( )
        {
            ClassCopy = new Reflect.ClassIO ( this );
            ClassCopy.Copy ( );
            CopyNode ( Root );
        }

        public void CopyNode ( Node3D node )
        {
            node.CopyProps ( );
            foreach ( Node3D nn in node.Sub )
            {
                CopyNode ( nn );
            }
        }

        public void End ( )
        {
            Root.End ( );
        }

        public void EndRun ( )
        {
        }

        public List<Node3D> GetList ( bool meshesOnly )
        {
            List<Node3D> list = new List<Node3D>();
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
                Cam3D nc = new Cam3D();
                nc.Read ( );
                Cams.Add ( nc );
            }
            int lc = r.ReadInt32();
            for ( int i = 0; i < lc; i++ )
            {
                Light3D nl = new Fusion3D.Lighting.Light3D();
                nl.Read ( );
                Lights.Add ( nl );
            }
            Entity3D re = new Entity3D();
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

        public virtual void RenderByTags ( List<string> tags )
        {
            foreach ( Node3D n in Root.Sub )
            {
                RenderNodeByTags ( tags, n );
            }
        }

        public virtual void Render ( )
        {
            SubGraph?.Render ( );
            List<string> defTags = new List<string>
            {
                "All"
            };
            RenderNodeByTags ( defTags, Root );
        }

        public void SetLightmapTex ( Texture.Texture2D tex )
        {
            Root.SetLightmapTex ( tex );
        }

        public void EditGraph ( EditNode editor )
        {
            Root.Edit ( editor );
        }

        public virtual void RenderNodeByTags ( List<string> tags, Node3D node )
        {
            bool rt=false;
            foreach ( string tag in tags )
            {
                if ( node.RenderTags.Contains ( tag ) )
                {
                    rt = true;
                    RenderThis ( node );
                    break;
                }
            }
            if ( rt )
            {
                foreach ( Node3D n in node.Sub )
                {
                    RenderNodeByTags ( tags, n );
                }
            }
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

        public virtual void RenderNode ( Node3D node )
        {
            // Console.WriteLine("RenderNode:" + node.Name);
            RenderThis ( node );
            foreach ( Node3D snode in node.Sub )
            {
                // Console.WriteLine("Rendering Node:" + snode.Name);
                RenderNode ( snode );
            }
        }

        private void RenderThis ( Node3D node )
        {
            if ( node.Name == "Terrain" )
            {
            }
            if ( CamOverride != null )
            {
                foreach ( Light3D l in Lights )
                {
                    Light3D.Active = l;

                    node.Present ( CamOverride );
                }
            }
            else
            {
                foreach ( Cam3D c in Cams )
                {
                    if ( node.AlwaysAlpha )
                    {
                        Entity3D ge = node as Entity3D;
                        if ( ge.Renderer is Visuals.RMultiPass )
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
                        foreach ( Light3D l in Lights )
                        {
                            Light3D.Active = l;

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
        }

        public virtual void RenderNodeDepth ( Node3D node, Cam3D c )
        {
            if ( node.CastDepth )
            {
                node.PresentDepth ( c );
            }
            foreach ( Node3D snode in node.Sub )
            {
                RenderNodeDepth ( snode, c );
            }
        }

        public virtual void RenderNodeNoLights ( Node3D node )
        {
            if ( CamOverride != null )
            {
                foreach ( Light3D l in Lights )
                {
                    Light3D.Active = l;

                    node.Present ( CamOverride );
                }
            }
            else
            {
                foreach ( Cam3D c in Cams )
                {
                    GL.Disable ( EnableCap.Blend );

                    // Console.WriteLine("Presenting:" + node.Name);
                    node.Present ( c );

                    // foreach (var n in Nodes) { n.Present(c); }
                }
            }
            foreach ( Node3D snode in node.Sub )
            {
                // Console.WriteLine("Rendering Node:" + snode.Name);
                RenderNodeNoLights ( snode );
            }
        }

        public virtual void RenderNoLights ( )
        {
            Lighting.Light3D.Active = null;
            RenderNodeNoLights ( Root );
        }

        public virtual void RenderShadows ( )
        {
            int ls = 0;
            GL.Disable ( EnableCap.Blend );
            foreach ( Light3D l in Lights )
            {
                ls++;
                l.DrawShadowMap ( this );
                // Console.WriteLine("LightShadows:" + ls);
            }
        }

        public void Restore ( )
        {
            ClassCopy.Reset ( );
            RestoreNode ( Root );
        }

        public void RestoreNode ( Node3D node )
        {
            node.RestoreProps ( );
            foreach ( Node3D nn in node.Sub )
            {
                RestoreNode ( nn );
            }
        }

        public Vector3 Rot ( Vector3 p, Node3D n )
        {
            return Vector3.TransformPosition ( p, n.World );
        }

        public void SaveGraph ( string file )
        {
            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            Help.IOHelp.w = bw;

            bw.Write ( Cams.Count );
            foreach ( Cam3D c in Cams )
            {
                c.Write ( );
            }
            bw.Write ( Lights.Count );
            foreach ( Light3D c in Lights )
            {
                c.Write ( );
            }
            Entity3D r = Root as Entity3D;
            if ( Root != null )
            {
                r.Write ( );
            }

            fs.Flush ( );
            fs.Close ( );
        }

        public virtual void Update ( )
        {
           

            //var tp = new XInput.XPad(0);

            UpdateNode ( Root );
        }

        public virtual void UpdateNode ( Node3D node )
        {
            node.Update ( );
            foreach ( Node3D snode in node.Sub )
            {
                UpdateNode ( snode );
            }
        }

        private void NodeToList ( Node3D node, bool meshes, List<Node3D> list )
        {
            if ( meshes )
            {
                if ( node is Entity3D )
                {
                    list.Add ( node );
                }
            }
            else
            {
                list.Add ( node );
            }
            foreach ( Node3D n2 in node.Sub )
            {
                NodeToList ( n2, meshes, list );
            }
        }
    }
}