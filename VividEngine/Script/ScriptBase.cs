using Vivid3D.Scene;

namespace Vivid3D.Script
{
    public class ScriptBase
    {
        public GraphNode3D Node { get; set; }
        public GraphEntity3D Entity { get; set; }

        public virtual void Init()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Draw()
        {
        }
    }
}