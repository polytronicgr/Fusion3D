using System;
using System.Collections.Generic;
using System.Linq;

namespace Fusion3D.Scene
{
    public class AnimEntity3D : Entity3D
    {
        public Animation.AnimEvaluator _Anim;
        public double _timePos = 0;
        public Animation.Animator Animator = null;
        public Fusion3D.Data.Mesh3D Mesh = null;
        public List<Fusion3D.Data.Mesh3D.Subset> Subsets = new List<Data.Mesh3D.Subset>();

        // new Animation.Animator();

        private readonly Queue<string> _clipQueue = new Queue<string>();

        private string _AnimName = "";

        public AnimEntity3D ( )
        {
            Console.WriteLine ( "AnimEntity created." );
        }

        public string AnimName
        {
            get => _AnimName;
            set
            {
                _AnimName = Animator.Animations.Any ( a => a.Name == value ) ? value : "Still";
                Animator.SetAnimation ( _AnimName );
                _timePos = 0;
            }
        }

        // these are the available animation clips
        public IEnumerable<string> Clips => Animator.Animations.Select ( a => a.Name );

        public bool LoopClips { get; set; }

        // the bone transforms for the mesh instance
        private List<OpenTK.Matrix4> FinalTransforms => Animator.GetTransforms ( ( float ) _timePos );

        public void Update ( float dt )
        {
            _timePos += dt;

            if ( _timePos > Animator.Duration )
            {
                _timePos = 0.0;
                return;
                if ( _clipQueue.Any ( ) )
                {
                    AnimName = _clipQueue.Dequeue ( );
                    if ( LoopClips )
                    {
                        _clipQueue.Enqueue ( AnimName );
                    }
                }
                else
                {
                    AnimName = "Still";
                }
            }
            Console.WriteLine ( "____" );
            foreach ( Data.Mesh3D m in Meshes )
            {
                Console.WriteLine ( "Mesh:" + m.Name );
            }
        }

        /// </summary>
        public override void UpdateNode ( float dt )
        {
            // return;
            if ( Animator != null )
            {
                Update ( dt );
                List<OpenTK.Matrix4> bones = Animator.GetTransforms((float)_timePos);

                int vi = 0;
                foreach ( Data.Vertex v in Mesh.VertexData )
                {
                    float weight0 = v.Weight;
                    //float weight1 = 1.0f - v.weight0 ;

                    //OpenTK.Vector3 p = OpenTK.Vector3.TransformVector(v.Pos, bones[v.BoneIndices[0]]); // OpenTK.Vector4.Transform(bones[v.BoneIndices[0]], new OpenTK.Vector4(v.Pos,1.0f));

                    OpenTK.Vector3 p;// = OpenTK.Vector4.Transform(new OpenTK.Vector4(v.Pos, 1.0f), bones[v.BoneIndices[0]]);

                    p = OpenTK.Vector3.TransformPosition ( v.Pos, bones [ v.BoneIndices [ 0 ] ] );

                    OpenTK.Vector3 n = OpenTK.Vector3.TransformNormal(v.Norm, bones[v.BoneIndices[0]]);

                    OpenTK.Vector3 t = OpenTK.Vector3.TransformNormal(v.Tan, bones[v.BoneIndices[0]]);

                    OpenTK.Vector3 b = OpenTK.Vector3.TransformNormal(v.BiNorm, bones[v.BoneIndices[0]]);

                    Meshes [ 0 ].VertexData [ vi ].Pos = new OpenTK.Vector3 ( p );
                    Meshes [ 0 ].VertexData [ vi ].Norm = new OpenTK.Vector3 ( n );
                    Meshes [ 0 ].VertexData [ vi ].Tan = new OpenTK.Vector3 ( t );
                    Meshes [ 0 ].VertexData [ vi ].BiNorm = new OpenTK.Vector3 ( b );

                    //p += weight1 *

                    vi++;
                }

                Meshes [ 0 ].Viz.Update ( );

                Console.WriteLine ( "Time:" + _timePos );
            }

            foreach ( Node3D n in Sub )
            {
                n.UpdateNode ( dt );
            }
        }
    }
}