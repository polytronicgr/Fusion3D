using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivid3D.Scene;
using Assimp;
using Assimp.Configs;
using Vivid3D.Data;
using System.IO;

namespace Vivid3D.Import
{
    public class AssImpImport : Importer
    {
        public static string IPath = "";
        public static float ScaleX = 1, ScaleY = 1, ScaleZ = 1;
        public static Texture.VTex2D NormBlank = null, DiffBlank, SpecBlank, MetalBlank;
        public static OpenTK.Vector3 CTV(Color4D col)
        {
            return new OpenTK.Vector3(col.R, col.G, col.B);
        }
        public override GraphNode3D LoadNode(string path)
        {

            if (NormBlank == null)
            {
                NormBlank = new Texture.VTex2D("data\\tex\\normblank.png", Texture.LoadMethod.Single, false);
                DiffBlank = new Texture.VTex2D("data\\tex\\diffblank.png", Texture.LoadMethod.Single, false);
                SpecBlank = new Texture.VTex2D("data\\tex\\specblank.png", Texture.LoadMethod.Single, false);
            }



            GraphEntity3D root = new GraphEntity3D();
            string file = path;
     
            var e = new Assimp.AssimpContext();
            var c1 = new Assimp.Configs.NormalSmoothingAngleConfig(75);
            e.SetConfig(c1);
            
            
            Console.WriteLine("Impporting:" + file);
            Assimp.Scene s = null;
            try
            {

                s = e.ImportFile(file, PostProcessSteps.OptimizeMeshes | PostProcessSteps.OptimizeGraph | PostProcessSteps.FindInvalidData | PostProcessSteps.FindDegenerates | PostProcessSteps.Triangulate | PostProcessSteps.ValidateDataStructure | PostProcessSteps.CalculateTangentSpace);
            }
            catch(AssimpException ae)
            {
                Console.WriteLine(ae);
                Console.WriteLine("Failed to import");
                Environment.Exit(-1);
            }
                Console.WriteLine("Imported.");
            Dictionary<string, VMesh> ml = new Dictionary<string, VMesh>();
            List<VMesh> ml2 = new List<VMesh>();
            Console.WriteLine("animCount:" + s.AnimationCount);

            var tf = s.RootNode.Transform;

            tf.Inverse();

            root.GlobalInverse = ToTK(tf);

            var boneToWeight = new Dictionary<uint, List<VertexWeight>>();

            //s.Animations[0].NodeAnimationChannels[0].
            //s.Animations[0].anim

       //     root.Animator.InitAssImp(model);

     
            foreach (var m in s.Meshes)
            {
                
                

                Console.WriteLine("M:" + m.Name + " Bones:" + m.BoneCount);
                Console.WriteLine("AA:" + m.HasMeshAnimationAttachments);
 
                var vm = new Material.Material3D();
                vm.TCol = DiffBlank;
                vm.TNorm = NormBlank;
                vm.TSpec = SpecBlank;
                var m2 = new VMesh(m.GetIndices().Length,m.VertexCount);
                ml2.Add(m2);
               // ml.Add(m.Name, m2);
               for(int b = 0; b < m.BoneCount; b++)
                {
                    uint index = 0;
                    string name = m.Bones[b].Name;

                }
                m2.Mat = vm;
               // root.AddMesh(m2);
                m2.Name = m.Name;
                var mat = s.Materials[m.MaterialIndex];
                TextureSlot t1;

                var sc = mat.GetMaterialTextureCount(TextureType.Unknown);
                Console.WriteLine("SC:" + sc);
                if (mat.HasColorDiffuse)
                {
                    vm.Diff = CTV(mat.ColorDiffuse);
                }
                if (mat.HasColorSpecular)
                {
                    vm.Spec = CTV(mat.ColorSpecular);
                    Console.WriteLine("Spec:" + vm.Spec);
                }
                if (mat.HasShininess)
                {
                   
                    //vm.Shine = 0.3f+ mat.Shininess;
                    Console.WriteLine("Shine:" + vm.Shine);
                }

                Console.WriteLine("Spec:" + vm.Spec);
                //for(int ic = 0; ic < sc; ic++)
                ///{
                if (sc > 0)
                {
                    var tex2 = mat.GetMaterialTextures(TextureType.Unknown)[0];
                                     vm.TSpec = new Texture.VTex2D(IPath + "\\" + tex2.FilePath, Texture.LoadMethod.Single, false);
                }

                if (mat.GetMaterialTextureCount(TextureType.Normals) > 0)
                {
                    var ntt = mat.GetMaterialTextures(TextureType.Normals)[0];
                    Console.WriteLine("Norm:"+ntt.FilePath);
                    vm.TNorm = new Texture.VTex2D(IPath + "\\" + ntt.FilePath, Vivid3D.Texture.LoadMethod.Single, false);
                }

                
                if (mat.GetMaterialTextureCount(TextureType.Diffuse) > 0)
                {
                    
                    t1 = mat.GetMaterialTextures(TextureType.Diffuse)[0];
                    Console.WriteLine("DiffTex:" + t1.FilePath);

                    

                    if (t1.FilePath != null)
                    {
                        try
                        {
                  //          Console.Write("t1:" + t1.FilePath);
                            vm.TCol = new Texture.VTex2D(IPath +"\\"+ t1.FilePath.Replace(".dds",".png"),Texture.LoadMethod.Single, false);
                            if (File.Exists(IPath + "norm" + t1.FilePath))
                            {
//                                vm.TNorm = new Texture.VTex2D(IPath + "norm" + t1.FilePath,Texture.LoadMethod.Single, false);

                    //            Console.WriteLine("TexLoaded");
                            }
                        }
                        catch
                        {

                        }
                    }
                        if (true)
                    {
                   
                        if (new FileInfo(t1.FilePath).Exists == true)
                        {
                            //  var tex = App.AppSal.CreateTex2D();
                            //  tex.Path = t1.FilePath;
                            // tex.Load();
                            //m2.DiffuseMap = tex;
                        }
                    }
                }
                for (int i = 0; i < m2.NumVertices; i++)
                {
                    var v = m.Vertices[i];// * new Vector3D(15, 15, 15);
                    var n = m.Normals[i];
                    var t = m.TextureCoordinateChannels[0];
                    Vector3D tan, bi;
                    if (m.Tangents != null && m.Tangents.Count >0)
                    {

                        tan = m.Tangents[i];
                        bi = m.BiTangents[i];
                    }
                    else
                    {
                        tan = new Vector3D(0, 0, 0);
                        bi = new Vector3D(0, 0, 0);
                    }
                    if (t.Count() == 0) 
                    {

                        m2.SetVertex(i, Cv(v), Cv(tan), Cv(bi), Cv(n), Cv2(new Vector3D(0,0,0)));
                    }
                    else
                    {
                        var tv = t[i];
                        tv.Y = 1.0f - tv.Y;
                        m2.SetVertex(i, Cv(v), Cv(tan), Cv(bi), Cv(n), Cv2(tv));
                    }
                }
                int[] id = m.GetIndices();
                int fi = 0;
                uint[] nd = new uint[id.Length];
                for (int i = 0; i < id.Length; i+=3)
                {
                    //Tri t = new Tri();
                    //t.V0 = (int)nd[i];
                   // t.V1 = (int)nd[i + 1];
                   // t.v2 = (int)nd[i + 2];

                    // nd[i] = (uint)id[i];
                    m2.SetTri(i / 3, (int)id[i], (int)id[i + 1], (int)id[i + 2]);


                }

                m2.Indices = nd;
                //m2.Scale(AssImpImport.ScaleX, AssImpImport.ScaleY, AssImpImport.ScaleZ);
                m2.Final();
                
            }

            ProcessNode(root, s.RootNode, ml2);

            

            return root as GraphNode3D;
        }

