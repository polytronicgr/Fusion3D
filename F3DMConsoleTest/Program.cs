using System;
using V3DM;

namespace V3DMConsoleTest
{
    internal class Program
    {
        private static void Main ( string [ ] args )
        {
            Console.WriteLine ( "Importing mesh" );

            V3DM.ImportV3D imp = new ImportV3D();

            imp.ImportMesh ( "v9.v3dm" );

            while ( true )
            {
            }
        }
    }
}