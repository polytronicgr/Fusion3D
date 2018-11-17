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
     

        public override dynamic Exec()
        {
            dynamic bc = null;
            for (int i = 0; i < call.Count; i++)
            {
                if(call[i]==".")
                {
                    continue;
                }
                if (bc != null && i == call.Count - 1)
                {

                    var meth = call[i];
                    dynamic[] pars = new dynamic[Pars.Pars.Count];
                    for (int p = 0; p < Pars.Pars.Count; p++)
                    {



                        pars[p] = Pars.Pars[p].Exec();
                    }
                    ManagedHost.Main.ExecuteMethod(bc, meth, pars);


                    //Console.WriteLine("Class:" + bc.ModuleName + " Meth:" + call[i]);

                }
                else
                {
                    if (bc == null)
                    {
                        bc = ManagedHost.CurrentScope.FindVar(call[i], true).Value;
                    }
                    else
                    {
                        bc = bc.FindVar(call[i]).Value;
                    }
                }
            }
            Console.WriteLine(":");
            return null;
        }

    
    }
}
