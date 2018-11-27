using OpenTK;
using System;
using FusionEngine.Draw;
using FusionEngine.Texture;

namespace FusionEngine.Resonance.Forms
{
    public class ButtonForm : UIForm
    {
        private bool Pressed = false, Over = false;
        private Vector4 NormCol = new Vector4(0.9f, 0.9f, 0.9f, 0.9f);
        private Vector4 OverCol = new Vector4(0.8f, 0.8f, 0.8f, 0.9f);
        private Vector4 PressCol = new Vector4(1, 1, 1, 1);
        public static Audio.VSoundSource BeepSound;
        public ButtonForm ( )
        {
           if (BeepSound == null)
          {
                BeepSound = new Audio.VSoundSource("data/audio/button1.wav");
            }
            
                SetImage ( new Texture2D ( "data/ui/skin/but_normal.png", LoadMethod.Single, true ) );
            Col = NormCol;

            void DrawFunc ( )
            {
                Pen2D.BlendMod = PenBlend.Alpha;

                DrawForm ( CoreTex );
                DrawText ( Text, W / 2 - UI.Font.Width ( Text ) / 2, H / 2 - UI.Font.Height ( ) / 2,Col );
            }

            void MouseEnterFunc ( )
            {
                if ( Pressed == false )
                {
                    Col = OverCol;
                }
                Over = true;
            }

            void MouseLeaveFunc ( )
            {
                if ( Pressed == false )
                {
                    Col = NormCol;
                }
                Over = false;
            }

            void MouseMoveFunc ( int x, int y, int dx, int dy )
            {
                if ( Pressed )
                {
                    // Drag?.Invoke(dx, dy);
                }
            }

            void MouseDownFunc ( int b )
            {
                Col = PressCol;
                Pressed = true;
               var s = BeepSound.Play2D();
            
            }

            void MouseUpFunc ( int b )
            {
                if ( Over )
                {
                    Col = OverCol;
                }
                else
                {
                    Col = NormCol;
                }
                Pressed = false;
                Console.WriteLine ( "CLicked!" );
                if ( Click != null )
                {
                    Console.WriteLine ( "Has click" );
                }
                Click?.Invoke ( b );
            }

            Draw = DrawFunc;
            MouseEnter = MouseEnterFunc;
            MouseLeave = MouseLeaveFunc;
            MouseMove = MouseMoveFunc;
            MouseDown = MouseDownFunc;
            MouseUp = MouseUpFunc;
        }
    }
}