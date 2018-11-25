using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Cloo;
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
                if (s.LinearData == true) continue;
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
                                var_l = "   unsigned char *" + v.Name + ";";
                            }
                            else
                            {
                                var_l = "   unsigned char " + v.Name + ";";
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
                                f_b += "  " + vv.StructName + " " + vv.Name;
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


                if (ov.LinearData)
                {
                    string v_t = "__global ";
                    switch (ov.Vars[0].Type)
                    {
                        case ComputeVarType.Byte:
                            v_t = "__global unsigned char";
                            break;
                        case ComputeVarType.Int:
                            v_t = "__global int";
                            break;
                        case ComputeVarType.Float:
                            v_t = "__global float";
                            break;
                        case ComputeVarType.Vec3:
                            v_t = "__global float3";
                            break;
                        case ComputeVarType.Vec4:
                            v_t = "__global float4";
                            break;
                    }
                    v_t += " ";
                    if (ov.Vars[0].Pointer)
                    {
                        v_t += "* ";
                    }
                    v_t += ov.LocalName;
                    kern_head += v_t;
                }
                else
                { 
                    if (ov.One)
                    {

                        kern_head += ov.StructName + " " + ov.LocalName;
                    }
                    else
                    {
                        kern_head += ov.StructName + " *" + ov.LocalName;
                    }
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


            ComputePlatform plat = ComputePlatform.Platforms[0];
            Console.WriteLine("Plat:" + plat.Name);

            ComputeContext context = new ComputeContext(ComputeDeviceTypes.Gpu, new ComputeContextPropertyList(plat), null, IntPtr.Zero);

            ComputeCommandQueue queue = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);

            StreamReader rs = new StreamReader("CLGen/CLOut1.cl");

            string clSrc = rs.ReadToEnd();

            rs.Close();

            ComputeProgram prog = new ComputeProgram(context, clSrc);

            try
            {
                prog.Build(null, null, null, IntPtr.Zero);
            }
            catch
            {

            }
            Console.WriteLine("BS:" + prog.GetBuildStatus(context.Devices[0]).ToString());
            Console.WriteLine("Info:" + prog.GetBuildLog(context.Devices[0]));

            ComputeKernel kern = prog.CreateKernel("imageRender");

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
