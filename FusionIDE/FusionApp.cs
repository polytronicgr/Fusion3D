using Fusion3D.App;
using FusionScript;

namespace FusionIDE.App
{
    public class FusionIDE : FusionApp
    {
        public static ManagedHost MainVM = null;
        public static FusionApp App = null;

        public static void InitFusion ( )
        {
           

            MainVM = new ManagedHost ( );

            ScriptSource init_ide_src = new ScriptSource("data/script/ide/ide_init.vs");
            Compiler comp = new Compiler();
            CompiledSource init_ide_compiled = comp.Compile(init_ide_src);

            MainVM.SetEntry ( init_ide_compiled.EntryPoint );

            CodeScope scope = MainVM.RunEntry ( );

            dynamic width = scope.FindVar("app_width",false).Value;
            dynamic height = scope.FindVar("app_height",false).Value;
            dynamic full_screen = scope.FindVar("full_screen",false).Value;

            App = new FusionIDE ( width, height, full_screen );
            InitState = new States.CodeScreen();

            App.Run ( );
        }

        public FusionIDE ( int width, int height, bool full_screen ) : base ( "FusionIDE", width, height, full_screen )
        {
        }
    }
}