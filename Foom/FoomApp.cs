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

            var cq = new CommandQueue();

            var buf1 = new MemBuffer(1024, BufferType.Read_Only);

            byte[] test = new byte[1024];

            var buf2 = new MemBuffer(BufferType.Read_Only, test);


            InitState = new FoomMenuState();

            Run();

        }



    }
}
