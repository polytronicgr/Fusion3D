using System;
using System.IO;
using DataCore;
using DataCore.DataTypes;

namespace ProjectCore
{
    public class Project
    {
        public static string ProjectPath = "c:\\projs\\";

        public DataIO ProjData = null;
        public string BasePath { get; set; }
        public string ContentPath { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Author { get; set; }
        public string IDEPath { get; set; }
        public System.Drawing.Bitmap Icon { get; set; }

        public Project(string path)
        {
            IDEPath = path;
            BasePath = ProjectPath + path + "\\";
            ContentPath = BasePath + "Content\\";
            ProjData = new DataIO(BasePath + "projectData");

            dynamic name = ProjData.GetData("Name");
            dynamic info = ProjData.GetData("Info");
            dynamic author = ProjData.GetData("Author");
            dynamic idepath = ProjData.GetData("IDEPath");
            dynamic icon = ProjData.GetData("Icon");
            Name = name.String;
            Info = info.String;
            Author = author.String;
            IDEPath = idepath.String;
            Icon = icon.Map;

            



        }

        public Project(string name,string info,string author,System.Drawing.Bitmap icon)
        {

            IDEPath = name;
            Directory.CreateDirectory(ProjectPath + name);
            Directory.CreateDirectory(ProjectPath + name + "\\Content\\");
            
            BasePath = ProjectPath + name + "\\";
            ContentPath = BasePath + "Content\\";

            ProjData = new DataIO(BasePath + "projectData");

            ProjData.AddData(new DataString(name), "Name");
            ProjData.AddData(new DataString(info), "Info");
            ProjData.AddData(new DataString(author), "Author");
            ProjData.AddData(new DataString(IDEPath), "IDEPath");
            ProjData.AddData(new DataBitmap(icon), "Icon");

            ProjData.Save();


        }

    }
}
