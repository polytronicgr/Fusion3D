using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Scene;
using System.IO;
namespace Vivid3D.Import
{
    public static class Import
    {
        public static Dictionary<string, Importer> Imports = new Dictionary<string, Importer>();
        public static void RegDefaults()
        {
            RegImp(".3ds", new AssImpImport());
            RegImp(".fbx", new AssImpImport());
            RegImp(".blend", new AssImpImport());
            RegImp(".dae", new AssImpImport());
            RegImp(".b3d", new AssImpImport());
        }
        public static void RegImp(string key,Importer imp)
        {
            Imports.Add(key, imp);
        }
        public static Importer GetImp(string key)
        {
            if (Imports.ContainsKey(key))
            {
                return Imports[key];
            }
            return null;
        }
        public static GraphNode3D ImportNode(string path)
        {
            string key = new FileInfo(path).Extension;
            var imp = Imports[key];
            var r=imp.LoadNode(path);
            return r;
           
        }
    }
}
