using Vivid3D.Scene;

namespace Vivid3D.Import
{
    public class Importer
    {
        public string Ext = "";

        public virtual GraphNode3D LoadNode(string path)
        {
            return null;
        }

        public virtual GraphNode3D LoadAnimNode(string path)
        {
            return null;
        }

        public virtual SceneGraph3D LoadScene(string path)
        {
            return null;
        }
    }
}