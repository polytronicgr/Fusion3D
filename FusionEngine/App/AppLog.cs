using System;

namespace Fusion3D.App
{
    public static class AppLog
    {
        public static void Log ( string msg, string area = "" )
        {
            Console.WriteLine ( msg + "@" + area );
        }
    }
}