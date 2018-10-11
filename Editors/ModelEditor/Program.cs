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
namespace ModelEditor
{
    class Program
    {
        static void Main(string[] args)
        {

            var startScreen = new States.StartScreen();

            VividApp.InitState = startScreen;

            var app = new ModelEditorApp(800,600,false);

            app.Run(60, 60);

        }
    }
}
