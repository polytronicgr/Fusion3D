using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VividScript;
namespace VividScript.VStructs
{
    public class VSClassCall : VStruct
    {
        public List<string> call = new List<string>();
        public VSCallPars Pars;// = new VSCallPars();
        public VSClassCall(VTokenStream s) : base(s)
        {

        }

        public override dynamic Exec()
        {
            dynamic bc = null;
            for (int i = 0; i < call.Count; i++)
            {
                if (bc != null)
                {

                    var meth = call[i];
                    dynamic[] pars = new dynamic[Pars.Pars.Count];
                    for (int p = 0; p < Pars.Pars.Count; p++)
                    {
              
            

                        pars[p] = Pars.Pars[p].Exec();
                    }
                    VME.Main.ExecuteMethod(bc, meth,pars);


                    //Console.WriteLine("Class:" + bc.ModuleName + " Meth:" + call[i]);

                }else
                {
                    bc = VME.CurrentScope.FindVar(call[i], true).Value;
                }
            }
            Console.WriteLine(":");
            return null;
        }

        public override void SetupParser()
        {

            dynamic prev = null;
            Parser = (t) =>
            {

                if (t.Text == ".")
                {
                    return;
                }
                if(t.Text == "(")
                {
                    Pars = new VSCallPars(TokStream);
                    return;
                }
                if(t.Text == ";")
                {
                    Done = true;
                    return;
                }
                call.Add(t.Text);
                

            };

        }

    }
}
