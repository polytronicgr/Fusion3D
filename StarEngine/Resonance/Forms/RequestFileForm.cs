using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using Vivid3D.Draw;
using OpenTK;
using System.IO;
using Vivid3D.App;
namespace Vivid3D.Resonance.Forms
{
    public delegate void SelectFile(string pth);
    public class RequestFileForm : WindowForm 
    {

        public SelectFile Selected = null;
        public ListForm Files;
        public VTex2D FolderPic = new VTex2D("Data\\UI\\folder1.png", LoadMethod.Single, true);
        public VTex2D FilePic = new VTex2D("Data\\UI\\file1.png", LoadMethod.Single, true);
        public ButtonForm BackFolder;
        public TextBoxForm DirBox, FileBox;
        public RequestFileForm(string title="",string defdir="")
        {
            LockedSize = true;
            DirBox = new TextBoxForm().Set(55, 35, 300, 20) as TextBoxForm;
            FileBox = new TextBoxForm().Set(10, 415, 300, 20) as TextBoxForm;
            Add(DirBox);
            Add(FileBox);

            var cancel = new ButtonForm().Set(10, 450, 120, 20, "Cancel");
            var ok = new ButtonForm().Set(180, 450, 120, 20, "Select");

            void SelectFunc(int b)
            {

                Selected?.Invoke(DirBox.Text + "/" + FileBox.Text);

            }

            ok.Click = SelectFunc;

            Add(cancel);
            Add(ok);

            if (defdir == "")
            {
                defdir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);              
            }

            if (title == "") title = "Select file";
            Set(VividApp.W / 2 - 200, VividApp.H / 2 - 250, 400, 500, title);

            Files = new ListForm().Set(10, 60, 370, 350, "") as ListForm;
            Add(Files);
            Scan(defdir);
            BackFolder = new ButtonForm().Set(0, 25, 64, 32, "").SetImage(new VTex2D("Data\\UI\\backfolder1.png", LoadMethod.Single, true)) as ButtonForm;

            void BackFunc(int b)
            {

                if (new DirectoryInfo(CurPath).Parent == null) return;
                string curPath = new DirectoryInfo(CurPath).Parent.FullName;
                Forms.Remove(Files);
                Files = new ListForm().Set(10, 60, 370, 350, "") as ListForm;
               
                Add(Files);

                Scan(curPath);
            }

            BackFolder.Click = BackFunc;
            Add(BackFolder);


        }
        public List<string> LastPath = new List<string>();
        public string CurPath = "";
        public void Scan(string folder)
        {
            if (new DirectoryInfo(folder).Exists == false) return;
            DirBox.Text = new DirectoryInfo(folder).FullName;
            // LastPath.Add(folder);
            CurPath = folder;
            var di = new DirectoryInfo(folder);
            foreach(var fold in di.GetDirectories())
            {
                var ni=Files.AddItem(fold.Name, FolderPic);
                void DoubleClickFunc(int b)
                {
                    Console.WriteLine("Scanning:" + fold.FullName);
                    Forms.Remove(Files);
                    Files = new ListForm().Set(10, 60, 370, 350, "") as ListForm;
                    Add(Files);
                    Scan(fold.FullName);
                 //   Files.Changed?.Invoke();
                }
                ni.DoubleClick = DoubleClickFunc;

            }
            foreach(var file in di.GetFiles())
            {
                var newi = Files.AddItem(file.Name, FilePic);
                void ClickFunc(int b)
                {
                    FileBox.Text = file.Name;
                }
                void DoubleClickFunc(int b)
                {
                    Selected?.Invoke(DirBox.Text + "/" + newi.Text);
                }
                newi.DoubleClick = DoubleClickFunc;
                newi.Click = ClickFunc;
            }
            Files.Changed?.Invoke();
        }

    }
}
