using OpenTK;
using System.Collections.Generic;
using Vivid3D.Material;
using Vivid3D.Visuals;

namespace Vivid3D.Data
{
    public struct Tri
    {
        public int V0, V1, v2;
    }

    public struct Vertex
    {
        public Vector3 BiNorm;
        public int[] BoneIndices;
        public Vector3 Norm;
        public Vector3 Pos;
        public Vector3 Tan;
        public Vector2 UV;
        public float Weight;
    }

    public class VMesh
    {
        public float[] Bi = null;

        public VVertex3D Data = null;

        public uint[] Indices = null;

        public Material3D Mat = null;

        public string Name = "NoName";

        public float[] Norm = null;

        public int NumVerts = 0;

        public List<Subset> Subs = new List<Subset>();

        public float[] Tan = null;

        public Tri[] TriData;

        public float[] UV = null;

        public Vertex[] VertexData;

        public float[] Vertices = null;

        public VVisualizer Viz = null;

        public VMesh ( )
        {
        }

        public VMesh ( int indices , int vertices )
        {
            //Data = new VVertex3D(vertices);
            Indices = new uint [ indices ];
            //Vertices = new float[vertices * 3];
            //Norm = new float[vertices * 3];
            //UV = new float[vertices * 2];
            //Bi = new float[vertices * 3];
            //Tan = new float[vertices * 3];
            Viz = new VVVBO ( vertices , indices );
            VertexData = new Vertex [ vertices ];
            TriData = new Tri [ indices / 3 ];

            NumVerts = vertices;
        }

        public int NumIndices => TriData.Length * 3;

        public int NumVertices => VertexData.Length;

        public void Clean ( )
        {
            //    Data = new VVertex3D(Data.Vertices);
        }

        public VMesh Clone ( )
        {
            VMesh cm = new VMesh
            {
                VertexData = new Vertex [ VertexData.Length ]
            };
            for ( int v = 0 ; v < VertexData.Length ; v++ )
            {
                cm.VertexData [ v ] = VertexData [ v ];
            }
            cm.TriData = new Tri [ TriData.Length ];
            for ( int t = 0 ; t < TriData.Length ; t++ )
            {
                cm.TriData [ t ] = TriData [ t ];
            }
            cm.NumVerts = NumVerts;
            cm.Mat = Mat;
            return cm;
        }

        public void Final ( )
        {
            if ( Viz == null )
            {
                Viz = new VVVBO ( NumVerts , NumIndices );
            }
            Viz.SetMesh ( this );
            Viz.Final ( );
        }

        public void FinalAnim ( )
        {
            Viz = new VVVBO ( VertexData.Length , TriData.Length * 3 );
            Viz.SetMesh ( this );
            Viz.FinalAnim ( );
        }

        public void GenerateTangents ( )
        {
            for ( int t = 0 ; t < TriData.Length ; t++ )
            {
                Vector3 v1, v2, v3;
                v1 = VertexData [ TriData [ t ].V0 ].Pos;
                v2 = VertexData [ TriData [ t ].V1 ].Pos;

                v3 = VertexData [ TriData [ t ].v2 ].Pos;

                Vector3 t1, t2, t3;

                t1 = new Vector3 ( VertexData [ TriData [ t ].V0 ].UV );
                t2 = new Vector3 ( VertexData [ TriData [ t ].V1 ].UV );
                t3 = new Vector3 ( VertexData [ TriData [ t ].v2 ].UV );

                Vector3 v2v1 = v2 - v1;
                Vector3 v3v1 = v3 - v1;

                float c1 = t2.X - t1.X;
                float c2 = t2.Y - t1.Y;

                float c3 = t3.X - t1.X;
                float c4 = t3.Y - t1.Y;

                Vector3 n = GetNorm(t);

                Vector3 tan = new Vector3(c3 * v2v1.X - c2 * v3v1.X, c3 * v2v1.Y - c2 * v3v1.Y, c3 * v2v1.Z - c2 * v3v1.Z);
                Vector3 bi = Vector3.Cross(n, tan).Normalized();
                Vector3 st = Vector3.Cross(bi, n).Normalized();

                SetNorm ( t , n , bi , tan );
            }
            return;
            // Vector3 v1, v2, v3;
            // Vector3 u1, u2, u3;
            for ( int i = 0 ; i < Indices.Length ; i += 3 )
            {
                Vector3 v1, v2, v3;
                v1 = GetPos ( i );
                v2 = GetPos ( i + 1 );
                v3 = GetPos ( i + 2 );

                Vector3 t1, t2, t3;

                t1 = GetUV ( i );
                t2 = GetUV ( i + 1 );
                t3 = GetUV ( i + 2 );

                Vector3 v2v1 = v2 - v1;
                Vector3 v3v1 = v3 - v1;

                float c1 = t2.X - t1.X;
                float c2 = t2.Y - t1.Y;

                float c3 = t3.X - t1.X;
                float c4 = t3.Y - t1.Y;

                Vector3 n = GetNorm(i);

                Vector3 tan = new Vector3(c3 * v2v1.X - c2 * v3v1.X, c3 * v2v1.Y - c2 * v3v1.Y, c3 * v2v1.Z - c2 * v3v1.Z);
                Vector3 bi = Vector3.Cross(n, tan).Normalized();
                Vector3 st = Vector3.Cross(bi, n).Normalized();

                SetNorm ( i , n , bi , tan );
                SetNorm ( i + 1 , n , bi , tan );
                SetNorm ( i + 2 , n , bi , tan );
            }
        }

