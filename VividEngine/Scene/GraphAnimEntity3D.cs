using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.Scene
{

    public class GraphAnimEntity3D : GraphEntity3D
    {


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

            if (Animator != null)
            {
                Update(dt);
            }

            foreach (var n in Sub)
            {
                n.UpdateNode(dt);
            }

        }

    }
}
