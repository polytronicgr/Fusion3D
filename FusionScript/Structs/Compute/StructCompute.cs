using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FusionCLNet;
namespace FusionScript.Structs.Compute
{
    public class StructCompute : Struct
    {

        public string ComputeName = "";
        public List<Compute.StructComputeStruct> Unique = new List<StructComputeStruct>();
        public List<Compute.StructComputeStruct> Inputs = new List<StructComputeStruct>();
        public List<Compute.StructComputeStruct> Outputs = new List<StructComputeStruct>();
        public List<ComputeVar> LocalVars = new List<ComputeVar>();
        public List<StructComputeFunc> Funcs = new List<StructComputeFunc>();
        public string CodeName = "";
        public static int cl_num = 0;
        public void GenCode()
        {
            cl_num++;
            CodeName = "CLGen/CLOut" + cl_num + ".cl";

            TextWriter tw = File.CreateText(CodeName);

            foreach (var s in Unique)
            {

                string head = "typedef struct __attribute__ ((packed)) tag_" + s.StructName + "{";
                tw.WriteLine(head);

                foreach (var v in s.Vars)
                {

                    string var_l = "";
                    switch (v.Type)
                    {
                        case ComputeVarType.Byte:
                            if (v.Pointer)
                            {
                                var_l = "   byte *" + v.Name + ";";
                            }
                            else
                            {
                                var_l = "   byte " + v.Name + ";";
                            }
                            break;
                        case ComputeVarType.Vec3:
                            if (v.Pointer)
                            {
                                var_l = "   float3 *" + v.Name + ";";
                            }
                            else
                            {
                                var_l = "   float3 " + v.Name + ";";
                            }
                                break;
                        case ComputeVarType.Int:
                            if (v.Pointer)
                            {
                                var_l = "   int *" + v.Name + ";";
                            }
                            else
                            {
                                var_l = "   int " + v.Name + ";";
                            }
                            break;
                    }

                    tw.WriteLine(var_l);

                }

                string foot = "}" + s.StructName + ";";
                tw.WriteLine(foot);
                tw.WriteLine(" ");
            }
            tw.WriteLine(" ");

            foreach (var fun in Funcs)
            {

                string f_b = "";
                switch (fun.ReturnType)
                {
                    case ComputeVarType.Int:
                        f_b = "int ";
                        break;
                    case ComputeVarType.Void:
                        f_b = "void ";
                        break;
                    case ComputeVarType.Vec3:
                        f_b = "float3 ";
                        break;

                }
                bool first2 = true;
                f_b += fun.FuncName + "(";
                foreach (var vv in fun.InVars)
                {
                    if (!first2)
                    {
                        f_b += " , ";
                    }
                    first2 = false;
                    switch (vv.Type)
                    {
                        case ComputeVarType.Int:
                            if (vv.Pointer)
                            {
                                f_b += " int * " + vv.Name;
                            }
                            else
                            {
                                f_b += " int " + vv.Name;
                            }
                            break;
                        case ComputeVarType.Vec3:
                            if (vv.Pointer)
                            {
                                f_b += " float3 * " + vv.Name;
                            }
                            else
                            {
                                f_b += " float3 " + vv.Name;
                            }
                            break;
                        case ComputeVarType.Struct:
                            if (vv.Pointer)
                            {
                                f_b += " " + vv.StructName + " * " + vv.Name;
                            }
                            else
                            {
                                f_b += " " + vv.StructName + " " + vv.Name;
                            }
                            break;
                    }
                }
                f_b += " )";

                tw.WriteLine(f_b);
                tw.WriteLine("{");

                WriteCode(tw,fun.Code);

                tw.WriteLine("}");

            }
            tw.WriteLine(" ");

            string kern_head = "__kernel void " + ComputeName + "(";

            bool first = true;
            int fvv = 0;
            foreach (var iv in Inputs)
            {

                kern_head += "__global " + iv.StructName + " *" + iv.LocalName;
                if(Inputs.Count>(fvv+1)) 
                {
                    kern_head += " , ";
                }
                first = false;

                fvv++;

            }
            if (Outputs.Count > 0 && Inputs.Count>0)
            {
                kern_head += " , ";
            }
            fvv = 0;
            foreach(var ov in Outputs)
            {

                if (ov.One)
                {
                    kern_head += "__global " + ov.StructName + " " + ov.LocalName;
                }
                else
                {
                    kern_head += "__global " + ov.StructName + " *" + ov.LocalName;
                }
                if (Outputs.Count > (fvv + 1))
                {
                    kern_head += " , ";
                }

            }

            kern_head += ")";

            tw.WriteLine(kern_head);
            tw.WriteLine("{");
            tw.WriteLine("  int index = get_global_id(0);");
            tw.WriteLine("}");


            tw.Flush();
            tw.Close();

            FusionCL.InitFusionCL();

            var prog = new CLProgram("CLGen/CLOut1.cl");
            if (!prog.Build())
            {
                Console.WriteLine("Not built!-cl");
            }

            var kern = prog.CreateKernel("imageRender");

            while (true)
            {

            }


        }
        public void WriteCode(TextWriter tw, StructComputeCode code)
        {

            foreach(var l in code.Lines)
            {
                if(l is Compute.ComputeCodeTypes.ComputeCodeFor)
                {

                    var lf = l as ComputeCodeTypes.ComputeCodeFor;

                    var line = "    ";

                    line += "for(";

                    var l_as = lf.InitAssign;

                    if (l_as.Init)
                    {

                        switch (l_as.Type)
                        {
                            case ComputeVarType.Int:
                                line += " int ";
                                break;
                            case ComputeVarType.Vec3:
                                line += " float3 ";
                                break;
                            

                        }

                    }

                    line += l_as.VarName;

                    if (l_as.Value != null)
                    {
                        line += " = ";
                        foreach(var le in l_as.Value.Seq)
                        {
                            line += le;
                        }
                    }

                    line += ";";

                    foreach(var ce in lf.Condition.Seq)
                    {
                        line += ce;
                    }

                    line += ";";

                    var f_as = lf.Inc;

                    line += f_as.VarName + " = ";
                    foreach(var ie in f_as.Value.Seq)
                    {
                        line += ie;
                    }

                    line += ")";
                    tw.WriteLine(line);
                    tw.WriteLine("  {");
                    WriteCode(tw, lf.Code);
                    tw.WriteLine("  }");



                    


                }
                if(l is Compute.ComputeCodeTypes.ComputeCodeAssign)
                {

                    var la = l as ComputeCodeTypes.ComputeCodeAssign;

                    var line = "    ";

                    if (la.Init)
                    {

                        switch (la.Type)
                        {
                            case ComputeVarType.Int:
                                line += "int ";
                                break;
                            case ComputeVarType.Vec3:
                                line += "float3 ";
                                break;
                        }
                    }
                        line += la.VarName;
                        if(la.Value == null)
                        {
                            line += ";";
                            tw.WriteLine(line);
                        }
                        else
                        {
                            tw.Write(line + " = ");
                            WriteExpr(tw, la.Value);
                            tw.Write(";");
                            tw.WriteLine("");
                        }

                    }

                    
                

                
            }

        }

        public void WriteExpr(TextWriter tw,ComputeCodeTypes.ComputeCodeExpr exp)
        {
            foreach(var s in exp.Seq)
            {
                tw.Write(s);
            }
           
        }

    }
    
}
