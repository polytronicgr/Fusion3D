namespace VividScript
{
    public class VCompiledSource
    {
        public VTokenStream Tokens = null;
        public VStructs.VSEntry EntryPoint;

        public void VCompileSource ( VStructs.VSEntry entry )
        {
            EntryPoint = entry;
        }
    }
}