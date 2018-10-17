using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
namespace VividEdit.Forms
{
    public class ContentBase
    {

        public string Name { get; set; }
        public Bitmap Icon { get; set; }
        public string Path { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Type = "";
        public string BasePath = "";
        public void DetermineType()
        {
            string ext = new FileInfo(Path).Extension.ToLower();
            switch (ext)
            {
                case ".3ds":
                case ".obj":
                case ".blend":
                case ".fbx":
                case ".gltf":
                    Type = "3D";
                    break;
                case ".bmp":
                case ".jpg":
                case ".png":
                case ".tga":
                case ".gif":
                    Type = "Image";
                    break;
            }
        }

    }
}