        public void ExtractBoneWeights(Mesh mesh,IDictionary<uint,List<VertexWeight>> vertexToBoneWeight)
        {
            foreach(var bone in mesh.Bones)
            {

                // int bone 


            }
        }

        private OpenTK.Matrix4 ToTK(Matrix4x4 mat)
        {
            return new OpenTK.Matrix4(mat.A1, mat.B1, mat.C1, mat.D1,mat.A2, mat.B2, mat.C2, mat.D2, mat.A3, mat.B3, mat.C3, mat.D3, mat.A4, mat.B4,mat.C4, mat.D4);
        }

        private void ProcessNode(GraphEntity3D root, Assimp.Node s,List<VMesh> ml)
        {
  
            GraphEntity3D r1 = new GraphEntity3D();
            root.Sub.Add(r1);
            r1.Top = root;
            r1.Name = s.Name;
            if (s.Name.ToLower().Contains("root"))
            {
                r1.Name =r1.Name+ "*";
                r1.BreakTop = true;
            }


            //r1.LocalTurn = new OpenTK.Matrix4(s.Transform.A1, s.Transform.A2, s.Transform.A3, s.Transform.A4, s.Transform.B1, s.Transform.B2, s.Transform.B3, s.Transform.B4, s.Transform.C1, s.Transform.C2, s.Transform.C3, s.Transform.C4, s.Transform.D1, s.Transform.D2, s.Transform.D3, s.Transform.D4);
            r1.LocalTurn = new OpenTK.Matrix4(s.Transform.A1, s.Transform.B1, s.Transform.C1, s.Transform.D1, s.Transform.A2, s.Transform.B2, s.Transform.C2, s.Transform.D2, s.Transform.A3, s.Transform.B3, s.Transform.C3, s.Transform.D3, s.Transform.A4, s.Transform.B4, s.Transform.C4, s.Transform.D4);
            var lt = r1.LocalTurn;

            r1.LocalTurn = lt.ClearTranslation();
            r1.LocalTurn = r1.LocalTurn.ClearScale();
            r1.LocalPos = lt.ExtractTranslation();
            
 
            r1.LocalScale = lt.ExtractScale();
           // r1.LocalPos = new OpenTK.Vector3(r1.LocalPos.X + 100, 0, 0);
            for(int i = 0; i < s.MeshCount; i++)
            {
                r1.AddMesh(ml[s.MeshIndices[i]]);
          
            }
            if (s.HasChildren)
            {
                foreach (var pn in s.Children)
                {
             
                    ProcessNode(r1, pn, ml);
                }
            }
        }

        public OpenTK.Vector2 Cv2(Assimp.Vector3D o)
        {
            return new OpenTK.Vector2(o.X, o.Y);
        }
        public OpenTK.Vector3 Cv(Assimp.Vector3D o)
        {
            return new OpenTK.Vector3(o.X, o.Y, o.Z);
        }
    }
}
