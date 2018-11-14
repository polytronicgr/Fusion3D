using Fusion3D.Scene;
using Fusion3D.Util.Texture;

namespace Fusion3D.Lighting.LightMapper
{
    public class Omni
    {
        public SceneGraph3D Graph { get; set; }
        public SceneGraph3D ResultGraph { get; set; }
        public TexTree FinalMap { get; set; }

        public int Tri_W,Tri_H;

        public Omni ( int mapWidth, int mapHeight, SceneGraph3D graph, int tri_w = 16, int tri_h = 16 )
        {
            Graph = graph;
            ResultGraph = new SceneGraph3D ( );

            FinalMap = new TexTree ( mapWidth, mapHeight );
            Tri_W = tri_w;
            Tri_H = tri_h;
            DoLightMap ( );
        }

        public void DoLightMap ( )
        {
            ResultGraph.Root = LightNode ( ( Entity3D ) Graph.Root );
        }

        public Entity3D LightNode ( Entity3D node )
        {
            Entity3D res = new Entity3D
            {
                Name = node.Name + "(Lightmapped)"
            };

            if ( node.LightMapInfo.ReceiveLight )
            {
                foreach ( Data.Mesh3D mesh in node.Meshes )
                {
                    Data.Mesh3D lit_msh = LightMesh ( mesh );
                    res.Meshes.Add ( lit_msh );
                }
            }
            else
            {
                foreach ( Data.Mesh3D mesh in node.Meshes )
                {
                    res.Meshes.Add ( mesh.Clone ( ) );
                }
            }
            foreach ( Node3D subnode in node.Sub )
            {
                res.Add ( LightNode ( ( Entity3D ) subnode ) );
            }

            return res;
        }

        public Data.Mesh3D LightMesh ( Data.Mesh3D mesh )
        {
            Data.Mesh3D res_msh = new Data.Mesh3D ( mesh.TriData.Length, mesh.VertexData.Length );

            Data.Vertex [ ] verts = mesh.VertexData;
            Data.Tri [ ] tris = mesh.TriData;

            System.Collections.Generic.List<MapVertex> lVerts = new System.Collections.Generic.List<MapVertex>();
            int vi=0;
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

                if ( Abs ( tri_norm.X ) > Abs ( tri_norm.Y ) && Abs ( tri_norm.X ) > Abs ( tri_norm.Z ) )
                {
                    lvert.Plane = 1;
                }
                else
                {
                    if ( Abs ( tri_norm.Y ) > Abs ( tri_norm.X ) && Abs ( tri_norm.Y ) > Abs ( tri_norm.Z ) )
                    {
                        lvert.Plane = 2;
                    }
                    else
                    {
                        lvert.Plane = 3;
                    }
                }

                switch ( lvert.Plane )
                {
                    case 1:

                        break;
                }

                lvert.VI = vi;
                lVerts.Add ( lvert );
            }

            return res_msh;
        }

        public float Abs ( float v )
        {
            return System.Math.Abs ( v );
        }
    }
}