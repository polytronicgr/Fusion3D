using FusionEngine.App;
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

            void InitIDE(int w,int h,int fs)
            {
                App = new FusionIDE(w, h, fs == 1);
                InitState = new States.CodeScreen();

                App.Run();
            }

            CFuncLink initIDE = new CFuncLink
            {
                Link = (t) =>
                {
                    InitIDE(t[0], t[1], t[2]);
                    return null;
                }
            };
            MainVM.AddCFunc("InitIDE", initIDE);
            CodeScope scope = MainVM.RunEntry ( );

   

          
        }
       
        public FusionIDE ( int width, int height, bool full_screen ) : base ( "FusionIDE", width, height, full_screen )
        {
        }
    }
}