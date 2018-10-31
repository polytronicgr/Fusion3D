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

        public DataIO ( string path )
        {
            DataPath = path;
            Data = new List<DataEntry> ( );

            if ( File.Exists ( path ) )
            {
                LoadIndex ( );
            }
        }

        public void LoadIndex ( )
        {
            FileStream index = File.Open(DataPath + ".index", FileMode.Open);

            BinaryReader ir = new BinaryReader(index);

            int ic = ir.ReadInt32();

            Data.Clear ( );

            for ( int i = 0 ; i < ic ; i++ )
            {
                DataEntry entry = new DataEntry
                {
                    Begin = ir.ReadInt32 ( ) ,
                    Size = ir.ReadInt32 ( ) ,
                    Compressed = ir.ReadBoolean ( ) ,
                    Type = ir.ReadString ( ) ,
                    Name = ir.ReadString ( )
                };
                Data.Add ( entry );
            }

            index.Close ( );
        }

        public void OpenData ( )
        {
            DataFile = File.Open ( DataPath , FileMode.Open );
            DataR = new BinaryReader ( DataFile );
        }

        public void CloseData ( )
        {
            DataFile.Flush ( );
            DataFile.Close ( );
            DataFile = null;
            DataR = null;
        }

        public void Save ( )
        {
            SaveIndex ( );

            FileStream data = File.Create(DataPath);

            BinaryWriter dw = new BinaryWriter(data);

            foreach ( DataEntry entry in Data )
            {
                if ( entry.Data.Generated == false )
                {
                    entry.Data.GenerateBytes ( );
                }

                dw.Write ( entry.Data.RawData );
            }

            data.Flush ( );
            data.Close ( );
        }

        public void SaveIndex ( )
        {
            FileStream index = File.Create(DataPath + ".index");

            BinaryWriter iw = new BinaryWriter(index);

            iw.Write ( Data.Count );

            foreach ( DataEntry entry in Data )
            {
                iw.Write ( entry.Begin );
                iw.Write ( entry.Size );
                iw.Write ( entry.Compressed );
                iw.Write ( entry.Type );
                iw.Write ( entry.Name );
            }

            index.Flush ( );

            index.Close ( );
        }

        public Data GetData ( string name )
        {
            foreach ( DataEntry entry in Data )
            {
                if ( entry.Name == name )
                {
                    if ( entry.Loaded == false )
                    {
                        OpenData ( );

                        DataFile.Seek ( entry.Begin , SeekOrigin.Begin );
                        byte [ ] dat = DataR.ReadBytes(entry.Size);

                        CloseData ( );

                        entry.Loaded = true;
                        entry.Data = Activator.CreateInstance ( Type.GetType ( "DataCore.DataTypes." + entry.Type ) ) as Data;
                        entry.Data.RawData = dat;
                        entry.Data.Reconstruct ( );
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

        public void AddData ( Data data , string name )
        {
            DataEntry entry = new DataEntry();
            if ( data.Generated == false )
            {
                data.GenerateBytes ( );
            }

            entry.Name = name;
            entry.Size = data.Bytes;
            entry.Loaded = true;
            entry.Compressed = false;
            entry.Type = data.GetType ( ).Name;
            Console.WriteLine ( "Type:" + entry.Type );
            entry.Data = data;

            Data.Add ( entry );
            RebuildStarts ( );
        }

        public void RebuildStarts ( )
        {
            int pos = 0;

            foreach ( DataEntry entry in Data )
            {
                entry.Begin = pos;
                pos += entry.Size;
            }
        }
    }
}