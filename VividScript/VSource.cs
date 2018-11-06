using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace VividScript
{
    public class VSource
    {
        public string Path = "";
        public VTokenStream Tokens = null;
        public VSource(string path)
        {
            Path = path;
            int it = 0;
            //FileStream fs = new FileStream(path, FileMode.Open,
              //  FileAccess.Read);
            //TextReader r = new StreamReader(fs);
            VTokenizer toker = new VTokenizer();
            Tokens = new VTokenStream();
            var elt = new VToken(TokenClass.Flow, Token.EndLine, "EndLine");
            string[] lines = (string[])File.ReadAllLines(path);
            int li = 0;
            while (true)
            {
                // Console.WriteLine("Pos:" + fs.Position + " Len:" + fs.Length);
                //if (fs.Position >= fs.Length) break;
                if (li == lines.Length) break;
                string cl = lines[li];
                li++;
                Console.WriteLine("LI:" + li);
                if (li > lines.Length) break;
                //fs.Seek(cl.Length, SeekOrigin.Begin);
                //it = it + cl.Length;


//                if (it > fs.Length) break;
                cl = cl + System.Environment.NewLine;
             //   Console.WriteLine("CL:" + cl);
                if(cl==null || cl==string.Empty || cl==" " || cl == System.Environment.NewLine)
                {
                    continue;
                }
                var ts = toker.ParseString(cl);
                
                //Console.WriteLine("Parsed line. " + ts.Len + " tokens.");
                foreach(var t in ts.Tokes)
                {
                    if (t.Text.Contains(System.Environment.NewLine) && t.Text.Contains("\r") && t.Text.Length<3) continue;
                    Tokens.Add(t);
                }
                Tokens.Add(elt);

            }
            List<VToken> nk = new List<VToken>();
  //          fs.Close();
            Console.WriteLine("TokDump");
            VToken pt = null;
            foreach (var t in Tokens.Tokes) 
            {
                if (pt != null)
                {
                    if (t.Token == Token.EndLine && pt.Token == Token.EndLine)
                    {
                        continue;
                    }
                }
                t.Text = t.Text.Replace(" ", "");
                t.Text = t.Text.Replace("   ", "");
                t.Text = t.Text.Replace(System.Environment.NewLine, "");
                if(t.Text.Length<1)
                {
                    continue;
                }
                if(t.Text == "")
                {
                    continue;
                }
                if(t.Text == System.Environment.NewLine)
                {
                    continue;
                }
                if(t.Text == "  ")
                {
                    continue;
                }
                nk.Add(t);
                pt = t;
                if(t.Text == "end" || t.Text == "End")
                {
                    t.Token = Token.End;
                    t.Class = TokenClass.Flow;
                }
                Console.WriteLine("T:" + t.Text + " TOK:" + t.Token);

            }
            Tokens = new VTokenStream();
            Tokens.Tokes = nk;
            Console.WriteLine("Done.");
            Console.WriteLine("Parsed Source:" + Tokens.Len + " tokens.");
        }
    }
}
