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

using Vivid3D.Reflect;
using System.Reflection;
using Vivid3D.Archive;
using Vivid3D.Lighting;
using Vivid3D.PostProcess;
using Vivid3D.Import;
using Vivid3D.Material;
using Vivid3D.State;
using ModelEditor.Forms;
namespace ModelEditor.States
{
    public class MeshEditScreen : VividState
    {
        public string MeshPath = "";
        public void SetMesh(string path)
        {
            MeshPath = path;
        }
        public override void InitState()
        {
            MeshPath = EG.MeshPath;
            InitUI();

            SUI.Root = new MeshEditForm().Set(0, 0, VividApp.W, VividApp.H);

            var me = SUI.Root as MeshEditForm;
            me.SetMesh(MeshPath);
            Console.WriteLine("Set Path:" + MeshPath);
            

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
