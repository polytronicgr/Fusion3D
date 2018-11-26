using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion3D;
using Fusion3D.App;
using FoomED.States;
namespace FoomED
{
    public class FoomEDApp : FusionApp
    {
        
        public FoomEDApp() : base("Foom Editor",1280,768,false)
        {

            InitState = new FoomED.States.MainMenuState();

            Run();

        }

    }
}
