﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Data;
using OpenTK;
using Vivid3D.Script;
namespace Vivid3D.Scene
{
    public enum Space
    {
        Local,World
    }
    public class GraphNode3D
    {
        public bool On = true;
        public string Name = "";
        static int nn = 0;
        public ScriptBase Script { get; set; }


        public Vector3 LocalPos = Vector3.Zero;
        public Vector3 LocalScale = Vector3.One;
        public Matrix4 LocalTurn = Matrix4.Identity;
        public bool AlwaysAlpha = false;
        public GraphNode3D TopTop
        {
            get
            {
                if (Top != null)
                {
                    return Top.TopTop;
                }
                else
                {
                    return this;
                }
            }
        }
        public bool BreakTop = false;
        public List<GraphNode3D> TopList
        {
            get
            {
                List<GraphNode3D> tl = new List<GraphNode3D>();
                if (Top != null)
                {
                    Top.AddTop(tl);
                    return tl;
                }
                else
                {
                    return null;
                }
                return null;
            }
        }
        public void AddTop(List<GraphNode3D> l)
        {
            l.Add(this);
            if (BreakTop) return;
            if (Top != null)
            {
                Top.AddTop(l);
            }
        }
        public Vector3 WorldPos
        {
            get
            {
                var v = World;

                return v.ExtractTranslation();
            }
        }
        public Matrix4 World
        {
            get
            {
                Matrix4 r = Matrix4.Identity;
                if (Top != null)
                {
                    r = Top.World;
                }
                r = (Matrix4.CreateScale(LocalScale) * LocalTurn * Matrix4.CreateTranslation(LocalPos))*r;





       
                return r;
            }
        }
    public void SetMultiPass()
        {
            if (this is GraphEntity3D)
            {
                dynamic tn = this;
                tn.Renderer = new Visuals.VRMultiPass();
            }
            foreach(var n in Sub)
            {
                n.SetMultiPass();
            }
        }
        public GraphNode3D Top = null;
        public List<GraphNode3D> Sub = new List<GraphNode3D>();
        public VInfoMap<string, object> Links = new VInfoMap<string, object>();
        public bool Lit = true;
        public bool FaceCamera = false;
        public bool CastShadows = true;
        public bool CastDepth = true;
        public Matrix4 PrevWorld;
        public void AddProxy(GraphNode3D node)
        {
            Sub.Add(node);
        }
        public void Add(GraphNode3D node)
        {
            Sub.Add(node);
            node.Top = this;
        }
        public void StartFrame()
        {
            PrevWorld = World;
        }

        public GraphNode3D()
        {
            Init();
            Rot(new Vector3(0, 0, 0), Space.Local);
            Name = "Node:" + nn;
            nn++;
        }
        public virtual void Init()
        {

        }
        public virtual void Update()
        {

        }
        public virtual void AddLink(string name,object obj)
        {
            Links.Add(name, obj);
        }
        public void Pos(Vector3 p, Space s)
        {
            if (s == Space.Local)
            {
                LocalPos = p;
            }
        }
        public virtual void Rot(Vector3 r,Space s)
        {
            if (s == Space.Local)
            {
                LocalTurn = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(r.Y)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(r.X)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(r.Z));
            }
        }
        public virtual void Turn(Vector3 r,Space s)
        {
            Matrix4 t = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(r.Y)) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(r.X)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(r.Z));
            LocalTurn = LocalTurn * t;
        }
        // public void LookAt(Vector3 t)
        //{
        //   LocalTurn = Matrix4.LookAt(WorldPos, t, Vector3.UnitY);
        // }
     
        public void Move(Vector3 v,Space s)
        {
           // v.X = -v.X;
            if(s==Space.Local)
            {

                Console.WriteLine("NV:" + v);
                var ov = WorldPos;
                var nv = Vector3.TransformPosition(v,World);
                var mm = nv - ov;

                LocalPos = LocalPos + mm;//Matrix4.Invert(nv);
                //LocalPos = LocalPos + new Vector3(nv.X, nv.Y, nv.Z);


            }
        }
        public void LookAt(GraphNode3D n)
        {
            LookAt(n.WorldPos, new Vector3(0, 1, 0));
        }
        public void LookAt(Vector3 p,Vector3 up)
        {

            Matrix4 m = Matrix4.LookAt(Vector3.Zero, p - LocalPos, up);
            //Console.WriteLine("Local:" + LocalPos.ToString() + " TO:" + p.ToString());
            //m=m.ClearTranslation();

         //   m = m.Inverted();
            //m = m.ClearScale();
            //m = m.ClearProjection();
            m = m.Inverted();


                LocalTurn = m;

        }
        public void LookAtZero(Vector3 p,Vector3 up)
        {
            Matrix4 m = Matrix4.LookAt(Vector3.Zero, p, up);
            LocalTurn = m;
        }
        public virtual void PresentDepth(GraphCam3D c)
        {

        }
        public virtual void Present(GraphCam3D c)
        {

        }
    }
}

