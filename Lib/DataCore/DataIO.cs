using System;
using System.Collections.Generic;
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
        public FileStream DataFile;
        public BinaryReader DataR;

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
            var index = File.Open(DataPath + ".index", FileMode.Open);

            BinaryReader ir = new BinaryReader(index);

            int ic = ir.ReadInt32();

            Data.Clear();

            for (int i = 0; i < ic; i++)
            {
                var entry = new DataEntry();
                entry.Begin = ir.ReadInt32();
                entry.Size = ir.ReadInt32();
                entry.Compressed = ir.ReadBoolean();
                entry.Type = ir.ReadString();
                entry.Name = ir.ReadString();
                Data.Add(entry);
            }

            index.Close();
        }

        public void OpenData()
        {
            DataFile = File.Open(DataPath, FileMode.Open);
            DataR = new BinaryReader(DataFile);
        }

        public void CloseData()
        {
            DataFile.Flush();
            DataFile.Close();
            DataFile = null;
            DataR = null;
        }

        public void Save()
        {
            SaveIndex();

            var data = File.Create(DataPath);

            BinaryWriter dw = new BinaryWriter(data);

            foreach (var entry in Data)
            {
                if (entry.Data.Generated == false)
                {
                    entry.Data.GenerateBytes();
                }

                dw.Write(entry.Data.RawData);
            }

            data.Flush();
            data.Close();
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

            index.Close();
        }

        public Data GetData(string name)
        {
            foreach (var entry in Data)
            {
                if (entry.Name == name)
                {
                    if (entry.Loaded == false)
                    {
                        OpenData();

                        DataFile.Seek(entry.Begin, SeekOrigin.Begin);
                        var dat = DataR.ReadBytes(entry.Size);

                        CloseData();

                        entry.Loaded = true;
                        entry.Data = Activator.CreateInstance(Type.GetType("DataCore.DataTypes." + entry.Type)) as Data;
                        entry.Data.RawData = dat;
                        entry.Data.Reconstruct();
                        return entry.Data;
                    }
                    else
                    {
                        return entry.Data;
                    }
                }
            }

            return null;
        }

        public void AddData(Data data, string name)
        {
            var entry = new DataEntry();
            if (data.Generated == false) data.GenerateBytes();

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

            foreach (var entry in Data)
            {
                entry.Begin = pos;
                pos += entry.Size;
            }
        }
    }
}