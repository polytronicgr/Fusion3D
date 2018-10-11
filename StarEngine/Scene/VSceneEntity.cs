using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Data;
using Vivid3D.Visuals;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Vivid3D.Material;
namespace Vivid3D.Scene
{
    public class GraphEntity3D : GraphNode3D
    {
        public VRenderer Renderer = null;
        public List<VMesh> Meshes = new List<VMesh>();
        public Bounds Bounds
        {
            get
            {
                sw = sh = sd = 20000;
                bw = bh = bd = -20000;
                GetBounds(this);
                Bounds res = new Bounds();
                res.W = bw - sw;
                res.H = bh - sh;
                res.D = bd - sd;
                res.MinX = sw;
                res.MaxX = bw;
                res.MinY = sh;
                res.MaxY = bh;
                res.MinZ = sd;
                res.MaxZ = bd;
                return res;
            }
        }
        private float sw, sh, sd;
        private float bw, bh, bd;
        public void GetBounds(GraphEntity3D node)
        {
            foreach(var m in node.Meshes)
            {
                for(int i = 0; i < m.NumVertices; i++)
                {
                    int vid = i * 3;
                    if (m.Vertices[vid] < sw)
                    {
                        sw = m.Vertices[vid];
                    }
                    if (m.Vertices[vid] > bw)
                    {
                        bw = m.Vertices[vid];
                    }
                    if(m.Vertices[vid+1]<sh)
                    {
                        sh = m.Vertices[vid + 1];
                    }
                    if (m.Vertices[vid + 1] > bh)
                    {
                        bh = m.Vertices[vid + 1];
                    }
                    if (m.Vertices[vid + 2] < sd)
                    {
                        sd = m.Vertices[vid + 2];
                    }
                    if (m.Vertices[vid + 2] > bd)
                    {
                        bd = m.Vertices[vid + 2];
                    }
                }
            }
            foreach(var snode in node.Sub)
            {
                if (snode is GraphEntity3D)
                {
                    GetBounds(snode as GraphEntity3D);
                }
            }
        }
        public Physics.PyType PyType;
        public Physics.PyObject PO = null;
        public void EnablePy(Physics.PyType type)
        {
            PyType = type;
            switch (PyType)
            {
                case Physics.PyType.Box:
                    PO = new Physics.PyDynamic(type,this);
                    break;
                case Physics.PyType.Mesh:
                    PO = new Physics.PyStatic(this);
                    break;

            }
        }

        public List<Vector3> GetAllVerts
        {
            get
            {
                List<Vector3> verts = new List<Vector3>();
                GetVerts(verts, this);
                return verts;
            }

        }
        public void ScaleMeshes(float x,float y,float z)
        {
            DScale(x, y, z, this);
        }
        private void DScale(float x,float y,float z,GraphEntity3D node)
        {
            foreach(var m in node.Meshes)
            {
                m.Scale(x, y, z);
            }
            foreach(var snode in node.Sub)
            {
                DScale(x, y, z, snode as GraphEntity3D);
            }
        }
        public void GetVerts(List<Vector3> verts,GraphEntity3D node)
        {
            foreach(var m in node.Meshes)
            {
                for(int i = 0; i < m.NumVertices; i++)
                {
                    int vid = i * 3;
                    var nv = new Vector3(m.Vertices[vid], m.Vertices[vid + 1], m.Vertices[vid + 2]);
                    nv = Vector3.TransformPosition(nv, node.World);
                    verts.Add(nv);
                    // verts.Add(m.Vertices[vid]);
                  //  verts.Add(m.Vertices[vid + 1]);
                //    verts.Add(m.Vertices[vid + 2]);
                }

            }
            foreach(var snode in node.Sub)
            {
                GetVerts(verts, snode as GraphEntity3D);
            }
        }
        public override void Init()
        {
            Renderer = new VRMultiPass();
        }
        public void AddMesh(VMesh mesh)
        {
            Meshes.Add(mesh);
        }
        public void Clean()
        {
            Meshes = new List<VMesh>();
            Renderer = null;
        }
        public void SetMat(Material3D mat)
        {
            foreach(var m in Meshes)
            {
                m.Mat = mat;
            }
            foreach(var n in Sub)
            {
                if(n is GraphEntity3D)
                {
                    var ge = n as GraphEntity3D;
                    ge.SetMat(mat);
                }
            }
        }
        public override void PresentDepth(GraphCam3D c)
        {
            SetMats(c);
            Bind();
            PreRender();
            RenderDepth();
            PostRender();
            Release();
            foreach (var s in Sub)
            {
                s.PresentDepth(c);
            }
        }
        public override void Present(GraphCam3D c)
        {
            //  GL.MatrixMode(MatrixMode.Projection);
            // GL.LoadMatrix(ref c.ProjMat);
            SetMats(c);
            Bind();
            PreRender();
            Render();
            PostRender();
            Release();
      //      foreach (var s in Sub)
        //    {
          //      s.Present(c);
           // }
        }

        public void SetMats(GraphCam3D c)
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

        /// <summary>
        /// To be called AFTER data asscoiation.
        /// </summary>

        public virtual void Bind()
        {

        }
        public virtual void PreRender()
        {
          
        }
        public virtual void RenderDepth()
        {
            Effect.FXG.Ent = this;
            foreach (var m in Meshes)
            {
                Effect.FXG.Mesh = m;
                Renderer.RenderDepth(m);
            }
        }
        public virtual void Render()
        {
            Effect.FXG.Ent = this;
            foreach(var m in Meshes)
            {
                Effect.FXG.Mesh = m;
                Renderer.Render(m);
            }
          
        }
        public virtual void PostRender()
        {

        }
        public virtual void Release()
        {

        }
    }
}
