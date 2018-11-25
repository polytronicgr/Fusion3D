using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionCLNet;
using Cloo;
using System.IO;
namespace FusionCLTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test2();

            Test1();

            ComputePlatform plat = ComputePlatform.Platforms[0];
            Console.WriteLine("Plat:" + plat.Name);

            ComputeContext context = new ComputeContext(ComputeDeviceTypes.Gpu, new ComputeContextPropertyList(plat), null, IntPtr.Zero);

            ComputeCommandQueue queue = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);

            StreamReader rs = new StreamReader("Foom/CL/testProg.txt");

            string clSrc = rs.ReadToEnd();

            rs.Close();

            ComputeProgram prog = new ComputeProgram(context, clSrc);

            prog.Build(null, null, null, IntPtr.Zero);

            Console.WriteLine("BS:" + prog.GetBuildStatus(context.Devices[0]).ToString());
            Console.WriteLine("Info:" + prog.GetBuildLog(context.Devices[0]));

            ComputeKernel kern = prog.CreateKernel("vector_add");

            int[] data = new int[1024];

            for(int i=0;i<1024;i++)
            {
                data[i] = 100;
            }

            ComputeBuffer<int> b1 = new ComputeBuffer<int>(context, ComputeMemoryFlags.CopyHostPointer, data);

            ComputeBuffer<int> b2 = new ComputeBuffer<int>(context, ComputeMemoryFlags.WriteOnly, 1024);

//            queue.WriteToBuffer<int>(data, b1, true, null);



            kern.SetMemoryArgument(0, b1);
            kern.SetMemoryArgument(1, b2);

            long[] wo = new long[1];
            wo[0] = 0;

            long[] ws = new long[1];
            ws[0] = 1024;

            long[] tc = new long[1];
            tc[0] = 16;

            queue.Execute(kern, wo, ws, tc, null);
            int c = Environment.TickCount;

            queue.Finish();

            c = Environment.TickCount - c;

          

            queue.ReadFromBuffer<int>(b2, ref data, true, null);


            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine("C:" + (int)data[i]);
            }

            Console.WriteLine("Done:" + c);


            while (true)
            {

            }


        
        }

        private static void Test1()
        {
          


        }

        private static void Test2()
        {
        
        }
    }
}