        public uint [ ] GetInds ( )
        {
            return Indices;
        }

        public Vector3 GetNorm ( int id )
        {
            return VertexData [ TriData [ id ].V0 ].Norm;

            id = ( int ) Indices [ id ];
            return new Vector3 ( Norm [ id * 3 ] , Norm [ id * 3 + 1 ] , Norm [ id * 3 + 2 ] );
        }

        public float [ ] GetNorms ( )
        {
            return Norm;
        }

        public Vector3 GetPos ( int id )
        {
            id = ( int ) Indices [ id ];
            return new Vector3 ( Vertices [ id * 3 ] , Vertices [ id * 3 + 1 ] , Vertices [ id * 3 + 2 ] );
        }

        public float [ ] GetUV ( )
        {
            return UV;
        }

        public Vector3 GetUV ( int id )
        {
            id = ( int ) Indices [ id ];
            return new Vector3 ( UV [ id * 2 ] , UV [ id * 2 + 1 ] , 0 );
        }

        public float [ ] GetVerts ( )
        {
            return Vertices;
        }

        public void Read ( )
        {
            int nv = Help.IOHelp.ReadInt();
            int ni = Help.IOHelp.ReadInt();
            NumVerts = nv;

            VertexData = new Vertex [ nv ];
            TriData = new Tri [ ni ];

            for ( int v = 0 ; v < nv ; v++ )
            {
                Vertex vert = new Vertex
                {
                    Pos = Help.IOHelp.ReadVec3 ( ) ,
                    Norm = Help.IOHelp.ReadVec3 ( ) ,
                    BiNorm = Help.IOHelp.ReadVec3 ( ) ,
                    Tan = Help.IOHelp.ReadVec3 ( )
                };
                Vector3 uv =  Help.IOHelp.ReadVec3 ( );
                vert.UV = new Vector2 ( uv.X , uv.Y );
                vert.Weight = Help.IOHelp.ReadFloat ( );
                VertexData [ v ] = vert;
            }

            for ( int t = 0 ; t < ni ; t++ )
            {
                Tri tri = new Tri
                {
                    V0 = Help.IOHelp.ReadInt ( ) ,
                    V1 = Help.IOHelp.ReadInt ( ) ,
                    v2 = Help.IOHelp.ReadInt ( )
                };
                TriData [ t ] = tri;
            }

            Viz = new VVVBO ( nv , ni * 3 );
            Final ( );
            Mat = new Material3D ( );
            Mat.Read ( );
        }

        public void Scale ( float x , float y , float z )
        {
            return;
            for ( int i = 0 ; i < NumVertices ; i++ )
            {
                int vid = i * 3;
                Vertices [ vid ] *= x;
                Vertices [ vid + 1 ] *= y;
                Vertices [ vid + 2 ] *= z;
            }
        }

        public void SetIndex ( int id , uint vertex )
        {
            //    Indices[id] = vertex;
        }

