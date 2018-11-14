using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion3D.Resonance;
using Fusion3D.Resonance.Forms;
using Fusion3D.Texture;
using Fusion3D.App;
using Fusion3D.State;
using FusionIDE.Forms;
namespace FusionIDE.States
{
    public class CodeScreen : FusionState
    {
        public CodeEditorForm EditWin = null;
        public override void InitState()
        {
            SUI = new UI();

            var code_bg = new ImageForm().Set(0, 0, AppInfo.W, AppInfo.H, "").SetImage(new Texture2D("data/ui/codebg1.jpg", LoadMethod.Single, false));

            EditWin = (CodeEditorForm)new CodeEditorForm().Set(10, 50, AppInfo.W - 20, AppInfo.H - 60, "Fusion - Code Editor");

            code_bg.Add(EditWin);

            SUI.Root.Add(code_bg);

            script.Scripts.ScanMods();

            base.InitState();
        }

        public override void UpdateState()
        {
            SUI.Update();
            base.UpdateState();
        }

        public override void DrawState()
        {

            SUI.Render();
            base.DrawState();
        }

    }
}
