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

        public CodeScope LocalScope = new CodeScope();

        public virtual void Exec ( )
        {
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

        public virtual void BackOne ( )
        {
            TokStream.Pos--;
            if ( TokStream.Pos < 0 )
            {
                TokStream.Pos = 0;
            }
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
                Parser ( TokStream.GetNext ( ) );
                if ( Done )
                {
                    return;
                }
            }
        }

        public VToken ConsumeNext ( )
        {
            return TokStream.GetNext ( );
        }

        public virtual void SetupParser ( )
        {
        }

        public StrandType Predict ( )
        {
            System.Console.WriteLine ( "Predicting." );
            int cpos = TokStream.Pos;
            for ( int i = cpos; i < TokStream.Len; i++ )
            {
                switch ( TokStream.Tokes [ i ].Class )
                {
                    case TokenClass.Id:
                        break;

                    case TokenClass.Scope:
                        return StrandType.FlatStatement;
                        break;
                }
                System.Console.WriteLine ( "Tok:" + TokStream.Tokes [ i ].Class.ToString ( ) + " 2:" + TokStream.Tokes [ i ].Token.ToString ( ) + " 3:" + TokStream.Tokes [ i ].Text );
            }
            System.Console.WriteLine ( "Done." );
            while ( true )
            {
            }
            return StrandType.Unknown;
        }
    }

    public enum StrandType
    {
        Statement, Assignment, Flow, Define, Macro, Header, Extends, Generic, Unknown,
        FlatStatement
    }
}