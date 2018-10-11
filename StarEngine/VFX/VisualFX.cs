using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vivid3D.VFX
{
    public static class VisualFX
    {
        public static List<VFXBase> FX
        {

            get;
            set;

        }

        public static Scene.SceneGraph Graph
        {
            get;
            set;
        }

        public static void Init()
        {
            FX = new List<VFXBase>();
        }

        public static void Add(VFXBase fx)
        {

            FX.Add(fx);
            fx.Init();

        }

        public static void Clear()
        {

            foreach(var fx in FX)
            {
                fx.Stop();
            }
            FX.Clear();
            
        }

        public static void Update()
        {

            foreach(var fx in FX)
            {
                fx.Update();
            }

        }

        public static void Render()
        {

            foreach(var fx in FX)
            {
                fx.Render();
            }

        }

    }
}
