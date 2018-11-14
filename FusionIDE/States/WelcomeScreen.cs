using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion3D;
using Fusion3D.Resonance.Forms;
using Fusion3D.Resonance;
using Fusion3D.State;
using FusionIDE.Forms;
namespace FusionIDE.States
{
    public class WelcomeScreen : FusionState
    {
        public Fusion3D.Texture.Texture2D bg_img;
        public ImageForm BG = null;
        public WelcomeForm MainForm = null;
        public BackgroundForm BGForm = null;
        public Fusion3D.Composition.Composite Com;
            public Fusion3D.Composition.Compositers.BloomUICompositer BloomUI;
        public WelcomeScreen()
        {

           

            SUI = new UI();
            bg_img = new Fusion3D.Texture.Texture2D("data/ui/skin/windowbg1.png", Fusion3D.Texture.LoadMethod.Single, true);
            BG = (ImageForm)new ImageForm().Set(0, 0, Fusion3D.App.AppInfo.W, Fusion3D.App.AppInfo.H);
            BG.SetImage(bg_img);
            MainForm = (WelcomeForm)new WelcomeForm().Set(450, 200, Fusion3D.App.AppInfo.W - 900, 250, "Welcome to Fusion");
            BGForm = (BackgroundForm)new BackgroundForm(20).Set(0, 0, Fusion3D.App.AppInfo.W, Fusion3D.App.AppInfo.H);
            var bgi = new ImageForm().Set(0, 0, Fusion3D.App.AppInfo.W, Fusion3D.App.AppInfo.H, "");
            bgi.SetImage(new Fusion3D.Texture.Texture2D("data/ui/bg1.jpg",Fusion3D.Texture.LoadMethod.Single,false));
            bgi.Add(BGForm);
            SUI.Root.Add(bgi);

            BGForm.Add(MainForm);
          //  SUI.Top = MainForm;
            MainForm.Create = (user, pass) =>
            {

                Console.WriteLine("Creating new account. User:" + user + " Pass:" + pass);

            };

            Com = new Fusion3D.Composition.Composite();
            BloomUI = new Fusion3D.Composition.Compositers.BloomUICompositer();
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
            new Fusion3D.Audio.VSoundSource("data/audio/bootup2.wav").Play2D(false);
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
