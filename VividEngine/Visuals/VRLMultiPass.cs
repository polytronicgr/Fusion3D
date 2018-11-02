using Vivid3D.Data;
using Vivid3D.Effect;

namespace Vivid3D.Visuals
{
    public class VRLMultiPassAnim : VRenderLayer
    {
        public EMultiPass3D fx = null;

        public override void Init ( )
        {
            fx = new EMultiPass3D ( );
        }

        public override void Render ( VMesh m, VVisualizer v )
        {
            m.Mat.Bind ( );
            if ( Lighting.GraphLight3D.Active != null )
            {
                Lighting.GraphLight3D.Active.ShadowFB.Cube.Bind ( 2 );
            }
            if ( FXG.FXOverride != null )
            {
                FXG.FXOverride.Bind ( );
            }
            else
            {
                fx.Bind ( );
            }
            v.SetMesh ( m );
            v.Bind ( );
            for ( int i = 0; i < m.Subs.Count; i++ )
            {
                v.Visualize ( i );
            }
            v.Release ( );
            if ( FXG.FXOverride != null )
            {
                FXG.FXOverride.Release ( );
            }
            else
            {
                fx.Release ( );
            }
            if ( Lighting.GraphLight3D.Active != null )
            {
                Lighting.GraphLight3D.Active.ShadowFB.Cube.Release ( 2 );
            }
            m.Mat.Release ( );
        }
    }

    public class VRLMultiPass : VRenderLayer
    {
        public EMultiPass3D fx = null;

        public override void Init ( )
        {
            fx = new EMultiPass3D ( );
        }

        public override void Render ( VMesh m, VVisualizer v )
        {
            m.Mat.Bind ( );
            if ( Lighting.GraphLight3D.Active != null )
            {
                Lighting.GraphLight3D.Active.ShadowFB.Cube.Bind ( 2 );
            }
            if ( FXG.FXOverride != null )
            {
                FXG.FXOverride.Bind ( );
            }
            else
            {
                fx.Bind ( );
            }
            v.SetMesh ( m );
            v.Bind ( );
            v.Visualize ( );
            v.Release ( );
            if ( FXG.FXOverride != null )
            {
                FXG.FXOverride.Release ( );
            }
            else
            {
                fx.Release ( );
            }
            if ( Lighting.GraphLight3D.Active != null )
            {
                Lighting.GraphLight3D.Active.ShadowFB.Cube.Release ( 2 );
            }
            m.Mat.Release ( );
        }
    }

    public class VRLTerrain : VRenderLayer
    {
        public ETerrain fx = null;

        public override void Init ( )
        {
            fx = new ETerrain ( );
        }

        public override void Render ( VMesh m, VVisualizer v )
        {
            m.Mat.Bind ( );
            if ( Lighting.GraphLight3D.Active != null )
            {
                Lighting.GraphLight3D.Active.ShadowFB.Cube.Bind ( 2 );
            }
            if ( FXG.FXOverride != null )
            {
                FXG.FXOverride.Bind ( );
            }
            else
            {
                fx.Bind ( );
            }
            v.SetMesh ( m );
            v.Bind ( );
            v.Visualize ( );
            v.Release ( );
            if ( FXG.FXOverride != null )
            {
                FXG.FXOverride.Release ( );
            }
            else
            {
                fx.Release ( );
            }
            if ( Lighting.GraphLight3D.Active != null )
            {
                Lighting.GraphLight3D.Active.ShadowFB.Cube.Release ( 2 );
            }
            m.Mat.Release ( );
        }
    }

    public class VRLLightMap : VRenderLayer
    {
        public ELightMap FX;

        public override void Init ( )
        {
            FX = new ELightMap ( );
        }

        public override void Render ( VMesh m, VVisualizer v )
        {
            m.Mat.BindLightmap ( );
            /* if (Lighting.GraphLight3D.Active != null)
             {
                 Lighting.GraphLight3D.Active.ShadowFB.Cube.Bind(2);
             }
             */
            if ( FXG.FXOverride != null )
            {
                FXG.FXOverride.Bind ( );
            }
            else
            {
                FX.Bind ( );
            }
            v.SetMesh ( m );
            v.Bind ( );
            v.Visualize ( );
            v.Release ( );
            if ( FXG.FXOverride != null )
            {
                FXG.FXOverride.Release ( );
            }
            else
            {
                FX.Release ( );
            }
            /*
            if (Lighting.GraphLight3D.Active != null)
            {
                Lighting.GraphLight3D.Active.ShadowFB.Cube.Release(2);
            }
            */
            m.Mat.ReleaseLightmap ( );
        }
    }

    public class VRLNoFX : VRenderLayer
    {
        public ENoFX FX;

        public override void Init ( )
        {
            FX = new ENoFX ( );
        }

        public override void Render ( VMesh m, VVisualizer v )
        {
            m.Mat.Bind ( );
            /* if (Lighting.GraphLight3D.Active != null)
             {
                 Lighting.GraphLight3D.Active.ShadowFB.Cube.Bind(2);
             }
             */
            if ( FXG.FXOverride != null )
            {
                FXG.FXOverride.Bind ( );
            }
            else
            {
                FX.Bind ( );
            }
            v.SetMesh ( m );
            v.Bind ( );
            v.Visualize ( );
            v.Release ( );
            if ( FXG.FXOverride != null )
            {
                FXG.FXOverride.Release ( );
            }
            else
            {
                FX.Release ( );
            }
            /*
            if (Lighting.GraphLight3D.Active != null)
            {
                Lighting.GraphLight3D.Active.ShadowFB.Cube.Release(2);
            }
            */
            m.Mat.Release ( );
        }
    }
}