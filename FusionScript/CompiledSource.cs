namespace FusionScript
{
    public class CompiledSource
    {
        public TokenStream Tokens = null;
        public Structs.StructEntry EntryPoint;

        public void VCompileSource ( Structs.StructEntry entry )
        {
            EntryPoint = entry;
        }
    }
}