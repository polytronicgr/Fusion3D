namespace DataCore
{
    public class Data
    {
        public Data()
        {
            Generated = false;
        }

        public bool Generated { get; set; }

        public int Bytes
        {
            get
            {
                return _Bytes;
            }

            set
            {
                _Bytes = value;
            }
        }

        protected int _Bytes = 0;

        public byte[] RawData
        {
            get
            {
                return _RawData;
            }
            set
            {
                _RawData = value;
            }
        }

        protected byte[] _RawData = new byte[0];

        public virtual void GenerateBytes()
        {
        }

        public virtual void Reconstruct()
        {
        }
    }
}