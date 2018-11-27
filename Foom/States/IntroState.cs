using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionEngine;
using FusionEngine.Resonance;
using FusionEngine.Resonance.Forms;
using FusionEngine.State;
using FusionEngine.App;
using FusionEngine.Audio;
using FusionEngine.Texture;

namespace Foom.States
{
    public class IntroState : FusionState
    {
        public FusionEngine.Audio.VSoundSource MenuSongSrc;
        public VSound MenuSongSound;
        public override void InitState()
        {
            base.InitState();
            MenuSongSrc = new VSoundSource("Foom/Song/intro1.mp3");
            MenuSongSound = MenuSongSrc.Play2D(true);

            var img = new ImageForm().Set(300, 800, 300, 300, "").SetImage(new Texture2D("Foom/Img/Intro/img1.jpg", LoadMethod.Single, true));
            UI.BootAlpha = 0.0f;
            int tc = 0;

            void ImgUp1()
            {
                img.Y = img.Y - 3;
            }

            bool ImgUpR()
            {
                if (img.Y < 200)
                {
                    return true;
                }
                return false;
            }

            void Img1Text()
            {

            }

            bool Img1TextD()
            {
                if(Environment.TickCount>(tc+3000))
                {
                    return true;
                }
                return false;
            }

            //int tc = 0;


            void FadeUi1()
            {
               
            }

            void UI3()
            {

            }

            bool UI3D()
            {
                if (Environment.TickCount > (tc + 3500))
                {
                    return true;
                }
                return false;
            }

            void UI4S()
            {

                SUI.Root.Forms.Clear();

                var img1 = new ImageForm().Set(50, 50, 600, 600).SetImage(new Texture2D("Foom/Img/Intro/img3.jpg", LoadMethod.Single, false));
                var tex1 = new ImageForm().Set(100, 300, 600, 250).SetImage(new Texture2D("Foom/Img/Intro/text2.png", LoadMethod.Single, true));

                SUI.Root.Forms.Add(img1);
                SUI.Root.Add(img1);
                img1.Add(tex1);
                UI.TarAlpha = 1.0f;

            }

            bool FadeDone1()
            {
                if (Environment.TickCount > (tc + 3500))
                {
                    UI.TarAlpha = 0.0f;
                    if (UI.BootAlpha < 0.01f)
                    {
                        UI.TarAlpha = 1.0f;    
                        return true;
                    }
                }
                return false;
            }

            void UI3S()
            {
                tc = Environment.TickCount;
                SUI.Root.Forms.Clear();

                var img2 = new ImageForm().Set(100, 100, 400, 400).SetImage(new Texture2D("Foom/Img/Intro/img2.jpg", LoadMethod.Single, true));
                SUI.Root.Add(img2);
                var log1 = new ImageForm().Set(220, 350, 500, 250).SetImage(new Texture2D("Foom/Img/Intro/text1.png", LoadMethod.Single, true));
                SUI.Root.Add(log1);
                Logics.When(FadeDone1, UI4S);

                UI.TarAlpha = 1.0f;
                Logics.Do(UI3, UI3D);
            }

           // bool UI3D()
           // {
            //    return false;
            //}


            bool FadeUID()
            {
                if (UI.BootAlpha < 0.01f)
                {
                   
                    return true;
                }
                return false;
            }

            void FadeUI()
            {
                UI.TarAlpha = 0.0f;
                Logics.Do(FadeUi1, FadeUID, UI3S);
            //    UI.BootAlpha += (0.0f - UI.BootAlpha) * 0.1f;
            }


            void NextImg()
            {
                tc = Environment.TickCount;
                Logics.Do(Img1Text, Img1TextD,FadeUI);

                var text = new LabelForm().Set(600, 250, 200, 80, "The Dawn Of Hell...");
                SUI.Root.Forms.Add(text);

            }

            Logics.Do(ImgUp1, ImgUpR,NextImg);



            SUI = new UI();

            SUI.Root.Add(img);

        }
        public override void UpdateState()
        {
            base.UpdateState();
            SUI.Update();
        }

        public override void DrawState()
        {
            base.DrawState();
            SUI.Render();
        }


    }
}
