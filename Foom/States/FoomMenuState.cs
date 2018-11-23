using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion3D;
using Fusion3D.Resonance;
using Fusion3D.Resonance.Forms;
using Fusion3D.State;
using Fusion3D.App;
using Fusion3D.Audio;
using Fusion3D.Texture;
namespace Foom.States
{
    public class FoomMenuState : FusionState
    {

        public Fusion3D.Audio.VSoundSource MenuSongSrc;
        public VSound MenuSongSound;
        public override void InitState()
        {
            base.InitState();

            MenuSongSrc = new VSoundSource("Foom/Song/menu1.mp3");
           //e2
            MenuSongSound = MenuSongSrc.Play2D(true);


            SUI = new Fusion3D.Resonance.UI();

            var TitleBG = new ImageForm().Set(0, 0, AppInfo.W, AppInfo.H).SetImage(new Texture2D("Foom/Img/titlebg1.jpg", LoadMethod.Single, false));

            var foomLab = new ImageForm().Set(AppInfo.W / 2 - 350, 40, 700, 356).SetImage(new Texture2D("Foom/Img/foom1.png", LoadMethod.Single, true));

            TitleBG.Add(foomLab);

            var StartGame = new ButtonForm().Set(AppInfo.W/2-120, 380, 260, 40, "Begin...");
            var ExitGame = new ButtonForm().Set(AppInfo.W/2-120, 430, 260, 40, "Leave...");

            TitleBG.Add(StartGame);
            TitleBG.Add(ExitGame);

            SUI.Root.Add(TitleBG);


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
