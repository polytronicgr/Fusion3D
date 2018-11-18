using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionScript.Structs
{
    public class StructReturn : Struct
    {

        public StructExpr ReturnExp = null;

        public override dynamic Exec()
        {

            if (ReturnExp != null)
            {
                return ReturnExp.Exec();
            }

            return null;
        }

    }
}
