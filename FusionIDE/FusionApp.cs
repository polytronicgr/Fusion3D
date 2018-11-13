using Vivid3D.App;
using VividScript;

namespace FusionIDE.App
{
    public class FusionApp : VividApp
    {
        public static VME MainVM = null;
        public static FusionApp App = null;

        public static void InitFusion ( )
        {
           

            MainVM = new VME ( );

            VSource init_ide_src = new VSource("data/script/ide/ide_init.vs");
            VCompiler comp = new VCompiler();
            VCompiledSource init_ide_compiled = comp.Compile(init_ide_src);

            MainVM.SetEntry ( init_ide_compiled.EntryPoint );

            CodeScope scope = MainVM.RunEntry ( );

            dynamic width = scope.FindVar("app_width",false).Value;
            dynamic height = scope.FindVar("app_height",false).Value;
            dynamic full_screen = scope.FindVar("full_screen",false).Value;

            App = new FusionApp ( width, height, full_screen );
            InitState = new States.CodeScreen();
            App.Run ( );
        }

        public FusionApp ( int width, int height, bool full_screen ) : base ( "FusionIDE", width, height, full_screen )
        {
        }
    }
}