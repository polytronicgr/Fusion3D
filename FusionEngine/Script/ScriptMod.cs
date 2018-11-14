using Fusion3D.Scene;

namespace Fusion3D.Script
{
    public class ScriptMod
    {
        public Node3D Node
        {
            get;
            set;
        }

        public Entity3D Entity
        {
            get;
            set;
        }

        public AnimEntity3D AnimEntity
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