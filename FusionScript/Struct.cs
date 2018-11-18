using System.Collections.Generic;

namespace FusionScript
{
    public delegate void ParseStructToken(CodeToken t);

    public enum StrandType
    {
        Statement, Assignment, Flow, Define, Macro, Header, Extends, Generic, Unknown, While, If, Else, ElseIf, Wend, For, Do, Loop,
        FlatStatement, ClassStatement,Return
    }

    public enum StructType
    {
        Entry, Module, Method, Function, Exit, Unknown
    }
    public class Struct
    {
       
        public CodeScope LocalScope = null;
        public string Name = "";
        
   
        public List<Struct> Structs = new List<Struct>();
     
        public StructType Type = StructType.Unknown;
        public Struct()
        {

        }

     


        public virtual string DebugString()
        {
            return "Empty";
        }

        public virtual dynamic Exec()
        {
            return null;
        }
    

     
       
    }
}