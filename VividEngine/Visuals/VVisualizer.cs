﻿using Vivid3D.Data;

namespace Vivid3D.Visuals
{
    public class VVisualizer
    {
        public VVertexData<float> dat = null;
        public VMesh md = null;
        public int Vertices = 0, Indices = 0;

        public VVisualizer ( int vc , int ic )
        {
            Vertices = vc;
            Indices = ic;
        }

        public virtual void SetData ( VVertexData<float> d )
        {
        }

        public virtual void SetMesh ( VMesh m )
        {
        }

        public virtual void FinalAnim ( )
        {
        }

        public virtual void Final ( )
        {
        }

        public virtual void Init ( )
        {
        }

        public virtual void Bind ( )
        {
        }

        public virtual void Update ( )
        {
        }

        public virtual void Visualize ( int sub )
        {
        }

        public virtual void Visualize ( )
        {
        }

        public virtual void Release ( )
        {
        }

        public virtual void Clean ( )
        {
        }
    }
}