using System;
using Vivid3D.Scene;
using Vivid3D.Util.Texture;

namespace Vivid3D.Lighting.LightMapper
{
    public class LightMapper
    {
        public SceneGraph3D Graph { get; set; }
        public TexTree FinalMap { get; set; }

        public LightMapper ( int mapWidth, int mapHeight, SceneGraph3D graph )
        {
            Graph = graph;
            FinalMap = new TexTree ( mapWidth, mapHeight );
            DoLightMap ( );
        }

        public void DoLightMap ( )
        {
            LightNode ( ( GraphEntity3D ) Graph.Root );
        }

        public void LightNode ( GraphEntity3D node )
        {
            foreach ( Data.VMesh mesh in node.Meshes )
            {
                LightMesh ( mesh );
            }
        }

        public void LightMesh ( Data.VMesh mesh )
        {
            Data.Vertex [ ] verts = mesh.VertexData;
            Data.Tri [ ] tris = mesh.TriData;

            foreach ( Data.Tri tri in tris )
            {
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

                OpenTK.Vector3 UVVector;

                OpenTK.Vector3 vec1,vec2;

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
            }
        }
    }

    public class MapVertex
    {
        public Data.Vertex[] Verts = new Data.Vertex[3];
    }
}