using Vivid3D.Scene;

namespace Vivid3D.Script
{
    public class ScriptMod
    {
        public GraphNode3D Node
        {
            get;
            set;
        }

        public GraphEntity3D Entity
        {
            get;
            set;
        }

        public GraphAnimEntity3D AnimEntity
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