using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Texture;
using Vivid3D.Resonance;
using Vivid3D.Resonance.Forms;
namespace FusionIDE.Forms
{
    public class CodeEditorForm : WindowForm
    {
        public TextAreaForm TextEdit;
        public CodeEditorForm()
        {

            //var text_edit = new TextAreaForm();
            
            TextEdit = new TextAreaForm();
            SubChanged = () =>
            {
                TextEdit.X = 100;
                TextEdit.Y = 25;
                TextEdit.W = W - 10;
                TextEdit.H = H - 30;
            };
          Add(TextEdit);

        }


        

    }
}
