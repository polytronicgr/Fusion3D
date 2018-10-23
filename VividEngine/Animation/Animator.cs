using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.Animation
{
    public class Bone
    {
        public string Name { get; set; }
        public OpenTK.Matrix4 Offset { get; set; }
        public OpenTK.Matrix4 LocalTransform { get; set; }
        public OpenTK.Matrix4 GlobalTransform { get; set; }
        public OpenTK.Matrix4 OriginalLocalTransform { get; set; }
        public Bone Parent { get; set; }
        public List<Bone> Children = new List<Bone>();

    }
    public class Animator
    {

        private Bone _skeleton;
        private readonly Dictionary<string, Bone> _bonesByName;
        private readonly Dictionary<string, int> _bonesToIndex;
        private readonly Dictionary<string, int> _animationNameToId;
        private readonly List<Bone> _bones;
        public List<AnimEvaluator> Animations { get; private set; }
        private int CurrentAnimationIndex { get; set; }
        public bool HasSkeleton { get { return _bones.Count > 0; } }
        public string AnimationName { get { return Animations[CurrentAnimationIndex].Name; } }
        public float AnimationSpeed { get { return Animations[CurrentAnimationIndex].TicksPerSecond; } }
        public float Duration
        {
            get { return Animations[CurrentAnimationIndex].Duration / Animations[CurrentAnimationIndex].TicksPerSecond; }
        }

        public Animator()
        {
            _skeleton = null;
            CurrentAnimationIndex = -1;
            _bonesByName = new Dictionary<string, Bone>();
            _bonesToIndex = new Dictionary<string, int>();
            _animationNameToId = new Dictionary<string, int>();
            _bones = new List<Bone>();
            Animations = new List<AnimEvaluator>();
        }

        public void InitAssImp(Assimp.Scene aiRoot,Scene.GraphEntity3D root)
        {
            if (aiRoot.HasAnimations == false)
            {
                return;
            }

            _skeleton = CreateBoneTree(aiRoot.RootNode, null);

        }
        private OpenTK.Matrix4 ToTK(Assimp.Matrix4x4 mat)
        {
            return new OpenTK.Matrix4(mat.A1, mat.B1, mat.C1, mat.D1, mat.A2, mat.B2, mat.C2, mat.D2, mat.A3, mat.B3, mat.C3, mat.D3, mat.A4, mat.B4, mat.C4, mat.D4);
        }

        private int _i = 0;

        private Bone CreateBoneTree(Assimp.Node node,Bone parent)
        {
            var internalNode = new Bone
            {
                Name = node.Name,
                Parent = parent
            };
            if(internalNode.Name == "")
            {
                internalNode.Name = "foo" + _i++;
            }
            _bonesByName[internalNode.Name] = internalNode;
            var trans = node.Transform;
            trans.Transpose();

            internalNode.LocalTransform = ToTK(trans);
            internalNode.OriginalLocalTransform = internalNode.LocalTransform;
            CalculateBoneToWorldTransform(internalNode);


            for (var i = 0; i < node.ChildCount; i++)
            {
                var child = CreateBoneTree(node.Children[i], internalNode);
                if (child != null)
                {
                    internalNode.Children.Add(child);
                }
            }
            return internalNode;



        }

        private static void CalculateBoneToWorldTransform(Bone child)
        {
            child.GlobalTransform = child.LocalTransform;
            var parent = child.Parent;
            while (parent != null)
            {
                child.GlobalTransform *= parent.LocalTransform;
                parent = parent.Parent;
            }
        }

    }
    public class VectorKey
    {
        public double Time = 0.0;
        public OpenTK.Vector3 Value;
    }
    public class QuatKey
    {
        public double Time = 0.0;
        public OpenTK.Quaternion Value;
    }
    public class AnimChannel
    {
        public string Name = "";
        public List<VectorKey> PositionKeys = new List<VectorKey>();
        public List<QuatKey> RotationKeys = new List<QuatKey>();
        public List<VectorKey> ScalingKeys = new List<VectorKey>();
    }
    public class NodeAnimChannel
    {
        public string NodeName = "";
        public List<VectorKey> PositionKeys = new List<VectorKey>();
        public List<QuatKey> RotationKeys = new List<QuatKey>();
        public List<VectorKey> ScalingKeys = new List<VectorKey>();
    }
    public class Anim
    {
        public string Name;
        public double DurationInTicks = 0.0;
        public double TicksPerSecond = 0.0;
        public List<NodeAnimChannel> NodeAnimationChannels = new List<NodeAnimChannel>();
    }
    public class AnimEvaluator
    {
        public string Name { get; private set; }
        private List<AnimChannel> Channels { get; set; }
        public bool PlayAnimationForward { get; set; }
        private float LastTime { get; set; }
        public float TicksPerSecond { get; set; }
        public float Duration { get; private set; }
        private List<Tuple<int, int, int>> LastPositions { get; set; }
        public List<List<OpenTK.Matrix4>> Transforms { get; private set; }

        public AnimEvaluator(Anim anim)
        {
            LastTime = 0.0f;
            TicksPerSecond = anim.TicksPerSecond > 0.0f ? (float)anim.TicksPerSecond : 920.0f;
            Duration = (float)anim.DurationInTicks;
            Name = anim.Name;
            Channels = new List<AnimChannel>();
            foreach (var channel in anim.NodeAnimationChannels)
            {
                var c = new AnimChannel
                {
                    Name = channel.NodeName,
                    PositionKeys = channel.PositionKeys.ToList(),
                    RotationKeys = channel.RotationKeys.ToList(),
                    ScalingKeys = channel.ScalingKeys.ToList()
                };
                Channels.Add(c);
            }
            LastPositions = Enumerable.Repeat(new Tuple<int, int, int>(0, 0, 0), anim.NodeAnimationChannelCount).ToList();
            Transforms = new List<List<OpenTK.Matrix4>>();
            PlayAnimationForward = true;
        }
    }
}
