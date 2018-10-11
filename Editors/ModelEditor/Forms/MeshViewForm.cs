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
using Vivid3D.Sound;
using Vivid3D.Reflect;
using System.Reflection;
using Vivid3D.Archive;
using Vivid3D.Lighting;
using Vivid3D.PostProcess;
using Vivid3D.Import;
using Vivid3D.Material;
using Vivid3D.State;
using Vivid3D.Texture;
using Vivid3D.Logic;
using Vivid3D.Resonance;
using Vivid3D.App;
using OpenTK;
using Vivid3D.ParticleSystem;
using Vivid3D.PostProcess.Processes;
using Vivid3D.Resonance.Forms;
using Vivid3D.Physics;
namespace ModelEditor.Forms
{
    public class MeshViewForm : WindowForm
    {
        public GraphEntity3D Ent;
        public GraphCam3D Cam = null;
        public GraphLight3D Light = null;
        public SceneGraph3D Scene = null;
        public override void DesignUI()
        {
            LockedPos = true;
            LockedSize = true;
            var sceneDis = new Graph3DForm().Set(0, 25, W, H);
            Console.WriteLine("W:" + W + " H:" + H);
            Scene = new SceneGraph3D();
            Scene.Add(Light);
            Scene.Add(Cam);

        }
        public void SetMesh(string mesh)
        {

            Ent = Import.ImportNode(mesh) as GraphEntity3D;
            Scene.Add(Ent);

        }

    }
}
