using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Draw;
using Vivid3D.FXS;
using Vivid3D.Reflect;
using OpenTK;
using System.IO;
namespace Vivid3D.Scene
{
    public class SceneGraph
    {

        public ClassIO ClassCopy
        {
            get;
            set;
        }

        public bool Running
        {
            get;
            set;
        }

        public FXLitImage LitImage
        {
            get;
            set;
        }

        public float X
        {
            get;
            set;
        }
        public float Y
        {
            get;
            set;
        }
        public float Z
        {
            get;
            set;
        }
        public float Rot
        {
            get;
            set;
        }

        public GraphNode Root
        {
            get;
            set;
        }

        public List<GraphLight> Lights
        {
            get;
            set;
        }

        public SceneGraph()
        {
            Running = false;
            X = 0;
            Y = 0;
            Z = 1;
            Rot = 0;
            Root = new GraphNode();
            Lights = new List<GraphLight>();
            LitImage = new FXLitImage();

        }

        public void Copy()
        {

            ClassCopy = new Reflect.ClassIO(this);
            ClassCopy.Copy();
            CopyNode(Root);
        }

        public void CopyNode(GraphNode node)
        {

            node.CopyProps();
            foreach(var nn in node.Nodes)
            {
                CopyNode(nn);
            }

        }

        public void Restore()
        {
            ClassCopy.Reset();
            RestoreNode(Root);
        }

        public void RestoreNode(GraphNode node)
        {
            node.RestoreProps();
            foreach(var nn in node.Nodes)
            {
                RestoreNode(nn);
            }
        }

        public void Add(GraphNode node)
        {
            node.Graph = this;
            Root.Nodes.Add(node);
            node.Root = Root;
        }

        public void Add(params GraphNode[] nodes)
        {

            foreach(var node in nodes)
            {

                node.Graph = this;
                Root.Nodes.Add(node);
                node.Root = Root;

            }

        }


        public void Add(GraphLight node, bool toGraph = false)
        {
            if (toGraph)
            {
                Root.Nodes.Add(node);
            }
            node.Graph = this;
            Lights.Add(node);
            
        }

        public void Add(params GraphLight[] lights)
        {
            foreach(var light in lights)
            {
                light.Graph = this;
                Lights.Add(light);
            }
        }

        public void Translate(float x,float y)
        {

            X = X + x;
            Y = Y + y;

        }

        public void Move(float x,float y)
        {

            var r = Util.Maths.Rotate(-x, -y, (180.0f-Rot), 1.0f);

            X = X + r.X;
            Y = Y + r.Y;


        }

        public void Update()
        {

            if (Running)
            {
                UpdateNode(Root);
            }
            else
            {

            }

        }

        public void UpdateNode(GraphNode node)
        {

            node.Update();

            foreach(var sub in node.Nodes)
            {
                UpdateNode(sub);
            }

        }
        
        public void DrawNode(GraphNode node)
        {
            //if(node.ImgFrame == null) 

            if (node.ImgFrame != null)
            {

                if(node.ImgFrame.Width < 2)
                {
                    Console.WriteLine("Illegal Image ID:" + node.ImgFrame.ID);
                    while (true)
                    {

                    }
                }

                bool first = true;

                foreach (var light in Lights)
                {
                    if (node.ImgFrame == null) continue;
                    LitImage.Graph = this;
                    LitImage.Light = light;

                    if (first)
                    {
                        Render.SetBlend(BlendMode.Alpha);
                        first = false;
                    }
                    else
                    {
                        Render.SetBlend(BlendMode.Add);
                    }

                    LitImage.Bind();


                    float[] xc;
                    float[] yc;

                    node.SyncCoords();

                    xc = node.XC;
                    yc = node.YC;

                    Render.Image(node.DrawP, node.ImgFrame);



                    //Render.Image(xc, yc, node.ImgFrame);



                    LitImage.Release();

                }
            }
            foreach(var snode in node.Nodes)
            {
                DrawNode(snode);
            }
        }
        public void Draw()
        {
            DrawNode(Root);
        }
        float sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            bool b1, b2, b3;

            b1 = sign(pt, v1, v2) < 0.0f;
            b2 = sign(pt, v2, v3) < 0.0f;
            b3 = sign(pt, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }
        public GraphNode PickNode(GraphNode node,int x,int y)
        {
            foreach (var n in node.Nodes)
            {
                var p = PickNode(n,x,y);
                if (p != null) return p;
            }
            if (node.DrawP != null)
            {
                if (PointInTriangle(new Vector2(x, y), node.DrawP[0], node.DrawP[1], node.DrawP[2]))
                {
                    return node;
                }
                else if (PointInTriangle(new Vector2(x, y), node.DrawP[2], node.DrawP[3], node.DrawP[0]))
                {
                    return node;
                }
            }
            return null;

        }
        public GraphNode Pick(int x,int y)
        {
            return PickNode(Root,x,y);
        }

        public Vector2 GetPoint(float x,float y)
        {
            int w, h;
            w = App.VividApp.W;
            h = App.VividApp.H;
            Vector2 r = new Vector2(x, y);
            r = Util.Maths.Push(r, -w / 2, -h / 2);
            r = Util.Maths.Rotate(r.X, r.Y, Rot, 1);
            r.X = r.X + X;
            r.Y = r.Y + Y;
            return r;
        }
        public void Load(string path)
        {

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            ReadGraph(r);
            Root.Read(r);
            fs.Close();
            r = null;
            fs = null;
        }

        public void ReadGraph(BinaryReader r)
        {
            X = r.ReadSingle();
            Y = r.ReadSingle();
            Z = r.ReadSingle();
            Rot = r.ReadSingle();
            int lc = r.ReadInt32();
            for (int i = 0; i < lc; i++)
            {
                var nl = new GraphLight();
                nl.Read(r);
                Add(nl);
            }
            Root = new GraphNode();
            Root.Graph = this;
            Root.Read(r);
        }

        public void Save(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter w = new BinaryWriter(fs);
            WriteGraph(w);
            fs.Flush();
            fs.Close();
            w = null;
            fs = null;
        }
       
        public  void WriteGraph(BinaryWriter w)
        {
            w.Write(X);
            w.Write(Y);
            w.Write(Z);
            w.Write(Rot);
            w.Write(Lights.Count());
            foreach (var l in Lights)
            {
                l.Write(w);
            }

            Root.Write(w);
        }
    }
}
