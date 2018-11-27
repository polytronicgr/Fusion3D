using System;
using System.Collections.Generic;
using System.Linq;

namespace FusionEngine.Animation
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
        public bool HasSkeleton => _bones.Count > 0;
        public string AnimationName => Animations [ CurrentAnimationIndex ].Name;
        public float AnimationSpeed => Animations [ CurrentAnimationIndex ].TicksPerSecond;

        public float Duration => Animations [ CurrentAnimationIndex ].Duration / Animations [ CurrentAnimationIndex ].TicksPerSecond;

        public Animator ( )
        {
            _skeleton = null;
            CurrentAnimationIndex = -1;
            _bonesByName = new Dictionary<string, Bone> ( );
            _bonesToIndex = new Dictionary<string, int> ( );
            _animationNameToId = new Dictionary<string, int> ( );
            _bones = new List<Bone> ( );
            Animations = new List<AnimEvaluator> ( );
        }

        public int GetBoneIndex ( string name )
        {
            if ( _bonesToIndex.ContainsKey ( name ) )
            {
                return _bonesToIndex [ name ];
            }
            return -1;
        }

        public void SetAnimationIndex ( int index )
        {
            CurrentAnimationIndex = index;
        }

        public void InitAssImp ( Assimp.Scene aiRoot, Scene.Entity3D root )
        {
            if ( aiRoot.HasAnimations == false )
            {
                return;
            }

            _skeleton = CreateBoneTree ( aiRoot.RootNode, null );
            Console.WriteLine ( "Proc bones:" + _skeleton.Name + " C:" + _skeleton.Children.Count );
            foreach ( Assimp.Mesh mesh in aiRoot.Meshes )
            {
                foreach ( Assimp.Bone bone in mesh.Bones )
                {
                    if ( !_bonesByName.TryGetValue ( bone.Name, out Bone found ) )
                    {
                        continue;
                    }

                    bool skip = (from t in _bones let bname = bone.Name where t.Name == bname select t).Any();
                    if ( skip )
                    {
                        continue;
                    }

                    found.Offset = ToTK ( bone.OffsetMatrix );
                    _bones.Add ( found );
                    _bonesToIndex [ found.Name ] = _bones.IndexOf ( found );
                }
                Assimp.Mesh mesh1 = mesh;
                foreach ( string bone in _bonesByName.Keys.Where ( b => mesh1.Bones.All ( b1 => b1.Name != b ) && b.StartsWith ( "Bone" ) ) )
                {
                    _bonesByName [ bone ].Offset = _bonesByName [ bone ].Parent.Offset;
                    _bones.Add ( _bonesByName [ bone ] );
                    _bonesToIndex [ bone ] = _bones.IndexOf ( _bonesByName [ bone ] );
                }
            }
            ExtractAnimations ( aiRoot );

            const float timestep = 1.0f / 30.0f;
            for ( int i = 0; i < Animations.Count; i++ )
            {
                SetAnimationIndex ( i );
                float dt = 0.0f;
                for ( float ticks = 0.0f; ticks < Animations [ i ].Duration; ticks += Animations [ i ].TicksPerSecond / 30.0f )
                {
                    dt += timestep;
                    Calculate ( dt );
                    List<OpenTK.Matrix4> trans = new List<OpenTK.Matrix4> ( );
                    for ( int a = 0; a < _bones.Count; a++ )
                    {
                        OpenTK.Matrix4 rotMat = _bones [ a ].Offset * _bones [ a ].GlobalTransform;
                        trans.Add ( rotMat );
                    }
                    Animations [ i ].Transforms.Add ( trans );
                }
            }
            Console.WriteLine ( "Finished loading animations with " + _bones.Count + " bones" );
        }

        private void Calculate ( float dt )
        {
            if ( ( CurrentAnimationIndex < 0 ) | ( CurrentAnimationIndex >= Animations.Count ) )
            {
                return;
            }
            Animations [ CurrentAnimationIndex ].Evaluate ( dt, _bonesByName );
            UpdateTransforms ( _skeleton );
        }

        private static void UpdateTransforms ( Bone node )
        {
            CalculateBoneToWorldTransform ( node );
            foreach ( Bone child in node.Children )
            {
                UpdateTransforms ( child );
            }
        }

        private void ExtractAnimations ( Assimp.Scene scene )
        {
            foreach ( Assimp.Animation animation in scene.Animations )
            {
                Animations.Add ( new AnimEvaluator ( animation ) );
            }
            for ( int i = 0; i < Animations.Count; i++ )
            {
                _animationNameToId [ Animations [ i ].Name ] = i;
            }
            CurrentAnimationIndex = 0;
        }

        private OpenTK.Matrix4 ToTK ( Assimp.Matrix4x4 mat )
        {
            return new OpenTK.Matrix4 ( mat.A1, mat.B1, mat.C1, mat.D1, mat.A2, mat.B2, mat.C2, mat.D2, mat.A3, mat.B3, mat.C3, mat.D3, mat.A4, mat.B4, mat.C4, mat.D4 );
        }

        private int _i = 0;

        private Bone CreateBoneTree ( Assimp.Node node, Bone parent )
        {
            Bone internalNode = new Bone
            {
                Name = node.Name,
                Parent = parent
            };
            if ( internalNode.Name == "" )
            {
                internalNode.Name = "foo" + _i++;
            }
            _bonesByName [ internalNode.Name ] = internalNode;
            Assimp.Matrix4x4 trans = node.Transform;
            // trans.Transpose();

            internalNode.LocalTransform = ToTK ( trans );
            internalNode.OriginalLocalTransform = internalNode.LocalTransform;
            CalculateBoneToWorldTransform ( internalNode );

            for ( int i = 0; i < node.ChildCount; i++ )
            {
                Bone child = CreateBoneTree(node.Children[i], internalNode);
                if ( child != null )
                {
                    internalNode.Children.Add ( child );
                }
            }
            return internalNode;
        }

        public bool SetAnimation ( string animation )
        {
            if ( _animationNameToId.TryGetValue ( animation, out int index ) )
            {
                int oldIndex = CurrentAnimationIndex;
                CurrentAnimationIndex = index;
                return oldIndex != CurrentAnimationIndex;
            }
            return false;
        }

        public void PlayAnimationForward ( )
        {
            Animations [ CurrentAnimationIndex ].PlayAnimationForward = true;
        }

        public void PlayAnimationBackward ( )
        {
            Animations [ CurrentAnimationIndex ].PlayAnimationForward = false;
        }

        public void AdjustAnimationSpeedBy ( float prc )
        {
            Animations [ CurrentAnimationIndex ].TicksPerSecond *= prc;
        }

        public void AdjustAnimationSpeedTo ( float ticksPerSec )
        {
            Animations [ CurrentAnimationIndex ].TicksPerSecond = ticksPerSec;
        }

        public List<OpenTK.Matrix4> GetTransforms ( float dt )
        {
            return Animations [ CurrentAnimationIndex ].GetTransforms ( dt );
        }

        private static void CalculateBoneToWorldTransform ( Bone child )
        {
            child.GlobalTransform = child.LocalTransform;
            Bone parent = child.Parent;
            while ( parent != null )
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
        private readonly List<MutableTuple<int , int , int>> LastPositions = new List<MutableTuple<int , int , int>> ( );
        public List<List<OpenTK.Matrix4>> Transforms { get; private set; }

        public void Evaluate ( float dt, Dictionary<string, Bone> bones )
        {
            dt *= TicksPerSecond;
            float time = 0.0f;
            if ( Duration > 0.0f )
            {
                time = dt % Duration;
            }
            for ( int i = 0; i < Channels.Count; i++ )
            {
                AnimChannel channel = Channels[i];
                if ( !bones.ContainsKey ( channel.Name ) )
                {
                    Console.WriteLine ( "Did not find the bone node " + channel.Name );
                    continue;
                }
                // interpolate position keyframes
                OpenTK.Vector3 pPosition = new OpenTK.Vector3 ( );
                if ( channel.PositionKeys.Count > 0 )
                {
                    int frame = (time >= LastTime) ? LastPositions[i].Item1 : 0;
                    while ( frame < channel.PositionKeys.Count - 1 )
                    {
                        if ( time < channel.PositionKeys [ frame + 1 ].Time )
                        {
                            break;
                        }
                        frame++;
                    }
                    if ( frame >= channel.PositionKeys.Count )
                    {
                        frame = 0;
                    }

                    int nextFrame = (frame + 1) % channel.PositionKeys.Count;

                    VectorKey key = channel.PositionKeys[frame];
                    VectorKey nextKey = channel.PositionKeys[nextFrame];
                    double diffTime = nextKey.Time - key.Time;
                    if ( diffTime < 0.0 )
                    {
                        diffTime += Duration;
                    }
                    if ( diffTime > 0.0 )
                    {
                        float factor = (float)((time - key.Time) / diffTime);
                        pPosition = key.Value + ( nextKey.Value - key.Value ) * factor;
                    }
                    else
                    {
                        pPosition = key.Value;
                    }
                    MutableTuple<int , int , int> tl = LastPositions [ i ];
                    tl.Item1 = frame;
                    LastPositions [ i ].Item1 = frame;
                }
                // interpolate rotation keyframes

                OpenTK.Quaternion pRot = new OpenTK.Quaternion ( 0 , 0 , 0 , 1 );
                if ( channel.RotationKeys.Count > 0 )
                {
                    int frame = (time >= LastTime) ? LastPositions[i].Item2 : 0;
                    while ( frame < channel.RotationKeys.Count - 1 )
                    {
                        if ( time < channel.RotationKeys [ frame + 1 ].Time )
                        {
                            break;
                        }
                        frame++;
                    }
                    if ( frame >= channel.RotationKeys.Count )
                    {
                        frame = 0;
                    }
                    int nextFrame = (frame + 1) % channel.RotationKeys.Count;

                    QuatKey key = channel.RotationKeys[frame];
                    QuatKey nextKey = channel.RotationKeys[nextFrame];
                    key.Value.Normalize ( );
                    nextKey.Value.Normalize ( );
                    double diffTime = nextKey.Time - key.Time;
                    if ( diffTime < 0.0 )
                    {
                        diffTime += Duration;
                    }
                    if ( diffTime > 0 )
                    {
                        float factor = (float)((time - key.Time) / diffTime);
                        pRot = OpenTK.Quaternion.Slerp ( key.Value, nextKey.Value, factor );
                    }
                    else
                    {
                        pRot = key.Value;
                    }
                    LastPositions [ i ].Item1 = frame;
                }
                // interpolate scale keyframes
                OpenTK.Vector3 pscale = new OpenTK.Vector3 ( 1 );
                if ( channel.ScalingKeys.Count > 0 )
                {
                    int frame = (time >= LastTime) ? LastPositions[i].Item3 : 0;
                    while ( frame < channel.ScalingKeys.Count - 1 )
                    {
                        if ( time < channel.ScalingKeys [ frame + 1 ].Time )
                        {
                            break;
                        }
                        frame++;
                    }
                    if ( frame >= channel.ScalingKeys.Count )
                    {
                        frame = 0;
                    }
                    LastPositions [ i ].Item3 = frame;
                }

                //OpenTK.Matrix4 mat = OpenTK.Matrix4.CreateFromQuaternion(pRot);

                // create the combined transformation matrix

                Assimp.Matrix4x4 mat = new Assimp.Matrix4x4 ( new Assimp.Quaternion ( pRot.W , pRot.X , pRot.Y , pRot.Z ).GetMatrix ( ) );

                mat.A1 *= pscale.X; mat.B1 *= pscale.X; mat.C1 *= pscale.X;
                mat.A2 *= pscale.Y; mat.B2 *= pscale.Y; mat.C2 *= pscale.Y;
                mat.A3 *= pscale.Z; mat.B3 *= pscale.Z; mat.C3 *= pscale.Z;
                mat.A4 = pPosition.X; mat.B4 = pPosition.Y; mat.C4 = pPosition.Z;

                // transpose to get DirectX style matrix
                //mat.Transpose();
                //mat.Inverse();

                bones [ channel.Name ].LocalTransform = ToTK ( mat );
            }
            LastTime = time;
        }

        private OpenTK.Matrix4 ToTK ( Assimp.Matrix4x4 mat )
        {
            return new OpenTK.Matrix4 ( mat.A1, mat.B1, mat.C1, mat.D1, mat.A2, mat.B2, mat.C2, mat.D2, mat.A3, mat.B3, mat.C3, mat.D3, mat.A4, mat.B4, mat.C4, mat.D4 );
        }

        public List<OpenTK.Matrix4> GetTransforms ( float dt )
        {
            return Transforms [ GetFrameIndexAt ( dt ) ];
        }

        private int GetFrameIndexAt ( float dt )
        {
            dt *= TicksPerSecond;
            float time = 0.0f;
            if ( Duration > 0.0f )
            {
                time = dt % Duration;
            }
            float percent = time / Duration;
            if ( !PlayAnimationForward )
            {
                percent = ( percent - 1.0f ) * -1.0f;
            }
            int frameIndexAt = (int)(Transforms.Count * percent);
            return frameIndexAt;
        }

        public AnimEvaluator ( Assimp.Animation anim )
        {
            LastTime = 0.0f;
            TicksPerSecond = anim.TicksPerSecond > 0.0f ? ( float ) anim.TicksPerSecond : 920.0f;
            Duration = ( float ) anim.DurationInTicks;
            Name = anim.Name;
            Channels = new List<AnimChannel> ( );
            foreach ( Assimp.NodeAnimationChannel channel in anim.NodeAnimationChannels )
            {
                AnimChannel nac = new AnimChannel();

                List<VectorKey> pk = new List<VectorKey> ( );
                List<QuatKey> rk = new List<QuatKey> ( );
                List<VectorKey> sk = new List<VectorKey> ( );
                foreach ( Assimp.VectorKey k in channel.PositionKeys )
                {
                    VectorKey npk = new VectorKey
                    {
                        Time = k.Time ,
                        Value = new OpenTK.Vector3 ( k.Value.X , k.Value.Y , k.Value.Z )
                    };

                    pk.Add ( npk );
                }
                foreach ( Assimp.QuaternionKey k in channel.RotationKeys )
                {
                    QuatKey nrk = new QuatKey
                    {
                        Time = k.Time ,
                        Value = new OpenTK.Quaternion ( k.Value.X , k.Value.Y , k.Value.Z , k.Value.W )
                    };
                    rk.Add ( nrk );
                }
                foreach ( Assimp.VectorKey s in channel.ScalingKeys )
                {
                    VectorKey nsk = new VectorKey
                    {
                        Time = s.Time ,
                        Value = new OpenTK.Vector3 ( s.Value.X , s.Value.Y , s.Value.Z )
                    };
                    sk.Add ( nsk );
                }
                nac.Name = channel.NodeName;
                nac.PositionKeys = pk;
                nac.RotationKeys = rk;
                nac.ScalingKeys = sk;
                Channels.Add ( nac );
            }
            LastPositions = Enumerable.Repeat ( new MutableTuple<int, int, int> ( 0, 0, 0 ), anim.NodeAnimationChannelCount ).ToList ( );
            Transforms = new List<List<OpenTK.Matrix4>> ( );
            PlayAnimationForward = true;
        }

        public AnimEvaluator ( Anim anim )
        {
            LastTime = 0.0f;
            TicksPerSecond = anim.TicksPerSecond > 0.0f ? ( float ) anim.TicksPerSecond : 920.0f;
            Duration = ( float ) anim.DurationInTicks;
            Name = anim.Name;
            Channels = new List<AnimChannel> ( );
            foreach ( NodeAnimChannel channel in anim.NodeAnimationChannels )
            {
                AnimChannel c = new AnimChannel
                {
                    Name = channel.NodeName,
                    PositionKeys = channel.PositionKeys.ToList(),
                    RotationKeys = channel.RotationKeys.ToList(),
                    ScalingKeys = channel.ScalingKeys.ToList()
                };
                Channels.Add ( c );
            }
            LastPositions = Enumerable.Repeat ( new MutableTuple<int, int, int> ( 0, 0, 0 ), anim.NodeAnimationChannels.Count ).ToList ( );
            Transforms = new List<List<OpenTK.Matrix4>> ( );
            PlayAnimationForward = true;
        }
    }

    public class MutableTuple<T1, T2, T3>
    {
        public MutableTuple ( T1 i, T2 i1, T3 i2 )
        {
            Item1 = i;
            Item2 = i1;
            Item3 = i2;
        }

        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }
        public T3 Item3 { get; set; }
    }
}