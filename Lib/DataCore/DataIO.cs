using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace DataCore
{
    public class DataIO
    {
        public string DataPath
        {
            get;
            set;
        }



        public List<DataEntry> Data { get; set; }

        public DataIO(string path)
        {

            DataPath = path;
            Data = new List<DataEntry>();

            if (File.Exists(path))
            {
                LoadIndex();
            }

        }
        public void LoadIndex()
        {


        }

        public void Save()
        {
            SaveIndex();

            var data = File.Create(DataPath);

            BinaryWriter dw = new BinaryWriter(data);

            foreach(var entry in Data)
            {
               
                    entry.Data.GenerateBytes();
                
                dw.Write(entry.Data.RawData);
            }

            data.Flush();
            data.Dispose();

        }

        public void SaveIndex()
        {

            var index = File.Create(DataPath + ".index");

            BinaryWriter iw = new BinaryWriter(index);

            iw.Write(Data.Count);

            foreach (var entry in Data)
            {
                iw.Write(entry.Begin);
                iw.Write(entry.Size);
                iw.Write(entry.Compressed);
                iw.Write(entry.Type);
                iw.Write(entry.Name);

            }

            index.Flush();

            index.Dispose();


        }

        public void AddData(Data data,string name)
        {

            var entry = new DataEntry();

            entry.Name = name;
            entry.Size = data.Bytes;
            entry.Loaded = true;
            entry.Compressed = false;
            entry.Type = data.GetType().Name;
            Console.WriteLine("Type:" + entry.Type);
            entry.Data = data;

            Data.Add(entry);
            RebuildStarts();

        }

        public void RebuildStarts()
        {

            int pos = 0;

            foreach(var entry in Data)
            {
                entry.Begin = pos;
                pos += entry.Size;
            }

        }



    }
}
