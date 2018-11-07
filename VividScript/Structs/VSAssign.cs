using System;

namespace VividScript.VStructs
{
    public class VSAssign : VStruct
    {
        public System.Collections.Generic.List<VSVar> Vars = new System.Collections.Generic.List<VSVar>();
        public System.Collections.Generic.List<VSExpr> Expr = new System.Collections.Generic.List<VSExpr>();

        public VSAssign ( VTokenStream s ) : base ( s )
        {
        }

        public bool CreatedVars = false;

        public override dynamic Exec ( )
        {
            if ( !CreatedVars )
            {
                VME.CurrentScope.RegisterVar ( Vars [ 0 ] );
            }
            Vars [ 0 ].Value = Expr [ 0 ].NextE ( 0, null );
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
                VSVar nv = new VSVar
                {
                    Name = name
                };
                Vars.Add ( nv );

                switch ( t.Token )
                {
                    case Token.Id:
                        if ( PeekNext ( ).Token == Token.Equal )
                        {
                            Console.WriteLine ( "=" );
                            ConsumeNext ( );
                            Expr.Add ( new VSExpr ( TokStream ) );
                            BackOne ( );
                            if ( PeekNext ( ).Token == Token.EndLine )
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