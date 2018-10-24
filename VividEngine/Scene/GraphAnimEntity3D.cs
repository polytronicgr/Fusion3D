using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.Scene
{

    public class GraphAnimEntity3D : GraphEntity3D
    {

        public Vivid3D.Data.VMesh Mesh = null;
        public List<Vivid3D.Data.VMesh.Subset> Subsets = new List<Data.VMesh.Subset>();


        public Animation.Animator Animator = null;// new Animation.Animator();

        public string AnimName
        {
            get
            {
                return _AnimName;
            }
            set
            {
                _AnimName = Animator.Animations.Any(a => a.Name == value) ? value : "Still";
                Animator.SetAnimation(_AnimName);
                _timePos = 0;
            }
        }

        // these are the available animation clips
        public IEnumerable<string> Clips { get { return Animator.Animations.Select(a => a.Name); } }

        private readonly Queue<string> _clipQueue = new Queue<string>();
        public bool LoopClips { get; set; }

        // the bone transforms for the mesh instance
        private List<OpenTK.Matrix4> FinalTransforms { get { return Animator.GetTransforms((float)_timePos); } }

        public void Update(float dt)
        {
            _timePos += dt;

            if (_timePos > Animator.Duration)
            {
                _timePos = 0.0;
                return;
                if (_clipQueue.Any())
                {
                    AnimName = _clipQueue.Dequeue();
                    if (LoopClips)
                    {
                        _clipQueue.Enqueue(AnimName);
                    }
                }
                else
                {
                    AnimName = "Still";
                }
            }
            Console.WriteLine("____");
            foreach (var m in Meshes)
            {
                Console.WriteLine("Mesh:" + m.Name);
            }
        }
        public double _timePos = 0;
        public Animation.AnimEvaluator _Anim;
        private string _AnimName = "";

        /// </summary>
        public override void UpdateNode(float dt)
        {
          //  return;
            if (Animator != null)
            {
                Update(dt);
                List<OpenTK.Matrix4> bones = Animator.GetTransforms((float)_timePos);

                int vi = 0;
                foreach(Data.Vertex v in Mesh.VertexData)
                {

                    float weight0 = v.Weight;
                    //float weight1 = 1.0f - v.weight0 ;


                    //OpenTK.Vector3 p = OpenTK.Vector3.TransformVector(v.Pos, bones[v.BoneIndices[0]]); // OpenTK.Vector4.Transform(bones[v.BoneIndices[0]], new OpenTK.Vector4(v.Pos,1.0f));

                    OpenTK.Vector3 p;// = OpenTK.Vector4.Transform(new OpenTK.Vector4(v.Pos, 1.0f), bones[v.BoneIndices[0]]);


                    p = OpenTK.Vector3.TransformPosition(v.Pos, bones[v.BoneIndices[0]]);

                    OpenTK.Vector3 n = OpenTK.Vector3.TransformNormal(v.Norm, bones[v.BoneIndices[0]]);

                    OpenTK.Vector3 t = OpenTK.Vector3.TransformNormal(v.Tan, bones[v.BoneIndices[0]]);

                    OpenTK.Vector3 b = OpenTK.Vector3.TransformNormal(v.BiNorm, bones[v.BoneIndices[0]]);

                    Meshes[0].VertexData[vi].Pos = new OpenTK.Vector3(p);
                    Meshes[0].VertexData[vi].Norm = new OpenTK.Vector3(n);
                    Meshes[0].VertexData[vi].Tan = new OpenTK.Vector3(t);
                    Meshes[0].VertexData[vi].BiNorm = new OpenTK.Vector3(b);


                    //p += weight1 * 

                    
                    vi++;
                }

                Meshes[0].Viz.Update();


                Console.WriteLine("Time:" + _timePos);


            }



            foreach (var n in Sub)
            {
                n.UpdateNode(dt);
            }

        }

    }
}
