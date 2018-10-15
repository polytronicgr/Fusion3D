using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Vivid3D.Archive
{
    public class VirtualFileSystem
    {
        public string Name
        {
            get;
            set;
        }
        public string Path
        {
            get;
            set;
        }

        public List<VirtualEntry> Enteries = new List<VirtualEntry>();

        public long Size
        {
            get
            {
                long s = 0;
                foreach (var e in Enteries)
                {
                    s += e.Size;
                }
                return s;
            }

        }

        public VirtualEntry Find(string name, string path)
        {

            foreach (var entry in Enteries)
            {
                if (entry.Name == name && entry.Path == path)
                {
                    return entry;
                }
            }
            return null;
        }
        public void ReadToc(string path)
        {
            path = path.Replace(".toc", "");
            arcpath = path + ".vfs";
            path = path + ".toc";
            //Console.WriteLine("Opening TOC:" + path);
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            //Console.WriteLine("Opened. Parsing table.");
            int ec = r.ReadInt32();
            Console.WriteLine("Enteries:" + ec);
            for (int i = 0; i < ec; i++)
            {
                var ne = new VirtualEntry();
                for(int i2 = 0; i2 < 16; i2++)
                {
                    ne.Par[i2] = r.ReadInt32();
                }
                ne.Name = r.ReadString();
                ne.Path = r.ReadString();
                ne.Compressed = r.ReadBoolean();
           //     Console.WriteLine("Name:" + ne.Name + " Path:" + ne.Path + " Compressed?" + ne.Compressed);
                //      ne.ImgW = r.ReadInt32();
                //       ne.ImgH = r.ReadInt32();
                ne.Start = r.ReadInt64();
                ne.Size = r.ReadInt64();
                Enteries.Add(ne);
            }

        }
        public VirtualEntry Load(VirtualEntry e)
        {
            //name = name.ToLower();
            //path = path.ToLower();


            if (e.Loaded == false)
            {
                FileStream fs = new FileStream(arcpath, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);
                e.RawData = new byte[(int)e.Size];
                fs.Seek(e.Start, SeekOrigin.Begin);
                r.Read(e.RawData, 0, (int)e.Size);
                if (e.Compressed)
                {
                    byte[] od;
                    ZLib.DecompressData(e.RawData, out od);
                    e.RawData = od;
                    fs.Close();
                    e.Size = od.Length;
                    e.Compressed = false;
                }

                r = null;
                fs = null;
                e.Loaded = true;
            }
            return e;


        }
    

        public void Add(VirtualEntry entry)
        {
            Enteries.Add(entry);
        }
        public void AddMediaFromNode(Scene.GraphNode node)
        {
            if (node.ImgFrame != null)
            {
                bool found = false;
                foreach (var e in Enteries)
                {
                    if (e.Name == node.ImgFrame.Name && e.Path == node.ImgFrame.Path)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    var ve = new VirtualEntry();
                    ve.Name = node.ImgFrame.Name;
                    ve.Path = node.ImgFrame.Path;
                    ve.RawData = node.ImgFrame.RawData;
                    ve.Size = ve.RawData.Length;
                    ve.Type = EntryType.Index;
                    ve.Par[0] = node.ImgFrame.Width;
                    ve.Par[1] = node.ImgFrame.Height;
                    ve.Par[2] = node.ImgFrame.Alpha ? 1 : 0;
                    Console.WriteLine("N:" + node.ImgFrame.Width + " H:" + node.ImgFrame.Height + " Alpha:" + node.ImgFrame.Alpha);
                    ve.Loaded = true;
                    ve.Compressed = false;
                    Enteries.Add(ve);
                }
            }
            foreach(var n2 in node.Nodes)
            {
                AddMediaFromNode(n2);
            }
        }
        public void Add(Scene.SceneGraph graph)
        {

            AddMediaFromNode(graph.Root);

            var ge = new VirtualEntry();
            ge.Path = "VirtualFile";
            ge.Name = graph.Root.Name;

            var ms = new MemoryStream(1024 * 1024);
            BinaryWriter w = new BinaryWriter(ms);

            graph.WriteGraph(w);

            var wd = new byte[ms.Position];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(wd, 0, wd.Length);
            Console.WriteLine("GraphSize:" + wd.Length + " bytes.");
            ge.RawData = wd;
            ge.Size = wd.Length;
            ms.Flush();
            w.Flush();
            ms.Dispose();
            ge.Loaded = true;
            ge.Compressed = false;
            Enteries.Add(ge);





        }
        public void Add(string path)
        {
          
            var fi = new FileInfo(path);

            var ext = fi.Extension.ToLower();
            var qn = fi.Name.Replace(ext, "").ToLower();
            switch (ext)
            {
                case ".bmp":
                case ".jpg":
                case ".png":
                    var na = new VirtualEntry();

                    System.Drawing.Bitmap tb = new System.Drawing.Bitmap(path);

                    var rd = new byte[tb.Width * tb.Height * 4];
                    int bi = 0;
                    for(int y = 0; y < tb.Height; y++)
                    {
                        for(int x = 0; x < tb.Width; x++)
                        {
                            var pix = tb.GetPixel(x, y);
                            rd[bi++] = pix.R;
                            rd[bi++] = pix.G;
                            rd[bi++] = pix.B;
                            rd[bi++] = pix.A;
                        }
                    }

                    na.RawData = rd;
                    na.Par[0] = tb.Width;
                    na.Par[1] = tb.Height;
                    na.Par[2] = 1;
                    tb.Dispose();

                    na.Size = na.RawData.Length;
                    na.Name = qn;
                    na.Path = path;
                    Enteries.Add(na);
                    break;
                case ".cs":

                    var sb = File.ReadAllBytes(path);

                    var ve = new VirtualEntry();
                    ve.RawData = sb;
                    ve.Size = sb.Length;
                    ve.Name = qn;
                    ve.Path = path;
                    Enteries.Add(ve);
                    break;
                case ".graph":
                    var gb = File.ReadAllBytes(path);
                    var e2 = new VirtualEntry();
                    e2.RawData = gb;
                    e2.Size = gb.Length;
                    e2.Name = qn;
                    e2.Path = path;
                    Enteries.Add(e2);
                    break;
                              
            }

        }
        public void LinkGraphImg(Scene.GraphNode node)
        {
            foreach(var img in Enteries)
            {
                if(img.Path == node.ImgLinkName)
                {
                    if(img.Loaded == false)
                    {
                        Load(img);
                    }
                    node.ImgFrame = new Tex.Tex2D(img, true);
                }
            }
            foreach(var n in node.Nodes)
            {
                LinkGraphImg(n);
            }
        }
        public Scene.SceneGraph GetGraph(string name)
        {
            Console.WriteLine("Searching for graph:" + name);
            foreach(var e in Enteries)
            {
                Console.WriteLine("E:" + e.Name + " Start:" + e.Start + " Size:" + e.Size + " P:" + e.Path);
                if (e.Name == name) 
                {
                    Console.WriteLine("Found Graph:" + name);
                    var ng = new Scene.SceneGraph();
                    Load(e);
                    var ms = new MemoryStream(e.RawData);
                    BinaryReader r = new BinaryReader(ms);
                 //   Console.WriteLine("Reading Graph.");
                    ng.ReadGraph(r);
                  //  Console.WriteLine("Read.");
                    ms.Close();
                    r = null;
                    ms = null;

                    LinkGraphImg(ng.Root);
                    return ng;


                }
            }
            return null;
        }
       public Tex.Tex2D GetTex(string name)
        {
            var e = GetEntry(name);
            Load(e);
            return new Tex.Tex2D(e, true);

        }

        public VirtualEntry GetEntry(string name)
        {
            name = name.ToLower();
            foreach(var e in Enteries)
            {
                if(e.Name.ToLower() == name)
                {
                    return e;
                }
            }
            return null;
        }
        public void Update(string path)
        {
            arcpath = path + "arc.vfs";
            tocpath = path + "arc.toc";
            if (new FileInfo(path + "arc.toc").Exists)
            {
                ReadToc(path + "arc.toc");
             
            }
            else
            {
                ScanFolder(path);
                SaveTOC(path);
                SaveFS(path);
            }
        }
        string arcpath = "";
        string tocpath = "";
        public void Save(string name,bool compressed)
        {
            if (compressed)
            {
                foreach (var v in Enteries)
                {
                    if (v.Compressed == false)
                    {
                        int ol = v.RawData.Length;
                        byte[] od = null;
                        ZLib.CompressData(v.RawData, out od);
                        v.RawData = od;
                        v.Size = od.Length;
                        v.Compressed = true;
                        int nl = v.RawData.Length;
                        Console.WriteLine("E:" + v.Name + " Old:" + ol + " New:" + nl);
                    }
                }

            }
            SaveTOC(name);
            SaveFS(name);
        }
        public void SaveFS(string path)
        {


           
            FileStream fs = new FileStream(path + ".vfs", FileMode.Create, FileAccess.Write);
            BinaryWriter w = new BinaryWriter(fs);

            foreach(var e in Enteries)
            {

                w.Write(e.RawData);
                
            }

            fs.Flush();
            fs.Close();
            w = null;
            fs = null;



        }
        public void SaveTOC(string path)
        {

            FileStream fs = new FileStream(path+".toc", FileMode.Create, FileAccess.Write);
            BinaryWriter w = new BinaryWriter(fs);


            long start = 0;
            w.Write(Enteries.Count);
            foreach(var e in Enteries)
            {
                for(int i = 0; i < 16; i++)
                {
                    w.Write(e.Par[i]);
                }
                w.Write(e.Name);
                w.Write(e.Path);
                w.Write(e.Compressed);
                w.Write(start);
                w.Write(e.Size);
                
                start += e.Size;
            }
            fs.Flush();
            fs.Close();
            fs = null;
            w = null;



        }
        public void ScanFolder(string path)
        {

            var fl = new DirectoryInfo(path).GetFiles();
            var dl = new DirectoryInfo(path).GetDirectories();
            foreach(var file in fl)
            {
                var fi = new FileInfo(file.FullName);
                var fe = Find(fi.Name, path);
                if (fe != null)
                {
                    if(fe.Size!=fi.Length)
                    {
                        
                    }
                }
                else
                {
                    var entry = new VirtualFile();
                    entry.Name = fi.Name;
                    entry.Path = path;
                    entry.Size = fi.Length;
                    entry.RawData = File.ReadAllBytes(fi.FullName);
                    byte[] od = null;
                    int os = entry.RawData.Length;
                    ZLib.CompressData(entry.RawData, out od);
                    entry.RawData = od;
                    
                    entry.Size = entry.RawData.Length;
                    Console.WriteLine("Adding:" + entry.Name);
                    Enteries.Add(entry);
                }



            }
            foreach(var dir in dl)
            {
                ScanFolder(dir.FullName);
            }


        }

    }
    public class VirtualEntry
    {
        public EntryType Type
        {
            get;
            set;
        }
        public int[] Par = new int[16];
        public string Name
        {
            get;
            set;
        }
     
        public bool Compressed
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }
        public byte[] RawData
        {
            get;
            set;
        }
        public long Start
        {
            get;
            set;
        }
        public long Size
        {
            get;
            set;
        }
        public bool Loaded
        {
            get;
            set;
        }
        public VirtualEntry()
        {
            Loaded = false;
            Start = Size = 0;
            Size = 1;
            RawData = new byte[1];
            RawData[0] = 0;
            Name = "";
            Path = "";
            Compressed = false;
            Type = EntryType.Index;
        }
        public byte[] ToBytes()
        {
            return RawData;
        }
    }
    public class VirtualFile : VirtualEntry
    {
        
    }
    public enum EntryType
    {
        Index,Ref
    }
}
