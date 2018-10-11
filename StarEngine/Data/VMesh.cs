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
        public void Final()
        {
            Viz.SetMesh(this);
            Viz.Final();
        }
    }
}
