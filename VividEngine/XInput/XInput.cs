using XInputDotNetPure;

namespace Vivid3D.XInput
{
    public static class XInput
    {
        public static void Init ( )
        {
        }

        public static void TestRumble ( )
        {
        }
    }

    public class XPad
    {
        private readonly int id = 0;
        private GamePadState state;

        public XPad ( int player )
        {
            id = player;
            Sync ( );
        }

        public bool Connected => state.IsConnected;

        public bool LeftShoulder => state.Buttons.LeftShoulder == ButtonState.Pressed;

        public float LeftTrigger => state.Triggers.Left;

        public float LeftX => state.ThumbSticks.Left.X;

        public float LeftY => state.ThumbSticks.Left.Y;

        public bool RightShoulder => state.Buttons.RightShoulder == ButtonState.Pressed;

        public float RightTrigger => state.Triggers.Right;

        public float RightX => state.ThumbSticks.Right.X;

        public float RightY => state.ThumbSticks.Right.Y;

        public void SetFB ( float l, float r )
        {
            GamePad.SetVibration ( PlayerIndex.One + id, l, r );
        }

        public void Sync ( )
        {
            state = GamePad.GetState ( PlayerIndex.One + id );
        }
    }
}