namespace FusionEngine.Visuals
{
    public class RendererSimple : Renderer
    {
        public RendererSimple ( )
        {
        }

        public override void Init ( )
        {
            Layers.Add ( new RLSimple ( ) );
        }
    }
}