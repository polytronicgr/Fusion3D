using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Vivid3D.Material;
using Vivid3D.Visuals;
namespace Vivid3D.Data
{
    public class VMesh
    {
        public VVisualizer Viz = null;
        public string Name = "NoName";
        public VVertex3D Data = null;
        public float[] Vertices = null;
        public float[] UV = null;
        public float[] Norm = null;
        public float[] Tan = null;
        public float[] Bi = null;
        public int NumVerts = 0;
        public uint[] Indices = null;
        public Material3D Mat = null;
        public int NumVertices
        {
            get
            {
                return NumVerts;
            }
        }
        public int NumIndices
        {
            get
            {
                return Indices.Length;
            }
        }
        public void Scale(float x,float y,float z)
        {
            for(int i = 0; i < NumVertices; i++)
            {
                int vid = i * 3;
                Vertices[vid] *= x;
                Vertices[vid + 1] *= y;
                Vertices[vid + 2] *= z;
            }
        }
        public VMesh()
        {

        }

        public void Write()
        {
            Help.IOHelp.WriteInt(NumVertices);
            Help.IOHelp.WriteInt(NumIndices);
            for(int i = 0; i<Vertices.Length; i++)
            {
                Help.IOHelp.WriteFloat(Vertices[i]);
                Help.IOHelp.WriteFloat(Norm[i]);
               
                Help.IOHelp.WriteFloat(Bi[i]);
                Help.IOHelp.WriteFloat(Tan[i]);
            }
            for(int i = 0;i< UV.Length; i++)
            {
                Help.IOHelp.WriteFloat(UV[i]);
            }
            for(int i = 0; i < Indices.Length; i++)
            {
                Help.IOHelp.WriteInt((int)Indices[i]);
            }
            Mat.Write();
        }
        public void Read()
        {
            int nv = Help.IOHelp.ReadInt();
            int ni = Help.IOHelp.ReadInt();
            NumVerts = nv;
      
            Vertices = new float[nv * 3];
            Norm = new float[nv * 3];
            Bi = new float[nv * 3];
            Tan = new float[nv * 3];
            Indices = new uint[ni];
            for (int i = 0; i < nv*3; i++)
            {
                Vertices[i] = Help.IOHelp.ReadFloat();
                Norm[i] = Help.IOHelp.ReadFloat();
                Bi[i] = Help.IOHelp.ReadFloat();
                Tan[i] = Help.IOHelp.ReadFloat();
            }
        
            UV = new float[nv * 2];
            for(int i = 0; i < nv * 2; i++)
            {
                UV[i] = Help.IOHelp.ReadFloat();
            }
            for(int i = 0; i < ni; i++)
            {
                Indices[i] = (uint)Help.IOHelp.ReadInt();
            }
            Viz = new VVVBO(nv, ni);
            Final();
            Mat = new Material3D();
            Mat.Read();
        }
        public VMesh(int indices,int vertices)
        {
            //Data = new VVertex3D(vertices);
            Indices = new uint[indices];
            Vertices = new float[vertices * 3];
            Norm = new float[vertices * 3];
            UV = new float[vertices * 2];
            Bi = new float[vertices * 3];
            Tan = new float[vertices * 3];
            Viz = new VVVBO(vertices, indices);
            NumVerts = vertices;
        }
        public void SetVertex(int id,Vector3 pos,Vector3 t,Vector3 b,Vector3 n,Vector2 uv)
        {
            int uid = id * 2;
            id = id * 3;
            Vertices[id] = pos.X;
            Vertices[id + 1] = pos.Y;
            Vertices[id + 2] = pos.Z;
            Norm[id] = n.X;
            Norm[id + 1] = n.Y;
            Norm[id + 2] = n.Z;
            Tan[id] = t.X;
            Tan[id + 1] = t.Y;
            Tan[id + 2] = t.Z;
            Bi[id] = b.X;
            Bi[id + 1] = b.Y;
            Bi[id + 2] = b.Z;
            UV[uid] = uv.X;
            UV[uid+1] = uv.Y;
            //uv.Y = 1 - uv.Y;
           
            //Data.SetVertex(id, pos, t, b, n, uv);
        }
        public void SetIndex(int id,uint vertex)
        {
            Indices[id] = vertex;
        }
        public void Clean()
        {
        //    Data = new VVertex3D(Data.Vertices);
        }
        public float[] GetVerts()
        {
            return Vertices;
        }
        public float[] GetNorms()
        {
            return Norm;
        }
        public float[] GetUV()
        {
            return UV;
        }
        public uint[] GetInds()
        {
            return Indices;
        }
        public int[] TriVert(int id)
        {
            int[] i = new int[3];
            i[0] = (int)Indices[id * 3];
            i[1] = (int)Indices[id * 3 + 1];
            i[2] =(int)Indices[id * 3 + 2];
            return i;
        }
        public Vector3 TriPos(int id)
        {
            var ii = TriVert(id);
            return new Vector3(Vertices[ii[0]], Vertices[ii[1]], Vertices[ii[2]]);
        }


        public Vector3 GetPos(int id)
        {
            id = (int)Indices[id];
            return new Vector3(Vertices[id * 3], Vertices[id * 3 + 1], Vertices[id * 3 + 2]);
        }
        public Vector3 GetUV(int id)
        {
            id = (int)Indices[id];
            return new Vector3(UV[id * 2], UV[id * 2 + 1], 0);
        }
        public Vector3 GetNorm(int id)
        {
            id = (int)Indices[id];
            return new Vector3(Norm[id * 3], Norm[id * 3 + 1], Norm[id * 3 + 2]);
        }
        public void SetNorm(int id, Vector3 n, Vector3 bi, Vector3 tan)
        {
            id = (int)Indices[id];
            Norm[id * 3] = n.X;
            Norm[id * 3 + 1] = n.Y;
            Norm[id * 3 + 2] = n.Z;

            Bi[id * 3] = bi.X;
            Bi[id * 3 + 1] = bi.Y;
            Bi[id * 3 + 2] = bi.Z;

            Tan[id * 3] = tan.X;
            Tan[id * 3 + 1] = tan.Y;
            Tan[id * 3 + 2] = tan.Z;
        }
        public void GenerateTangents()
        {
            // Vector3 v1, v2, v3;
            // Vector3 u1, u2, u3;
            for (int i = 0; i < Indices.Length; i+=3)
            {
                Vector3 v1, v2, v3;
                v1 = GetPos(i);
                v2 = GetPos(i+1);
                v3 = GetPos(i+2);

                Vector3 t1, t2, t3;

                t1 = GetUV(i);
                t2 = GetUV(i + 1);
                t3 = GetUV(i + 2);

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

                SetNorm(i, n, bi, tan);
                SetNorm(i + 1, n, bi, tan);
                SetNorm(i + 2, n, bi, tan);



            }

        }
        public void Final()
        {
            if (Viz == null)
            {
                Viz = new VVVBO(NumVerts, NumIndices);
            }
            Viz.SetMesh(this);
            Viz.Final();
        }
    }
}
