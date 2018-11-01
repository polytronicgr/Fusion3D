using Vivid3D.Scene;

namespace Vivid3D.Script
{
    public class ScriptBase : GraphNode3D
    {
        public GraphNode3D Node
        {
            get => Top;
            set
            {
                if ( Top != null )
                {
                    Top.Sub.Remove ( this );
                }
                Top = value;
                Top.Sub.Add ( this );
            }
        }

        public GraphEntity3D Entity
        {
            get => Top as GraphEntity3D;
            set
            {
                if ( Top != null )
                {
                    Top.Sub.Remove ( this );
                }
                Top = value;
                Top.Sub.Add ( this );
            }
        }

        public string FilePath
        {
            get;
            set;
        }

        public virtual void Begin ( )
        {
        }

        public virtual void End ( )
        {
        }

        public virtual void Pause ( )
        {
        }

        public virtual void Resume ( )
        {
        }

        public virtual void Update ( )
        {
        }

        public virtual void Draw ( )
        {
        }
    }
}