using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionScript;
namespace FusionScript.Structs
{
    public class StructClassCall : Struct
    {
        public List<string> call = new List<string>();
        public StructCallPars Pars;// = new VSCallPars();
        public StructClassCall(TokenStream s) : base(s)
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
                    ManagedHost.Main.ExecuteMethod(bc, meth,pars);


                    //Console.WriteLine("Class:" + bc.ModuleName + " Meth:" + call[i]);

                }else
                {
                    bc = ManagedHost.CurrentScope.FindVar(call[i], true).Value;
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
                    Pars = new StructCallPars(TokStream);
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
