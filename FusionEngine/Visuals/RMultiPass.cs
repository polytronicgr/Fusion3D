namespace FusionEngine.Visuals
{
    public class RMultiPass : Renderer
    {
        public RMultiPass ( )
        {
        }

        public override void Init ( )
        {
            Layers.Add ( new RLMultiPass ( ) );
        }
    }

    public class VRNoFx : Renderer
    {
        public VRNoFx ( )
        {
        }

        public override void Init ( )
        {
            Layers.Add ( new RLNoFX ( ) );
        }
    }

    public class VRTerrain : Renderer
    {
        public VRTerrain ( )
        {
        }

        public override void Init ( )
        {
            Layers.Add ( new RLTerrain ( ) );
        }
    }

    public class VRLightMap : Renderer
    {
        public VRLightMap ( )
        {
        }

        public override void Init ( )
        {
            Layers.Add ( new RLLightMap ( ) );
        }
    }

    public class VRMultiPassAnim : Renderer
    {
        public VRMultiPassAnim ( )
        {
        }

        public override void Init ( )
        {
            Layers.Add ( new RLMultiPassAnim ( ) );
        }
    }
}