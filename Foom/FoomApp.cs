using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion3D;
using Fusion3D.App;
using Foom.States;
namespace Foom
{
    public class FoomApp : FusionApp
    {

        public FoomApp() : base("Foom - A Fusion powred game.", 1024, 768, false)
        {



            InitState = new FoomMenuState();

            Run();

        }



    }
}
