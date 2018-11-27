using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FusionEngine.Texture;
using FusionEngine.Resonance;
using FusionEngine.Resonance.Forms;
namespace FusionIDE.Forms
{
    public class CodeEditorForm : WindowForm
    {
        public TextAreaForm TextEdit;
        public OpenTK.Vector4 ModuleKey = new OpenTK.Vector4(1, 1, 0.2f, 0.8f);
        public OpenTK.Vector4 EndKey = new OpenTK.Vector4(1, 1, 0.2f, 0.8f);
        public OpenTK.Vector4 Func = new OpenTK.Vector4(0.3f, 1.0f, 1.0f, 0.8f);
        public CodeEditorForm()
        {

            //var text_edit = new TextAreaForm();

            TextEdit = new TextAreaForm();
            SubChanged = () =>
            {
                TextEdit.X = 5;
                TextEdit.Y = 25;
                TextEdit.W = W - 10;
                TextEdit.H = H - 30;
            };
            Add(TextEdit);
            TextEdit.Keys.Add(new TextAreaForm.KeyColor("module", ModuleKey));
            TextEdit.Keys.Add(new TextAreaForm.KeyColor("func",Func));
            TextEdit.Keys.Add(new TextAreaForm.KeyColor("end", EndKey));
        }


        

    }
}
