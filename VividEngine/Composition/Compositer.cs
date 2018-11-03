namespace Vivid3D.Composition
{
    public class Compositer
    {
        public FrameType [ ] Types
        {
            get;
            set;
        }

        public Scene.SceneGraph3D Graph
        {
            get => _Graph;
            set
            {
                _Graph = value;
                foreach ( FrameType type in Types )
                {
                    type.Graph = value;
                }
                if ( InputFrame != null )
                {
                    InputFrame.Graph = value;
                }
            }
        }

        private Scene.SceneGraph3D _Graph = null;

        public FrameType OutputFrame = null;

        public FrameType InputFrame
        {
            get => _IF;
            set
            {
                _IF = value;
                foreach ( FrameType f in Types )
                {
                }
            }
        }

        private FrameType _IF;
        public FrameBlend Blend = FrameBlend.Add;

        public Compositer ( int types )
        {
            Types = new FrameType [ types ];
        }

        public void GenerateFrames ( )
        {
            if ( InputFrame != null )
            {
                InputFrame.Generate ( );
            }
            foreach ( FrameType type in Types )
            {
                type.Generate ( );
            }
        }

        public void PresentFrame ( int frame )
        {
            Types [ frame ].Present ( );
        }

        public virtual void PreGen ( )
        {
        }

        public virtual void Process ( )
        {
            GenerateFrames ( );
        }

        public virtual void Render ( )
        {
        }
    }

    public enum FrameBlend
    {
        Solid, Alpha, Add, Mod
    }
}