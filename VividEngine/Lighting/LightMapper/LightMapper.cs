using System;
using Vivid3D.Scene;
using Vivid3D.Util.Texture;

namespace Vivid3D.Lighting.LightMapper
{
    public class LightMapper
    {
        public SceneGraph3D Graph { get; set; }
        public SceneGraph3D ResultGraph { get; set; }
        public TexTree FinalMap { get; set; }

        public int Tri_W,Tri_H;

        public LightMapper ( int mapWidth, int mapHeight, SceneGraph3D graph, int tri_w = 16, int tri_h = 16 )
        {
            Graph = graph;
            ResultGraph = new SceneGraph3D
            {
                Lights = graph.Lights,
                Cams = graph.Cams
            };

            FinalMap = new TexTree ( mapWidth, mapHeight );
            Tri_W = tri_w;
            Tri_H = tri_h;
            DoLightMap ( );
        }

        public void DoLightMap ( )
        {
            ResultGraph.Root = LightNode ( ( GraphEntity3D ) Graph.Root );

            Texture.VTex2D lm_tex = FinalMap.GetMap ( );

            void E_SetTex ( GraphNode3D node )
            {
                GraphEntity3D ge = node as GraphEntity3D;
                foreach ( Data.VMesh m in ge.Meshes )
                {
                    m.Mat.TCol = lm_tex;
                }
                ge.Renderer = new Visuals.VRLightMap ( );
            }

            ResultGraph.EditGraph ( E_SetTex );

            // ResultGraph.Root.SetMultiPass ( );
        }

        public GraphEntity3D LightNode ( GraphEntity3D node )
        {
            GraphEntity3D res = new GraphEntity3D
            {
                Name = node.Name + "(Lightmapped)"
            };

            foreach ( Data.VMesh mesh in node.Meshes )
            {
                Data.VMesh lit_msh = LightMesh ( mesh,node.World );
                res.Meshes.Add ( lit_msh );
            }

            foreach ( GraphNode3D subnode in node.Sub )
            {
                res.Add ( LightNode ( ( GraphEntity3D ) subnode ) );
            }

            return res;
        }

