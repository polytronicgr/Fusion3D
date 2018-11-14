using System;

namespace FusionScript.Structs
{
    public class StructAssign : Struct
    {
        public System.Collections.Generic.List<Var> Vars = new System.Collections.Generic.List<Var>();
        public System.Collections.Generic.List<StructExpr> Expr = new System.Collections.Generic.List<StructExpr>();

        public StructAssign ( TokenStream s ) : base ( s )
        {
        }

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
            Vars[0].Value = Expr[0].Exec();
            return null;
        }

        public override void SetupParser ( )
        {
            Parser = ( t ) =>
            {
                string name = t.Text;
                if ( t.Text == "=" )
                {
                    t = BackOne ( );
                    t = BackOne ( );
                    name = t.Text;
                    ConsumeNext ( );
                }
                Var nv = new Var
                {
                    Name = name
                };
                Vars.Add ( nv );

                switch ( t.Token )
                {
                    case Token.Id:
                        if ( PeekNext ( ).Token == Token.Equal )
                        {

                            //Console.WriteLine ( "=" );
                            ConsumeNext ( );
                            Expr.Add ( new StructExpr ( TokStream ) );
                            BackOne ( );
                            if ( PeekNext ( ).Token == Token.EndLine || PeekNext ( ).Token == Token.RightPara )
                            {
                                Done = true;
                                return;
                            }
                        }
                        break;

                    case Token.End:
                    case Token.EndLine:
                        Done = true;
                        return;
                        break;
                }
            };
        }
    }
}