using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionEngine;
using FusionEngine.App;
using Foom.States;
//using FusionCLNet;

namespace Foom
{
    public class InvaderAPP : FusionApp
    {

        public InvaderAPP() : base("Foom - A Fusion powred game.", 1024, 768, false)
        {

            /*
            FusionCL.InitFusionCL();

            
            var cq = new CLCommandQueue();

            //var buf1 = new CLMemBuffer(1024, CLBufferType.Read_Only);


            byte[] test = new byte[1024];

            for(int i = 0; i < test.Length; i++)
            {
                test[i] = 80;
            }

            var buf1 = new CLMemBuffer(1024,CLBufferType.Read_Only);

            var buf2 = new CLMemBuffer(1024, CLBufferType.Write_Only);

            cq.Write(buf1, true, test, 0);

         
            CLProgram prog = new CLProgram("Foom/CL/testProg.txt");

            if (prog.Build())
            {
                Console.WriteLine("Built!");
            }

            CLKernel vector_add = prog.CreateKernel("vector_add");

            vector_add.SetArg(0, buf1);
            vector_add.SetArg(1, buf2);
        
            cq.Range(vector_add, 1024, 16);

            byte[] pdat = cq.Read(buf2, true);

            for(int i = 0; i < pdat.Length; i++)
            {
                Console.WriteLine("I:" + i + " V:" + pdat[i]);
            }
            */

            InitState = new InvaderMenuState();

            Run();

        }



    }
}
