using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.VFX
{
    public class VFXParticleSystem : VFXBase
    {

        public List<ParticleBase> Particles
        {
            get;
            set;
        }

        public List<ParticleBase> Bases
        {
            get;
            set;
        }

        public List<ParticleBase> Removes
        {
            get;
            set;
        }

        public float SpawnRot1
        {
            get;
            set;
        }

        public float SpawnRot2
        {
            get;
            set;
        }

        public float SpawnZSmall
        {
            get;
            set;
        }

        public float SpawnZBig
        {
            get;
            set;
        }

        public float PowerSmall
        {
            get;
            set;
        }

        public float PowerBig
        {
            get;
            set;
        }

        public float XSmall
        {
            get;
            set;
        }

        public float YSmall
        {
            get;
            set;
        }

        public float XBig
        {
            get;
            set;
        }

        public float YBig
        {
            get;
            set;
        }

        public float ZSmall
        {
            get;
            set;
        }
        
        public float ZBig
        {
            get;
            set;
        }

        public float XIJit
        {
            get;
            set;
        }
        public float YIJit
        {
            get;
            set;
        }
        public float ZIJit
        {
            get;
            set;
        }
        public float RJit
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

        public float WJit
        {
            get;
            set;
        }

        public float HJit
        {
            get;
            set;
        }


        public VFXParticleSystem()
        {

            Particles = new List<ParticleBase>();
            Bases = new List<ParticleBase>();
            Removes = new List<ParticleBase>();
            SpawnRot1 = 0;
            SpawnRot2 = 359;
            SpawnZSmall = -0.25f;
            SpawnZBig = 0.25f;
            PowerSmall = 0.1f;
            PowerBig = 2.0f;
            XBig = YBig = XSmall = YSmall = 5;
            ZSmall = 0.2f;
            ZBig = 0.2f;
            RJit = 350;
            XIJit = 5;
            YIJit = 5;
            ZIJit = 0.2f;
            W = 32;
            H = 32;
        }

        public void Spawn(int number,float x,float y,float z,float xi,float yi,float zi)
        {

            Random rnd = new Random(Environment.TickCount);

            for(int i = 0; i < number; i++)
            {
                foreach(var pbase in Bases)
                {

                    var np = pbase.Clone();

                    np.W = W + rnd.Next(-(int)WJit / 2, (int)WJit / 2);
                    np.H = H + rnd.Next(-(int)HJit / 2, (int)HJit / 2);

                    np.X = x+rnd.Next((int)-XSmall, (int)XBig);
                    np.Y = y + rnd.Next((int)-YSmall, (int)YBig);

                    float zd = ZBig - ZSmall;

                    zd = zd * (float)rnd.NextDouble();

                    np.Z = z + zd;

                    np.Rot = rnd.Next((int)SpawnRot1, (int)SpawnRot2);

                    float xj = XIJit * (float)rnd.NextDouble();
                    float yj = YIJit * (float)rnd.NextDouble();
                    float zj = ZIJit * (float)rnd.NextDouble();

                    if(rnd.Next(0,5)>2)
                    {
                        xj = -xj;
                    }
                    if (rnd.Next(0, 5) > 2)
                    {
                        yj = -yj;
                    }
                    if (rnd.Next(0, 5) > 2)
                    {
                        zj = -zj;
                    }

                    float pw = (PowerBig-PowerSmall) * (float)rnd.NextDouble();

                    pw = PowerSmall + pw;

                    np.XI = (xi + xj)*pw;
                    np.YI = (yi + yj)*pw;
                    np.ZI = (zi + zj)*pw;

                    np.RI = rnd.Next((int)-RJit, (int)RJit);

                    //np.Life = 100.0f;
                    np.LifeRate = 0.03f;
                    np.LifeDrag = 0.98f;
                    np.Sys = this;

                    Particles.Add(np);

                }
            }

        }

        public void Remove(ParticleBase particle)
        {

            Removes.Add(particle);

        }

        public void AddBase(ParticleBase pbase)
        {

            Bases.Add(pbase);

        }

        public override void Init()
        {

        }

        public override void Update()
        {
            foreach(var particle in Particles)
            {
                particle.Update();
            }
            foreach(var particle in Removes)
            {
                Particles.Remove(particle);
            }
            Removes.Clear();
        }

        public override void Render()
        {
            foreach(var particle in Particles)
            {
                particle.Render();
            }
        }

        public override void Stop()
        {
            
        }

    }
}
