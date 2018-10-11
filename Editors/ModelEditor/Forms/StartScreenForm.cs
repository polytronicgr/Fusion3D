using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D;
using Vivid3D.App;
using Vivid3D.Draw;
using Vivid3D.FX;
using Vivid3D.FXS;
using Vivid3D.Input;
using Vivid3D.Scene;
using Vivid3D.Tex;
using Vivid3D.Util;
using Vivid3D.VFX;
using Vivid3D.Sound;
using Vivid3D.Reflect;
using System.Reflection;
using Vivid3D.Archive;
using Vivid3D.Lighting;
using Vivid3D.PostProcess;
using Vivid3D.Import;
using Vivid3D.Material;
using Vivid3D.State;
using Vivid3D.Texture;
using Vivid3D.Logic;
using Vivid3D.Resonance;
using Vivid3D.App;
using OpenTK;
using Vivid3D.ParticleSystem;
using Vivid3D.PostProcess.Processes;
using Vivid3D.Resonance.Forms;
using Vivid3D.Physics;
using ModelEditor.States;
namespace ModelEditor.Forms
{
    public class StartScreenForm : UIForm
    {

        public override void DesignUI()
        {

            VTex2D bgtex = new VTex2D("Data\\bg\\bg1.jpg", LoadMethod.Single, false);
           // VTex2D bgtex2 = new VTex2D("Data\\bg\\bg2.jpg", LoadMethod.Single, false);

            ImageForm bg = new ImageForm().Set(0, 0, VividApp.W, VividApp.H).SetImage(bgtex) as ImageForm;

            WindowForm win = new WindowForm().Set(0, 0, VividApp.W, VividApp.H, "Vivid3D - Model Editor - Start Screen") as WindowForm;

            win.LockedPos = false;
            win.LockedSize = true;

            PanelForm options = new PanelForm().Set(VividApp.W - 180, 20, 175, VividApp.H-20) as PanelForm;

            ButtonForm importMesh = new ButtonForm().Set(10, 20, 150, 25, "Import Mesh") as ButtonForm;

            options.Add(importMesh);

            win.Add(options);

            bg.Add(win);

            //ImageForm b2 = new ImageForm().Set(100, 100, 200, 200).SetImage(bgtex2).SetPeak(true, false) as ImageForm;
            //WindowForm b2 = new WindowForm().Set(300, 400, 200, 150/,"Test1") as WindowForm;


            RequestFileForm impFile = null;

            void ImportMeshFunc(int b)
            {
                switch (b)
                {
                    case 0:

                        void SelectFunc(string path)
                        {

                            Console.WriteLine("Import:" + path);
                            VividApp.ActiveState.SUI.Top = null;
                            EG.MeshPath = path;
                            var ns = new MeshEditScreen();
                            VividApp.PushState(ns);
                            ns.SetMesh(path);
                            return;


                        }

                        impFile = new RequestFileForm("Select a mesh to import");
                        impFile.Selected = SelectFunc;
                        VividApp.ActiveState.SUI.Top = impFile;
                        break;
                }
            }

            importMesh.Click = ImportMeshFunc;

            //bg.Add(b2);

            Forms.Add(bg);

        }

    }
}
