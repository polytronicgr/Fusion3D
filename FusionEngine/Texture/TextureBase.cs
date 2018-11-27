namespace FusionEngine.Texture
{
    public enum Format
    {
        RGB, RGBA, A, Depth16, Depth32, Stencil16, Stencil32, FrameBuffer16, FrameBuffer32
    }

    public class TextureBase
    {
        public int ID = 0;
        public string Path = "";
        public int W, H, D;
        public Format Form = Format.RGBA;

        public virtual void Bind ( int texu )
        {
        }

        public virtual void Release ( int texu )
        {
        }
    }
}