using System;
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
        public BackgroundForm BGForm = null;
        public Vivid3D.Composition.Composite Com;
            public Vivid3D.Composition.Compositers.BloomUICompositer BloomUI;
        public WelcomeScreen()
        {

           

            SUI = new UI();
            bg_img = new Vivid3D.Texture.VTex2D("data/ui/skin/windowbg1.png", Vivid3D.Texture.LoadMethod.Single, true);
            BG = (ImageForm)new ImageForm().Set(0, 0, Vivid3D.App.AppInfo.W, Vivid3D.App.AppInfo.H);
            BG.SetImage(bg_img);
            MainForm = (WelcomeForm)new WelcomeForm().Set(350, 200, Vivid3D.App.AppInfo.W - 700, 250, "Welcome to Fusion");
            BGForm = (BackgroundForm)new BackgroundForm(20).Set(0, 0, Vivid3D.App.AppInfo.W, Vivid3D.App.AppInfo.H);
            var bgi = new ImageForm().Set(0, 0, Vivid3D.App.AppInfo.W, Vivid3D.App.AppInfo.H, "");
            bgi.SetImage(new Vivid3D.Texture.VTex2D("data/ui/bg1.jpg",Vivid3D.Texture.LoadMethod.Single,false));
            bgi.Add(BGForm);
            SUI.Root.Add(bgi);

            BGForm.Add(MainForm);
          //  SUI.Top = MainForm;
            MainForm.Create = (user, pass) =>
            {

                Console.WriteLine("Creating new account. User:" + user + " Pass:" + pass);

            };

            Com = new Vivid3D.Composition.Composite();
            BloomUI = new Vivid3D.Composition.Compositers.BloomUICompositer();
            dynamic ui = BloomUI.InputFrame;
            ui.GUI = SUI;
            Com.AddCompositer(BloomUI);

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
            Com.Render();
         //   SUI.Render();

        }


    }
}
