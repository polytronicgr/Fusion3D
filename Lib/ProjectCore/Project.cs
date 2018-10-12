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
        public string Name { get; set; }
        public string Info { get; set; }
        public string Author { get; set; }
        

        public Project(string path)
        {
            BasePath = ProjectPath + path + "\\";

            ProjData = new DataIO(BasePath + "projectData");

            dynamic name = ProjData.GetData("Name");
            dynamic info = ProjData.GetData("Info");
            dynamic author = ProjData.GetData("Author");
            Name = name.String;
            Info = info.String;
            Author = author.String;
            



        }

        public Project(string name,string info,string author)
        {

            Directory.CreateDirectory(ProjectPath + name);
            BasePath = ProjectPath + name + "\\";

            ProjData = new DataIO(BasePath + "projectData");

            ProjData.AddData(new DataString(name), "Name");
            ProjData.AddData(new DataString(info), "Info");
            ProjData.AddData(new DataString(author), "Author");

            ProjData.Save();


        }

    }
}
