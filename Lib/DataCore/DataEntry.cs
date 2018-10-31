namespace DataCore
{
    public class DataEntry
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Begin { get; set; }
        public int Size { get; set; }
        public Data Data { get; set; }
        public bool Loaded { get; set; }
        public bool Compressed { get; set; }

        public DataEntry()
        {
            Loaded = false;
            Compressed = false;
        }
    }
}