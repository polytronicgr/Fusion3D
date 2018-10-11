using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Vivid3D.Texture;
using Vivid3D.Material;
using Vivid3D.Effect;
using Vivid3D.Visuals;
namespace Vivid3D.ParticleSystem
{
    public class ParticleEmitter
    {
        public List<Particle> Particles = new List<Particle>();
        public Scene.SceneGraph3D Graph = null;
        public MaterialParticle3D PMat = null;
        public EParticle PE;
        public VRParticle PRen;
        public ParticleEmitter()
        {
            //PMat = new MaterialParticle3D();
            PE = new EParticle();
            PRen = new VRParticle();
        }
        public void Emit(Particle bp, Vector3 pos, Vector3 inertia)
        {

            var np = new Particle(bp);
           // np.Tex = bp.Tex;

            //np.Life = bp.Life;
            //np.Alpha = bp.Alpha;
            np.Pos = pos;
            np.Inertia = inertia;
            Particles.Add(np);
            np.Meshes[0].Mat = new MaterialParticle3D();
            np.Meshes[0].Mat.TCol = bp.Tex;
            Graph.Add(np);
            

        }

        public void Update()
        {

        }

        public void Render()
        {

            //List<Particle>[] chains = Sort();
           // Console.WriteLine("Chains:" + chains.Length);
  //          foreach(var pl in chains)
    //        {
            //    Console.WriteLine("Chain:" + pl.Count);
//            }


        }
       
       
    }
}
