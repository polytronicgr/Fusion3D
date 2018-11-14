using System;

namespace VividScript.VStructs
{
    public class VSPars : VStruct
    {
        public System.Collections.Generic.List<VSPar> Pars = new System.Collections.Generic.List<VSPar>();

        public VSPars ( VTokenStream s ) : base ( s )
        {
        }

        public override string DebugString ( )
        {
            return "Pars:";
        }

        public override void SetupParser ( )
        {
            Parser = ( t ) =>

            {
                
                if ( t.Token == Token.RightPara )
                {
                    Done = true;
                    return;
                }
                if ( t.Token == Token.Comma )
                {
                    return;
                }
                VSPar np = new VSPar(TokStream);
                if(t.Token == Token.Id)
                {

                   
                    Pars.Add(np);

                }
                /*
                switch ( t.Token )
                {
                    case Token.Int:
                        VToken name = ConsumeNext();

                        np.Name = name.Text;
                        np.ParType = VarType.Int;
                        Pars.Add ( np );
                        break;

                    case Token.Float:
                    case Token.Byte:
                    case Token.Bool:
                    case Token.Double:
                    case Token.String:
                        VToken name3 = ConsumeNext();

                        np.Name = name3.Text;
                        np.ParType = VarType.String;
                        Pars.Add ( np );
                        break;

                    case Token.Short:
                        VToken name2 = ConsumeNext();

                        np.Name = name2.Text;
                        np.ParType = VarType.Short;
                        Pars.Add ( np );
                        break;
                }
                */
                if ( t.Token == Token.RightPara )
                {
                    Done = true;
                }
            };
        }
    }
}