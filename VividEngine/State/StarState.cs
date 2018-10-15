using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Logic;
using Vivid3D.Resonance;
using Vivid3D.Resonance.Forms;
namespace Vivid3D.State
{

    public class VividState
    {

        public Logic.Logics Logics = new Logics(1000 / 60, false);
        public Logic.Logics Graphics = new Logics(1000 / 60, false);
        public Resonance.UI SUI = null;

        public string Name
        {
            get;
            set;
        }

        public bool Running
        {
            get;
            set;
        }

        public void InitUI()
        {
            SUI = new Resonance.UI();
        }

        public VividState(string name = "")
        {
            Name = name;
            Running = false;
          //  SUI = new Resonance.UI();
        }

        public virtual void InitState()
        {

        }

        public virtual void StartState()
        {

        }

        public virtual void UpdateState()
        {

        }

        
        public virtual void DrawState()
        {

        }

        public virtual void StopState()
        {

        }

        public virtual void ResumeState()
        {

        }

        public void InternalUpdate()
        {
            Logics.SmartUpdate();
            Graphics.SmartUpdate();

        }


    }

}
