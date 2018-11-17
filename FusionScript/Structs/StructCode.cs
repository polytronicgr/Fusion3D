using System;
using System.Collections.Generic;

namespace FusionScript.Structs
{
    public class StructCode : Struct
    {
        public List<Struct> Lines = new List<Struct>();

    

        public override string DebugString ( )
        {
            return ( "CodeBody." );
        }

        public override dynamic Exec ( )
        {
            foreach ( Struct l in Lines )
            {
                l.Exec ( );
            }
            return null;
        }

     
    }
}