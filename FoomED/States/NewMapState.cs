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
using System.IO;
namespace FoomED.States
{
    public class NewMapState : FusionState
    {

        public override void InitState()
        {

            SUI = new UI();


            var bg = new ImageForm().Set(0, 0, AppInfo.W, AppInfo.H, "").SetImage(new Texture2D("FoomED/bg1.jpg", LoadMethod.Single, false));

            SUI.Root.Add(bg);

            var back = new ButtonForm().Set(5, 5, 80, 30, "Back");

            bg.Add(back);

            back.Click = (b) =>
            {

                FusionApp.PopState();
                
            };

            var lab = new LabelForm().Set(25, 80, 200, 30, "Map Title:");

            bg.Add(lab);

            TextBoxForm map_name = new TextBoxForm().Set(120, 75, 200, 30, "") as TextBoxForm;

            bg.Add(map_name);

            map_name.Enter = (txt) =>
            {

                Directory.CreateDirectory("Game/Maps/" + txt + "/");
                

            };



        }

        public override void UpdateState()
        {


            SUI.Update();
        }

        public override void DrawState()
        {

            SUI.Render();
        }
    }
}
