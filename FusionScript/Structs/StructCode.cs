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
                if(l is StructReturn)
                {
                    var sr = l as StructReturn;
                    if (sr.ReturnExp!=null)
                    {
                        return sr.ReturnExp.Exec();
                    }
                    return null;
                }
                l.Exec ( );
            }
            return null;
        }

     
    }
}