        public void SetNorm ( int id , Vector3 n , Vector3 bi , Vector3 tan )
        {
            Vertex n1 = VertexData[TriData[id].V0];
            Vertex n2 = VertexData[TriData[id].V1];
            Vertex n3 = VertexData[TriData[id].v2];

            n1.Norm = n;
            n2.Norm = n;
            n3.Norm = n;

            n1.BiNorm = bi;
            n2.BiNorm = bi;
            n3.BiNorm = bi;

            n1.Tan = tan;
            n2.Tan = tan;
            n3.Tan = tan;

            VertexData [ TriData [ id ].V0 ] = n1;
            VertexData [ TriData [ id ].V1 ] = n2;
            VertexData [ TriData [ id ].v2 ] = n3;
        }

        public void SetTri ( int id , int v0 , int v1 , int v2 )
        {
            TriData [ id ].V0 = v0;
            TriData [ id ].V1 = v1;
            TriData [ id ].v2 = v2;
        }

        public void SetVertex ( int id , Vector3 pos , Vector3 t , Vector3 b , Vector3 n , Vector2 uv )
        {
            VertexData [ id ].Pos = pos;
            VertexData [ id ].Norm = n;
            VertexData [ id ].Tan = t;
            VertexData [ id ].BiNorm = b;
            VertexData [ id ].UV = uv;
        }

        public void SetVertexBone ( int id , float weight , byte [ ] bones )
        {
            VertexData [ id ].Weight = weight;
            VertexData [ id ].BoneIndices = new int [ 4 ];
            if ( bones.Length > 0 )
            {
                VertexData [ id ].BoneIndices [ 0 ] = bones [ 0 ];
            }
            if ( bones.Length > 1 )
            {
                VertexData [ id ].BoneIndices [ 1 ] = bones [ 1 ];
            }
            if ( bones.Length > 2 )
            {
                VertexData [ id ].BoneIndices [ 2 ] = bones [ 2 ];
            }
            if ( bones.Length > 3 )
            {
                VertexData [ id ].BoneIndices [ 3 ] = bones [ 3 ];
            }
            //    SetVertexBones(id, bones[0], bones[1], bones[2], bones[3]);
        }

        public void SetVertexBones ( int id , int b0 , int b1 , int b2 , int b3 )
        {
            VertexData [ id ].BoneIndices = new int [ 4 ];
            VertexData [ id ].BoneIndices [ 0 ] = b0;
            VertexData [ id ].BoneIndices [ 1 ] = b1;
            VertexData [ id ].BoneIndices [ 2 ] = b2;
            VertexData [ id ].BoneIndices [ 3 ] = b3;
        }

        public Vector3 TriPos ( int id )
        {
            int [ ] ii = TriVert(id);
            return new Vector3 ( Vertices [ ii [ 0 ] ] , Vertices [ ii [ 1 ] ] , Vertices [ ii [ 2 ] ] );
        }

        public int [ ] TriVert ( int id )
        {
            int[] i = new int[3];
            i [ 0 ] = ( int ) Indices [ id * 3 ];
            i [ 1 ] = ( int ) Indices [ id * 3 + 1 ];
            i [ 2 ] = ( int ) Indices [ id * 3 + 2 ];
            return i;
        }

        public void Write ( )
        {
            Help.IOHelp.WriteInt ( VertexData.Length );
            Help.IOHelp.WriteInt ( TriData.Length );

            foreach ( Vertex v in VertexData )
            {
                Help.IOHelp.WriteVec ( v.Pos );
                Help.IOHelp.WriteVec ( v.Norm );
                Help.IOHelp.WriteVec ( v.BiNorm );
                Help.IOHelp.WriteVec ( v.Tan );
                Help.IOHelp.WriteVec ( new Vector3 ( v.UV ) );
                Help.IOHelp.WriteFloat ( v.Weight );
            }

            foreach ( Tri t in TriData )
            {
                Help.IOHelp.WriteInt ( t.V0 );
                Help.IOHelp.WriteInt ( t.V1 );
                Help.IOHelp.WriteInt ( t.v2 );
            }

            Mat.Write ( );
        }

        public class Subset
        {
            public int FaceCount;
            public int FaceStart;
            public int VertexCount;
            public int VertexStart;
        }
    }
}