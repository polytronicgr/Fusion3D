using System.Collections.Generic;
namespace FusionScript.Structs
{
    public class StructIf : Struct
    {
        public StructExpr Condition;
        public StructCode TrueCode;
        public StructCode ElseCode;
        public List<StructExpr> ElseIf = new List<StructExpr>();
        public List<StructCode> ElseIfCode = new List<StructCode>();

        public override dynamic Exec ( )
        {
            if ( Condition.Exec ( )==1 )
            {
                TrueCode.Exec ( );
            }
            else
            {
                int ii = 0;
                bool done = false;
                foreach(var else_if in ElseIf)
                {
                    if (else_if.Exec() == 1)
                    {
                        ElseIfCode[ii].Exec();
                        done = true;
                        break;
                    }
                    ii++;
                }

                if (!done)
                {
                    if (ElseCode != null)
                    {
                        ElseCode.Exec();
                    }
                }
             
            }
            return null;
        }

       
    }
}