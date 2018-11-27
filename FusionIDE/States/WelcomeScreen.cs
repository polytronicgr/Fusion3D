using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionEngine;
using FusionEngine.Resonance.Forms;
using FusionEngine.Resonance;
using FusionEngine.State;
using FusionIDE.Forms;
namespace FusionIDE.States
{
    public class WelcomeScreen : FusionState
    {
        public FusionEngine.Texture.Texture2D bg_img;
        public ImageForm BG = null;
        public WelcomeForm MainForm = null;
        public BackgroundForm BGForm = null;
        public FusionEngine.Composition.Composite Com;
            public FusionEngine.Composition.Compositers.BloomUICompositer BloomUI;
        public WelcomeScreen()
        {

           

            SUI = new UI();
            bg_img = new FusionEngine.Texture.Texture2D("data/ui/skin/windowbg1.png", FusionEngine.Texture.LoadMethod.Single, true);
            BG = (ImageForm)new ImageForm().Set(0, 0, FusionEngine.App.AppInfo.W, FusionEngine.App.AppInfo.H);
            BG.SetImage(bg_img);
            MainForm = (WelcomeForm)new WelcomeForm().Set(450, 200, FusionEngine.App.AppInfo.W - 900, 250, "Welcome to Fusion");
            BGForm = (BackgroundForm)new BackgroundForm(20).Set(0, 0, FusionEngine.App.AppInfo.W, FusionEngine.App.AppInfo.H);
            var bgi = new ImageForm().Set(0, 0, FusionEngine.App.AppInfo.W, FusionEngine.App.AppInfo.H, "");
            bgi.SetImage(new FusionEngine.Texture.Texture2D("data/ui/bg1.jpg",FusionEngine.Texture.LoadMethod.Single,false));
            bgi.Add(BGForm);
            SUI.Root.Add(bgi);

            BGForm.Add(MainForm);
          //  SUI.Top = MainForm;
            MainForm.Create = (user, pass) =>
            {

                Console.WriteLine("Creating new account. User:" + user + " Pass:" + pass);

            };

            Com = new FusionEngine.Composition.Composite();
            BloomUI = new FusionEngine.Composition.Compositers.BloomUICompositer();
            dynamic ui = BloomUI.InputFrame;
            ui.GUI = SUI;
            Com.AddCompositer(BloomUI);
            int t = System.Environment.TickCount + 8000;
            while (System.Environment.TickCount < t)
            {

            }
        }

        public override void InitState()
        {
            //            base.InitState();
            Console.WriteLine("Welcome to Fusion.");
            new FusionEngine.Audio.VSoundSource("data/audio/bootup2.wav").Play2D(false);
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
