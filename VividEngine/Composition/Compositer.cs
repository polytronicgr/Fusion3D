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
            }
        }

        private Scene.SceneGraph3D _Graph = null;

        public Compositer ( int types )
        {
            Types = new FrameType [ types ];
        }

        public void GenerateFrames ( )
        {
            foreach ( FrameType type in Types )
            {
                type.Generate ( );
            }
        }

        public void PresentFrame ( int frame )
        {
            Types [ frame ].Present ( );
        }

        public virtual void Process ( )
        {
            GenerateFrames ( );
        }

        public virtual void Render ( )
        {
        }
    }
}