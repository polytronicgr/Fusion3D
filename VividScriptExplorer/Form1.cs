using System;
using System.Windows.Forms;
using VividScript;

namespace VividScriptExplorer
{
    public partial class Form1 : Form
    {
        public Form1 ( )
        {
            InitializeComponent ( );
        }

        private void button1_Click ( object sender, EventArgs e )
        {
            openFileDialog1.ShowDialog ( );

            VSource src = new VSource(openFileDialog1.FileName);
            VCompiler comp = new VCompiler();
            VCompiledSource s = comp.Compile(src);

            RebuildTree ( s.EntryPoint );
        }

        public void RebuildTree ( VividScript.VStructs.VSEntry entry )
        {
            codeTree.Nodes [ 0 ].Nodes.Clear ( );

            TreeNode EntryNode = new TreeNode(entry.DebugString());

            codeTree.Nodes [ 0 ].Nodes.Add ( EntryNode );

            TreeNode mod_node = new TreeNode("Modules:");
            codeTree.Nodes [ 0 ].Nodes [ 0 ].Nodes.Add ( mod_node );
            foreach ( VividScript.VStructs.VSModule mod in entry.Modules )
            {
                TreeNode modnode = new TreeNode(mod.DebugString());
                mod_node.Nodes.Add ( modnode );
                foreach ( VividScript.VStructs.VSVar v in mod.Vars )
                {
                    TreeNode var_node = new TreeNode("Var:"+v.DebugString());
                    modnode.Nodes.Add ( var_node );
                }
                //codeTree.Nodes [ 0 ].Nodes [ 0 ].Nodes [ 0 ].Nodes.Add ( modnode );
            }

            TreeNode func_node = new TreeNode("SystemFuncs");
            codeTree.Nodes [ 0 ].Nodes [ 0 ].Nodes.Add ( func_node );

            foreach ( VividScript.VStructs.VSFunc var in entry.SystemFuncs )
            {
                TreeNode func = new TreeNode(var.DebugString());
                func_node.Nodes.Add ( func );
                TreeNode pars = new TreeNode("Pars:");
                func.Nodes.Add ( pars );
                foreach ( VividScript.VStructs.VSPar p in var.Pars.Pars )
                {
                    TreeNode par_node = new TreeNode(p.DebugString());
                    pars.Nodes.Add ( par_node );
                }
                TreeNode code = new TreeNode("Code:");
                func.Nodes.Add ( code );
            }

            codeTree.Invalidate ( );
        }
    }
}