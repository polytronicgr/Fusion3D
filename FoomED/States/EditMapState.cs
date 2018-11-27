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
using System.IO;
using FusionEngine.FrameBuffer;
namespace FoomED.States
{
    public class EditMapState : FusionState
    {
        public string MapName = "";
        public FrameBufferColor EditFB;
        public EditMapState(string name)
        {
            MapName = name;
            EditFB = new FrameBufferColor(1024, 1024);

        }
        public override void InitState()
        {
            base.InitState();
            SUI = new UI();

            var bg = new ImageForm().Set(0, 0, AppInfo.W, AppInfo.H).SetImage(new Texture2D("FoomED/bg2.jpg", LoadMethod.Single, false));

            SUI.Root.Add(bg);

            var edit_win = new Forms.EditMapForm().Set(30, 200, AppInfo.W - 80, AppInfo.H - 250, "Edit:" + MapName);

            bg.Add(edit_win);

            var tool_win = new WindowForm().Set(30, 10, 600, 120, "Tools");

            bg.Add(tool_win);


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
