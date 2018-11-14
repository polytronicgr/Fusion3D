using System.Collections.Generic;

namespace VividScript
{
    public enum VStructType
    {
        Entry, Module, Method, Function, Exit, Unknown
    }

    public delegate void ParseStructToken ( VToken t );

    public class VStruct
    {
        public VStructType Type = VStructType.Unknown;
        public string Name = "";
        public List<VStruct> Structs = new List<VStruct>();
        public int At = 0;
        public bool Ran = false;
        public long RunCount = 0;
        public ParseStructToken PreParser = null;
        public ParseStructToken Parser = null;
        public bool Done = false;
        public VTokenStream TokStream = null;

        public CodeScope LocalScope = null;

        public virtual dynamic Exec ( )
        {
            return null;
        }

        public virtual string DebugString ( )
        {
            return "Empty";
        }

        public VStruct ( VTokenStream toks, bool noParse = false )
        {
            TokStream = toks;
            if ( noParse )
            {
                return;
            }

            Parse ( );
        }

        public VStruct()
        {

        }

        public virtual VToken BackOne ( )
        {
            TokStream.Pos--;
            if ( TokStream.Pos < 0 )
            {
                TokStream.Pos = 0;
            }
            return TokStream.Tokes [ TokStream.Pos ];
        }

        public virtual void SkipOne ( )
        {
            TokStream.Pos++;
            if ( TokStream.Pos > TokStream.Len - 1 )
            {
                TokStream.Pos = TokStream.Len - 1;
            }
        }

        public virtual VToken PeekNext ( )
        {
            if ( TokStream.Pos >= TokStream.Len )
            {
                return null;
            }

            return TokStream.Tokes [ TokStream.Pos ];
        }

        public virtual VToken Peek ( int c )
        {
            c = c - 1;
            if ( TokStream.Pos + c >= TokStream.Len )
            {
                return null;
            }

            return TokStream.Tokes [ TokStream.Pos + c ];
        }

        public virtual void PrevBegin ( )
        {
            for ( int i = TokStream.Pos - 1; i == 0; i-- )
            {
                if ( TokStream.Tokes [ i ].Class == TokenClass.BeginLine )
                {
                    TokStream.Pos = i;
                    return;
                }
            }
        }

        public virtual void NextBegin ( )
        {
            for ( int i = TokStream.Pos; i < TokStream.Len; i++ )
            {
                if ( TokStream.Tokes [ i ].Class == TokenClass.BeginLine )
                {
                    TokStream.Pos = i;
                    return;
                }
            }
        }

        public virtual void Parse ( )
        {
            SetupParser ( );
            if ( PreParser != null )
            {
                PreParser ( TokStream.GetNext ( ) );
            }

            while ( TokStream.Pos < TokStream.Len )
            {
                VToken nt = PeekNext();
                // Console.WriteLine("VS:" + nt.Text + " T:" + nt.Token);
                Trace = Trace + TokStream.Tokes [ TokStream.Pos ];
                TextTrace = TextTrace + " " + TokStream.Tokes [ TokStream.Pos ].Text;
                if ( TokStream.Tokes [ TokStream.Pos ].Class == TokenClass.BeginLine )
                {
                    if ( PeekNext ( ) == null )
                    {
                        Done = true;
                        return;
                    }
                    if(PeekNext().Token == Token.End)
                    {
                        Done = true;
                        return;
                    }
                    ConsumeNext ( );
                    if(PeekNext().Token == Token.End){
                        Done = true;
                        return;
                    }
                    if ( PeekNext ( ) == null )
                    {
                        Done = true;
                        return;
                    }
                    if ( PeekNext ( ).Class == TokenClass.BeginLine )
                    {
                        ConsumeNext ( );
                    }
                }
                if ( PeekNext ( ) == null )
                {
                    Done = true;
                    return;
                }
                if ( PeekNext ( ).Class == TokenClass.BeginLine )
                {
                    if ( TokStream.Pos >= TokStream.Len - 1 )
                    {
                        Done = true;
                        return;
                    }
                }
                Parser ( TokStream.GetNext ( ) );
                if ( Done )
                {
                    return;
                }
            }
        }

        public string Trace = "";
        public string TextTrace = "";

        public VToken ConsumeNext ( )
        {
            return TokStream.GetNext ( );
        }

        public virtual void SetupParser ( )
        {
        }

        public StrandType Predict ( )
        {
         
            int cpos = TokStream.Pos;
            if ( cpos >= TokStream.Len - 1 )
            {
                return StrandType.Unknown;
            }
            int imod=0;
            if ( TokStream.Tokes [ cpos ].Token == Token.LeftPara )
            {
                imod = -1;
            }

            for ( int i = cpos; i < TokStream.Len; i++ )
            {
                int ni = i + imod;
                if ( TokStream.Tokes [ i ].Token == Token.If )
                {
                    return StrandType.If;
                }
                if ( TokStream.Tokes [ ni ].Token == Token.For )
                {
                    //System.Console.WriteLine ( "Yep!" );
                    return StrandType.For;
                }
                if ( TokStream.Tokes [ ni ].Token == Token.While )
                {
                    //System.Console.WriteLine ( "While!" );
                    return StrandType.While;
                }
                if ( TokStream.Tokes [ ni ].Token == Token.Equal )
                {
                    return StrandType.Assignment;
                }
                switch ( TokStream.Tokes [ ni ].Class )
                {
                    case TokenClass.Id:
                        break;

                    case TokenClass.Scope:
                        return StrandType.FlatStatement;
                        break;
                }
                System.Console.WriteLine ( "Tok:" + TokStream.Tokes [ ni ].Class.ToString ( ) + " 2:" + TokStream.Tokes [ ni ].Token.ToString ( ) + " 3:" + TokStream.Tokes [ ni ].Text );
            }

            return StrandType.Unknown;
        }
    }

    public enum StrandType
    {
        Statement, Assignment, Flow, Define, Macro, Header, Extends, Generic, Unknown, While, If, Else, ElseIf, Wend, For, Do, Loop,
        FlatStatement
    }
}