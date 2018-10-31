using System.Collections.Generic;

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

        public static void Init ( )
        {
            FX = new List<VFXBase> ( );
        }

        public static void Add ( VFXBase fx )
        {
            FX.Add ( fx );
            fx.Init ( );
        }

        public static void Clear ( )
        {
            foreach ( VFXBase fx in FX )
            {
                fx.Stop ( );
            }
            FX.Clear ( );
        }

        public static void Update ( )
        {
            foreach ( VFXBase fx in FX )
            {
                fx.Update ( );
            }
        }

        public static void Render ( )
        {
            foreach ( VFXBase fx in FX )
            {
                fx.Render ( );
            }
        }
    }
}