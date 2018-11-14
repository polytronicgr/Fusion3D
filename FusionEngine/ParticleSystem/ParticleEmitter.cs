using OpenTK;
using System.Collections.Generic;
using Fusion3D.Effect;
using Fusion3D.Material;
using Fusion3D.Visuals;

namespace Fusion3D.ParticleSystem
{
    public class ParticleEmitter
    {
        public List<Particle> Particles = new List<Particle>();
        public Scene.SceneGraph3D Graph = null;
        public MaterialParticle3D PMat = null;
        public FXParticle PE;
        public RParticle PRen;

        public ParticleEmitter ( )
        {
            //PMat = new MaterialParticle3D();
            PE = new FXParticle ( );
            PRen = new RParticle ( );
        }

        public void Emit ( Particle bp, Vector3 pos, Vector3 inertia )
        {
            Particle np = new Particle ( bp )
            {
                // np.Tex = bp.Tex;

                //np.Life = bp.Life;
                //np.Alpha = bp.Alpha;
                Pos = pos ,
                Inertia = inertia
            };
            Particles.Add ( np );
            np.Meshes [ 0 ].Mat = new MaterialParticle3D
            {
                TCol = bp.Tex
            };
            Graph.Add ( np );
        }

        public void Update ( )
        {
        }

        public void Render ( )
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