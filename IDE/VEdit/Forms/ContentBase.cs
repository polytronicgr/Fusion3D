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

        public void DetermineType ( )
        {
            string ext = new FileInfo(Path).Extension.ToLower();
            switch ( ext )
            {
                case ".cs":
                    Type = "Code";
                    break;

                case ".v3dm":
                    Type = "V3D";
                    break;

                case ".3ds":
                case ".obj":
                case ".blend":
                case ".fbx":
                case ".gltf":
                case ".x":
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