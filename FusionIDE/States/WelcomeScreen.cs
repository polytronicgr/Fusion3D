﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D;
using Vivid3D.Resonance.Forms;
using Vivid3D.Resonance;
using Vivid3D.State;
using FusionIDE.Forms;
namespace FusionIDE.States
{
    public class WelcomeScreen : VividState
    {
        public Vivid3D.Texture.VTex2D bg_img;
        public ImageForm BG = null;
        public WelcomeForm MainForm = null;
        public WelcomeScreen()
        {

            SUI = new UI();
            bg_img = new Vivid3D.Texture.VTex2D("data/ui/skin/windowbg1.png", Vivid3D.Texture.LoadMethod.Single, true);
            BG = (ImageForm)new ImageForm().Set(0, 0, Vivid3D.App.AppInfo.W, Vivid3D.App.AppInfo.H);
            BG.SetImage(bg_img);
            MainForm = (WelcomeForm)new WelcomeForm().Set(350, 200, Vivid3D.App.AppInfo.W - 700, 250, "Welcome to Fusion");
            SUI.Root.Add(BG);
            SUI.Top = MainForm;
            MainForm.Create = (user, pass) =>
            {

                Console.WriteLine("Creating new account. User:" + user + " Pass:" + pass);

            };

        }

        public override void InitState()
        {
            //            base.InitState();
            Console.WriteLine("Welcome to Fusion.");
        }

        public override void UpdateState()
        {
            SUI.Update();
        }

        public override void DrawState()
        {
            //base.DrawState();

            SUI.Render();

        }


    }
}
