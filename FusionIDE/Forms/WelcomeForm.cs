using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionEngine.Resonance;
using FusionEngine.Resonance.Forms;
using FusionEngine.Resonance;
namespace FusionIDE.Forms
{
    public class WelcomeForm :  WindowForm
    {
        public LoggedIn LogIn = null;
        public LoggedIn Create = null;
        public WelcomeForm()
        {

            var ul = new LabelForm().Set(20, 60, 120, 25, "Username");
            Add(ul);
            var ub = new TextBoxForm().Set(120, 63, 170, 25);
            Add(ub);

            ul = new LabelForm().Set(20, 110, 120, 25, "Password");
            var pb = new TextBoxForm().Set(120, 113, 170, 25);
            Add(pb);
            Add(ul);

            var eb = new ButtonForm().Set(20, 200, 90, 25, "Login");
            Add(eb);

            eb.Click = (b) =>
            {
                LogIn?.Invoke(ub.Text, pb.Text);
            };

            var qb = new ButtonForm().Set(215, 200, 90, 25, "Exit");
            Add(qb);

            qb.Click = (b) =>
            {

                Environment.Exit(0);


            };

            var cab = new ButtonForm().Set(125, 200, 80, 25, "Create");
            Add(cab);

            cab.Click = (b) =>
            {

                Create?.Invoke(ub.Text, pb.Text);

            };


        }

    }
    public delegate void LoggedIn(string user, string pass);
}
