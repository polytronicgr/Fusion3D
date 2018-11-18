using System;

namespace FusionScript.Structs
{
    public class StructAssign : Struct
    {
        public System.Collections.Generic.List<Var> Vars = new System.Collections.Generic.List<Var>();
        public System.Collections.Generic.List<StructExpr> Expr = new System.Collections.Generic.List<StructExpr>();

      

        public bool CreatedVars = false;

        public override dynamic Exec ( )
        {
            if ( !CreatedVars )
            {
                Var qv = ManagedHost.CurrentScope.FindVar( Vars[0].Name,true );
                if ( qv != null )
                {
                    Vars [ 0 ] = qv;
                }
                else
                {
                    ManagedHost.CurrentScope.RegisterVar ( Vars [ 0 ] );
                }
            }
            if (Vars.Count == 1)
            {
                Vars[0].Value = Expr[0].Exec();
            }
            else
            {
                dynamic basev = null;
                foreach(var var in Vars)
                {
                    if (basev == null)
                    {
                        
                        basev = ManagedHost.CurrentScope.FindVar(var.Name, true);
                    }
                    else
                    {
                        basev = basev.Value.FindVar(var.Name);
                    }
                }

                basev.Value = Expr[0].Exec();

                Console.WriteLine("!");

            }
            Console.WriteLine("Assign:" + Vars[0].Name + " Val:" + Vars[0].Value);
            return null;
        }

      
    }
}