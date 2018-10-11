using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Scene;
using Vivid3D.Tex;
using Vivid3D.Util;

namespace Vivid3D.VFX
{
    public class ParticleBase
    {

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

        public float XI
        {
            get;
            set;
        }

        public float YI
        {
            get;
            set;
        }

        public float ZI
        {
            get;
            set;
        }

        public float RI
        {
            get;
            set;
        }

        public float Weight
        {
            get;
            set;
        }

        public float XDrag
        {
            get;
            set;
        }

        public float YDrag
        {
            get;
            set;
        }

        public float ZDrag
        {
            get;
            set;
        }

        public float RDrag
        {
            get;
            set;
        }

        public float Life
        {
            get;
            set;
        }

        public float LifeRate
        {
            get;
            set;
        }

        public float LifeDrag
        {
            get;
            set;
        }

        public float W
        {
            get;
            set;
        }

        public float H
        {
            get;
            set;
        }
       
        public Tex2D Img
        {
            get;
            set;
        }

        public float Alpha
        {
            get;
            set;
        }

        public float AI
        {
            get;
            set;
        }

        public VFXParticleSystem Sys
        {
            get;
            set;
        }

        public ParticleBase()
        {
            X = Y = Z = Rot = 0;
            XDrag = YDrag = ZDrag = RDrag = 0.0f;

            Img = null;
            XI = YI = ZI = RI = 0;
            Weight = 1.0f;
            Life = 1.0f;
            LifeRate = 0.1f;
            LifeDrag = 0.78f;
            W = 16;
            H = 16;
            Alpha = 1.0f;
            AI = 0.91f;
        }

        public virtual ParticleBase Clone()
        {
            return null;
        }

        public virtual void Init()
        {

        }

        public virtual void Update()
        {
            X += XI;
            Y += YI;
            Z += ZI;
            Rot += RI;

            XI = XI * XDrag * Weight;
            YI = YI * YDrag * Weight;
            ZI = ZI * ZDrag * Weight;
            RI = RI * RDrag * Weight;

            Life = Life - LifeRate * LifeDrag;

            Alpha *= AI;
          //  Console.WriteLine("Life:" + Life);
            if (Life <= 0.0f)
            {
             //   Console.WriteLine("Removed");
                Sys.Remove(this);
            }


        }


        public virtual void Render()
        {

        }

        public virtual void Stop()
        {

        }

    }
}
