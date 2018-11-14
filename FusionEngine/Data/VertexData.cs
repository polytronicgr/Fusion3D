namespace Fusion3D.Data
{
    public class VertexData<T>
    {
        public int StrideSize = 0;
        public int ComponentSize = 0;
        public int Components = 0;
        public int Vertices = 0;
        public int Indices = 0;
        public T[] Data = null;
        public int[] Index = null;

        public virtual void Init ( int vertexCount, int components, int strideSize, int componentSize, int indexCount )
        {
            Data = new T [ vertexCount * components ];
            StrideSize = strideSize;
            ComponentSize = componentSize;
            Components = components;
            Vertices = vertexCount;
            Indices = indexCount;
            Index = new int [ indexCount ];
        }
    }
}