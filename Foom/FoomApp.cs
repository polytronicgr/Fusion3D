using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion3D;
using Fusion3D.App;
using Foom.States;
using FusionCLNet;
namespace Foom
{
    public class FoomApp : FusionApp
    {

        public FoomApp() : base("Foom - A Fusion powred game.", 1024, 768, false)
        {

            FusionCL.InitFusionCL();

            var cq = new CLCommandQueue();

            var buf1 = new CLMemBuffer(1024, CLBufferType.Read_Only);

            byte[] test = new byte[1024];

            var buf2 = new CLMemBuffer(CLBufferType.Read_Only, test);


            byte[] testDat = new byte[1024];

            for(int i = 0; i < 1024; i++)
            {
                testDat[i] = 125;
            }

            cq.Write(buf1, true, testDat);

            CLProgram prog = new CLProgram("Foom/CL/testProg.txt");

   

            if (prog.Build())
            {
                Console.WriteLine("Built!");
            }

            CLKernel vector_add = prog.CreateKernel("vector_add");

            InitState = new FoomMenuState();

            Run();

        }



    }
}
