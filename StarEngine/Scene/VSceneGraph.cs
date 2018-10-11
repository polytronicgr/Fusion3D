using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Lighting;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace Vivid3D.Scene
{
    public class SceneGraph3D
    {
        public List<GraphNode3D> Nodes = new List<GraphNode3D>();
        public List<GraphCam3D> Cams = new List<GraphCam3D>();
        public List<GraphLight3D> Lights = new List<GraphLight3D>();
        public GraphCam3D CamOverride = null;
        public GraphNode3D Root = new GraphNode3D();
        public void BeginFrame()
        {
            BeginFrameNode(Root);
            foreach(var c in Cams)
            {
                c.StartFrame();
            }
        }
        public void BeginFrameNode(GraphNode3D node)
        {
            node.StartFrame();
            foreach(var snode in node.Sub)
            {
                BeginFrameNode(snode);
            }
        }
        public virtual void Add(GraphCam3D c)
        {
            Cams.Add(c);
        }
        public virtual void Add(GraphLight3D l)
        {
            Lights.Add(l);
        }
        public virtual void Add(GraphNode3D n)
        {
            Root.Sub.Add(n);
            n.Top = Root;
        }
        public virtual void Clean()
        {
            Nodes.Clear();
        }
        public virtual void Bind()
        {

        }
        public virtual void Release()
        {

        }
        public virtual void RenderDepth()
        {
            GL.ClearColor(new OpenTK.Graphics.Color4(1.0f, 1.0f, 1.0f, 1.0f));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (CamOverride != null)
            {
                //foreach (var n in Nodes)
                //{
                  RenderNodeDepth(Root,CamOverride);
                    
                //}
            } else
                foreach (var c in Cams)
                {

                    //foreach (var n in Nodes)
                   // {
                      //  n.PresentDepth(c);
                    //}
                }
        }
        public virtual void RenderShadows()
        {
            int ls = 0;
            GL.Disable(EnableCap.Blend);
            foreach (var l in Lights)
            {
                ls++;
                l.DrawShadowMap(this);
                //    Console.WriteLine("LightShadows:" + ls);
            }
        }
        public virtual void Update()
        {
            UpdateNode(Root);
        }
        public virtual void UpdateNode(GraphNode3D node)
        {
            node.Update();
            foreach(var snode in node.Sub)
            {
                UpdateNode(snode);
            }
        }
        public virtual void RenderNodeDepth(GraphNode3D node, GraphCam3D c)
        {

            if (node.CastDepth)
            {
                node.PresentDepth(c);
            }
            foreach(var snode in node.Sub)
            {
                RenderNodeDepth(snode, c);
            }
        }
        public virtual void RenderNode(GraphNode3D node)
        {

            if (CamOverride != null)
            {
                foreach (var l in Lights)
                {
                    GraphLight3D.Active = l;
                               
                    node.Present(CamOverride);
                    
                }
            }
            else
            {


                foreach (var c in Cams)
                {
                    if (node.Lit)
                    {
                        bool first = true;
                        foreach (var l in Lights)
                        {
                            GraphLight3D.Active = l;
                            if (first)
                            {
                                first = false;
                                GL.Disable(EnableCap.Blend);
                            }
                            else
                            {
                                GL.Enable(EnableCap.Blend);
                                GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
                            }
                           // Console.WriteLine("Presenting:" + node.Name);
                            node.Present(c);

                            //                        foreach (var n in Nodes)
                            //                      {
                            //                        n.Present(c);
                            //                  }

                        }
                    }
                    else
                    {
                        if (node.FaceCamera)
                        {
                            node.LookAt(c.LocalPos, new Vector3(0, 1, 0));
                            
                        }
                        GL.Enable(EnableCap.Blend);
                        GL.BlendFunc(BlendingFactor.Src1Alpha, BlendingFactor.OneMinusSrcAlpha);
                        GL.DepthMask(false);
                        node.Present(c);
                        GL.DepthMask(true);
                    }
                }
            }
            foreach(var snode in node.Sub)
            {
             //   Console.WriteLine("Rendering Node:" + snode.Name);
                RenderNode(snode);
            }

        }
        public virtual void RenderNodeNoLights(GraphNode3D node)
        {

            if (CamOverride != null)
            {
                foreach (var l in Lights)
                {
                    GraphLight3D.Active = l;

                    node.Present(CamOverride);

                }
            }
            else
            {


                foreach (var c in Cams)
                {



                    GL.Disable(EnableCap.Blend);


                    // Console.WriteLine("Presenting:" + node.Name);
                    node.Present(c);

                    //                        foreach (var n in Nodes)
                    //                      {
                    //                        n.Present(c);
                    //                  }


                }
            }
            foreach (var snode in node.Sub)
            {
                //   Console.WriteLine("Rendering Node:" + snode.Name);
                RenderNodeNoLights(snode);
            }

        }
        public virtual void RenderNoLights()
        {
            Lighting.GraphLight3D.Active = null;
            RenderNodeNoLights(Root);
        }
        public virtual void Render()
        {

            RenderNode(Root);
           

        }
    }
}