        public Data.VMesh LightMesh ( Data.VMesh mesh, OpenTK.Matrix4 world_mat )
        {
            Data.VMesh res_msh = new Data.VMesh ( mesh.TriData.Length*3, mesh.VertexData.Length );

            Data.Vertex [ ] verts = mesh.VertexData;
            Data.Tri [ ] tris = mesh.TriData;

            int vi=0;
            foreach ( Data.Vertex ov in mesh.VertexData )
            {
                res_msh.SetVertex ( vi, ov.Pos, ov.Tan, ov.BiNorm, ov.Norm, ov.UV );

                vi++;
            }

            res_msh.Transform ( world_mat );

            int ti=0;
            foreach ( Data.Tri tri in tris )
            {
                res_msh.SetTri ( ti, tri.V0, tri.V1, tri.v2 );
                ti++;
                MapVertex lvert = new MapVertex();
                lvert.Verts [ 0 ] = verts [ tri.V0 ];
                lvert.Verts [ 1 ] = verts [ tri.V1 ];
                lvert.Verts [ 2 ] = verts [ tri.v2 ];

                MapVertex dvert = new MapVertex();
                dvert.Verts [ 0 ] = verts [ tri.V0 ];
                dvert.Verts [ 1 ] = verts [ tri.V1 ];
                dvert.Verts [ 2 ] = verts [ tri.v2 ];

                OpenTK.Vector3 tri_norm = verts [ tri.V0 ].Norm;

                tri_norm.Normalize ( );

                OpenTK.Vector3 pointonplane = lvert.Verts [ 0 ].Pos;

                int flag=-1;

                if ( Math.Abs ( tri_norm.X ) > Math.Abs ( tri_norm.Y ) &&
                    Math.Abs ( tri_norm.X ) > Math.Abs ( tri_norm.Z ) )
                {
                    flag = 1;
                    lvert.Verts [ 0 ].UV.X = dvert.Verts [ 0 ].Pos.Y;
                    lvert.Verts [ 0 ].UV.Y = dvert.Verts [ 0 ].Pos.Z;

                    lvert.Verts [ 1 ].UV.X = dvert.Verts [ 1 ].Pos.Y;
                    lvert.Verts [ 1 ].UV.Y = dvert.Verts [ 1 ].Pos.Z;

                    lvert.Verts [ 2 ].UV.X = dvert.Verts [ 2 ].Pos.Y;
                    lvert.Verts [ 2 ].UV.Y = dvert.Verts [ 2 ].Pos.Z;
                }
                else if ( Math.Abs ( tri_norm.Y ) > Math.Abs ( tri_norm.X ) &&
                    Math.Abs ( tri_norm.Y ) > Math.Abs ( tri_norm.Z ) )
                {
                    flag = 2;
                    lvert.Verts [ 0 ].UV.X = dvert.Verts [ 0 ].Pos.X;
                    lvert.Verts [ 0 ].UV.Y = dvert.Verts [ 0 ].Pos.Z;

                    lvert.Verts [ 1 ].UV.X = dvert.Verts [ 1 ].Pos.X;
                    lvert.Verts [ 1 ].UV.Y = dvert.Verts [ 1 ].Pos.Z;

                    lvert.Verts [ 2 ].UV.X = dvert.Verts [ 2 ].Pos.X;
                    lvert.Verts [ 2 ].UV.Y = dvert.Verts [ 2 ].Pos.Z;
                }
                else
                {
                    flag = 3;
                    lvert.Verts [ 0 ].UV.X = dvert.Verts [ 0 ].Pos.X;
                    lvert.Verts [ 0 ].UV.Y = dvert.Verts [ 0 ].Pos.Y;

                    lvert.Verts [ 1 ].UV.X = dvert.Verts [ 1 ].Pos.X;
                    lvert.Verts [ 1 ].UV.Y = dvert.Verts [ 1 ].Pos.Y;

                    lvert.Verts [ 2 ].UV.X = dvert.Verts [ 2 ].Pos.X;
                    lvert.Verts [ 2 ].UV.Y = dvert.Verts [ 2 ].Pos.Y;
                }

                float min_u = lvert.Verts[0].UV.X;
                float min_v = lvert.Verts[0].UV.Y;

                float max_u = lvert.Verts[0].UV.X;
                float max_v = lvert.Verts[0].UV.Y;

                for ( int i = 0; i < 3; i++ )
                {
                    if ( lvert.Verts [ i ].UV.X < min_u )
                    {
                        min_u = lvert.Verts [ i ].UV.X;
                    }
                    if ( lvert.Verts [ i ].UV.X < min_v )
                    {
                        min_v = lvert.Verts [ i ].UV.Y;
                    }
                    if ( lvert.Verts [ i ].UV.X > max_u )
                    {
                        max_u = lvert.Verts [ i ].UV.X;
                    }
                    if ( lvert.Verts [ i ].UV.Y > max_v )
                    {
                        max_v = lvert.Verts [ i ].UV.Y;
                    }
                }

                float delta_u = max_u-min_u;
                float delta_v = max_v-min_v;

                for ( int i = 0; i < 3; i++ )
                {
                    lvert.Verts [ i ].UV.X = lvert.Verts [ i ].UV.X - min_u;
                    lvert.Verts [ i ].UV.Y = lvert.Verts [ i ].UV.Y - min_v;
                    lvert.Verts [ i ].UV.X = lvert.Verts [ i ].UV.X / delta_u;
                    lvert.Verts [ i ].UV.Y = lvert.Verts [ i ].UV.Y / delta_v;
                }

                float dist = (tri_norm.X * pointonplane.X + tri_norm.Y * pointonplane.Y + tri_norm.Z * pointonplane.Z);

                float X,Y,Z;

                OpenTK.Vector3 UVVector = OpenTK.Vector3.Zero;

                OpenTK.Vector3 vec1=OpenTK.Vector3.Zero,vec2=OpenTK.Vector3.Zero;

                switch ( flag )
                {
                    case 1:
                        X = -( tri_norm.Y * min_u + tri_norm.Z * min_v + dist ) / tri_norm.X;
                        UVVector.X = X;
                        UVVector.Y = min_u;
                        UVVector.Z = min_v;
                        X = -( tri_norm.Y * max_u + tri_norm.Z * min_v + dist ) / tri_norm.X;
                        vec1.X = X;
                        vec1.Y = max_u;
                        vec1.Z = min_v;
                        X = -( tri_norm.Y * min_u + tri_norm.Z * max_v + dist ) / tri_norm.X;
                        vec2.X = X;
                        vec2.Y = min_u;
                        vec2.Z = max_v;
                        break;

                    case 2:
                        Y = -( tri_norm.X * min_u + tri_norm.Z * min_v + dist ) / tri_norm.Y;
                        UVVector.X = min_u;
                        UVVector.Y = Y;
                        UVVector.Z = min_v;
                        Y = -( tri_norm.X * max_u + tri_norm.Z * min_v + dist ) / tri_norm.Y;
                        vec1.X = max_u;
                        vec1.Y = Y;
                        vec1.Z = min_v;
                        Y = -( tri_norm.X * min_u + tri_norm.Z * max_v + dist ) / tri_norm.Y;
                        vec2.X = min_u;
                        vec2.Y = Y;
                        vec2.Z = max_v;
                        break;

                    case 3:

                        Z = -( tri_norm.X * min_u + tri_norm.Y * min_v + dist ) / tri_norm.Z;
                        UVVector.X = min_u;
                        UVVector.Y = min_v;
                        UVVector.Z = Z;
                        Z = -( tri_norm.X * max_u + tri_norm.Y * min_v + dist ) / tri_norm.Z;
                        vec1.X = max_u;
                        vec1.Y = min_v;
                        vec1.Z = Z;
                        Z = -( tri_norm.X * min_u + tri_norm.Y * max_v + dist ) / tri_norm.Z;
                        vec2.X = min_u;
                        vec2.Y = max_v;
                        vec2.Z = Z;

                        break;
                }

                OpenTK.Vector3 edge1;

                edge1.X = vec1.X - UVVector.X;
                edge1.Y = vec1.Y - UVVector.Y;
                edge1.Z = vec1.Z - UVVector.Z;

                OpenTK.Vector3 edge2;

                edge2.X = vec2.X - UVVector.X;
                edge2.Y = vec2.Y - UVVector.Y;
                edge2.Z = vec2.Z - UVVector.Z;

                Console.WriteLine ( "Grabbing TexLeaf" );
                TreeLeaf tex_node = FinalMap.Insert(Tri_W,Tri_H);

                byte[] rgb = new byte[Tri_W*Tri_H*3];

                OpenTK.Vector3[,] lumels = new OpenTK.Vector3 [ Tri_W, Tri_H ];

                for ( int iX = 0; iX < Tri_W; iX++ )
                {
                    for ( int iY = 0; iY < Tri_H; iY++ )
                    {
                        float ufactor = (iX/(float)Tri_W);
                        float vfactor = (iY/(float)Tri_H);

                        OpenTK.Vector3 newedge1;
                        newedge1.X = edge1.X * ufactor;
                        newedge1.Y = edge1.Y * ufactor;
                        newedge1.Z = edge1.Z * ufactor;

                        OpenTK.Vector3 newedge2;

                        newedge2.X = edge2.X * vfactor;
                        newedge2.Y = edge2.Y * vfactor;
                        newedge2.Z = edge2.Z * vfactor;

                        int rloc = (iY * Tri_W * 3) + iX * 3;

                        lumels [ iX, iY ].X = UVVector.X + newedge2.X + newedge1.X;
                        lumels [ iX, iY ].Y = UVVector.Y + newedge2.Y + newedge1.Y;
                        lumels [ iX, iY ].Z = UVVector.Z + newedge2.Z + newedge1.Z;

                        rgb [ rloc ] = 255;
                        rgb [ rloc + 1 ] = 255;
                        rgb [ rloc + 2 ] = 255;
                    }
                }

                for ( int cv = 0; cv < 3; cv++ )
                {
                    float lx = ((tex_node.RC.X+1)+(tex_node.RC.W-2)* lvert.Verts[cv].UV.X)/tex_node.Root.RC.W;
                    float ly = ((tex_node.RC.Y+1)+(tex_node.RC.H-2)* lvert.Verts[cv].UV.Y)/tex_node.Root.RC.H;

                    res_msh.VertexData [ tri.V0 ].UV = new OpenTK.Vector2 ( lx, ly );
                }

                tex_node.SetRaw ( rgb );
            }
            res_msh.Mat = new Material.Material3D ( );

            res_msh.Final ( );
            return res_msh;
        }
    }

    public class MapVertex
    {
        public Data.Vertex[] Verts = new Data.Vertex[3];
        public int Plane=1;
        public Util.Texture.TreeLeaf TexLeaf;
        public float EUX,EVX;
        public float EUY,EVY;
        public float EUZ,EVZ;
        public float OX,OY,OZ;
        public int CW,CH;
        public float MX,MY,MZ;
        public float CX,CY,CZ,EntX,EntY,EntZ;
        public int VI;
    }
}