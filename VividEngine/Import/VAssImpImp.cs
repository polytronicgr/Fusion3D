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
        Animation.Animator _ta;
        private void ExtractBoneWeightsFromMesh(Mesh mesh, IDictionary<uint, List<VertexWeight>> vertToBoneWeight)
        {
            foreach (var bone in mesh.Bones)
            {
                var boneIndex = _ta.GetBoneIndex(bone.Name);
                // bone weights are recorded per bone in assimp, with each bone containing a list of the vertices influenced by it
                // we really want the reverse mapping, i.e. lookup the vertexID and get the bone id and weight
                // We'll support up to 4 bones per vertex, so we need a list of weights for each vertex
                foreach (var weight in bone.VertexWeights)
                {
                    if (vertToBoneWeight.ContainsKey((uint)weight.VertexID))
                    {
                        vertToBoneWeight[(uint)weight.VertexID].Add(new VertexWeight((int)boneIndex, weight.Weight));
                    }
                    else
                    {
                        vertToBoneWeight[(uint)weight.VertexID] = new List<VertexWeight>(
                            new[] { new VertexWeight((int)boneIndex, weight.Weight) }
                        );
                    }
                }
            }
        }

        public override GraphNode3D LoadAnimNode(string path)
        {

            if (NormBlank == null)
            {
                NormBlank = new Texture.VTex2D("data\\tex\\normblank.png", Texture.LoadMethod.Single, false);
                DiffBlank = new Texture.VTex2D("data\\tex\\diffblank.png", Texture.LoadMethod.Single, false);
                SpecBlank = new Texture.VTex2D("data\\tex\\specblank.png", Texture.LoadMethod.Single, false);
            }



            GraphAnimEntity3D root = new GraphAnimEntity3D();
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
            catch (AssimpException ae)
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

            root.Animator = new Animation.Animator();

            if (s.AnimationCount > 0)
            {
                Console.WriteLine("Processing animations.");
                root.Animator.InitAssImp(s, root);
                Console.WriteLine("Processed.");
                _ta = root.Animator;
            }
            var vertToBoneWeight = new Dictionary<uint, List<VertexWeight>>();




            //s.Animations[0].NodeAnimationChannels[0].
            //s.Animations[0].anim

            //     root.Animator.InitAssImp(model);

            List<Vivid3D.Data.Vertex> _vertices = new List<Vertex>();
            List<Vivid3D.Data.Tri> _tris = new List<Tri>();

            List<Vertex> ExtractVertices(Mesh m,Dictionary<uint,List<VertexWeight>> vtb)
            {
                List<Vertex> rl = new List<Vertex>();

                for(int i = 0; i < m.VertexCount; i++)
                {
                    var pos = m.HasVertices ? m.Vertices[i] : new Assimp.Vector3D();
                    var norm = m.HasNormals ? m.Normals[i] : new Assimp.Vector3D();
                    var tan = m.HasTangentBasis ? m.Tangents[i] : new Assimp.Vector3D();
                    var bi = m.HasTangentBasis ? m.BiTangents[i] : new Assimp.Vector3D();

                    var nv = new Vertex();
                    nv.Pos = new OpenTK.Vector3(pos.X, pos.Y, pos.Z);
                    nv.Norm = new OpenTK.Vector3(norm.X, norm.Y, norm.Z);
                    nv.Tan = new OpenTK.Vector3(tan.X, tan.Y, tan.Z);
                    nv.BiNorm = new OpenTK.Vector3(bi.X, bi.Y, bi.Z);

                    if (m.HasTextureCoords(0))
                    {

                        var coord = m.TextureCoordinateChannels[0][i];
                        nv.UV = new OpenTK.Vector2(coord.X,1- coord.Y);

                    }

                    var weights = vtb[(uint)i].Select(w => w.Weight).ToArray();
                    var boneIndices = vtb[(uint)i].Select(w => (byte)w.VertexID).ToArray();

                    nv.Weight = weights.First();
                    nv.BoneIndices = new int[4];
                    nv.BoneIndices[0] = boneIndices[0];
                    if (boneIndices.Length > 1)
                    {
                        nv.BoneIndices[1] = boneIndices[1];
                    }
                    if (boneIndices.Length > 2)
                    {
                        nv.BoneIndices[2] = boneIndices[2];
                    }
                    if (boneIndices.Length > 3)
                    {
                        nv.BoneIndices[3] = boneIndices[3];
                    }
                    rl.Add(nv);


                }

                return rl;

            }

            root.Mesh = new VMesh();



            foreach (var m in s.Meshes)
            {

                ExtractBoneWeightsFromMesh(m, vertToBoneWeight);
                var sub = new Vivid3D.Data.VMesh.Subset();
                sub.VertexCount = m.VertexCount;
                sub.VertexStart = _vertices.Count;
                sub.FaceStart = _tris.Count;
                sub.FaceCount = m.FaceCount;

                root.Mesh.Subs.Add(sub);

                var verts = ExtractVertices(m, vertToBoneWeight);

                _vertices.AddRange(verts);

                

                var indices = m.GetIndices().Select(i => (short)(i + (uint)sub.VertexStart)).ToList();

                for(int i = 0; i < indices.Count; i+=3)
                {

                    var t = new Tri();
                    t.V0 = indices[i];
                    t.V1 = indices[i + 2];
                    t.v2 = indices[i + 1];
                    _tris.Add(t);

                }




            }

            root.Mesh.VertexData = _vertices.ToArray();
            root.Mesh.TriData = _tris.ToArray();

            root.Mesh.FinalAnim();
            root.Renderer = new Visuals.VRMultiPassAnim();

            root.Meshes.Add(root.Mesh.Clone());
            root.Meshes[0].FinalAnim();

            var m1 = new Material.Material3D();
            m1.TCol = DiffBlank;
            m1.TNorm = NormBlank;
            m1.TSpec = SpecBlank;
            root.Mesh.Mat = m1;
            root.Meshes[0].Mat = root.Mesh.Mat;

            var mat = s.Materials[0];
            TextureSlot t1;

            var sc = mat.GetMaterialTextureCount(TextureType.Unknown);
            Console.WriteLine("SC:" + sc);
            if (mat.HasColorDiffuse)
            {
                m1.Diff = CTV(mat.ColorDiffuse);
            }
            if (mat.HasColorSpecular)
            {
                m1.Spec = CTV(mat.ColorSpecular);
            //    Console.WriteLine("Spec:" + vm.Spec);
            }

            if (mat.HasShininess)
            {

                //vm.Shine = 0.3f+ mat.Shininess;
               // Console.WriteLine("Shine:" + vm.Shine);
            }

            //Console.WriteLine("Spec:" + vm.Spec);
            //for(int ic = 0; ic < sc; ic++)
            ///{
            if (sc > 0)
            {
                var tex2 = mat.GetMaterialTextures(TextureType.Unknown)[0];
                m1.TSpec = new Texture.VTex2D(IPath + "\\" + tex2.FilePath, Texture.LoadMethod.Single, false);
            }

            if (mat.GetMaterialTextureCount(TextureType.Normals) > 0)
            {
                var ntt = mat.GetMaterialTextures(TextureType.Normals)[0];
                Console.WriteLine("Norm:" + ntt.FilePath);
                m1.TNorm = new Texture.VTex2D(IPath + "\\" + ntt.FilePath, Vivid3D.Texture.LoadMethod.Single, false);
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
                        m1.TCol = new Texture.VTex2D(IPath + "\\" + t1.FilePath.Replace(".dds", ".png"), Texture.LoadMethod.Single, false);
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



            //r1.LocalTurn = new OpenTK.Matrix4(s.Transform.A1, s.Transform.A2, s.Transform.A3, s.Transform.A4, s.Transform.B1, s.Transform.B2, s.Transform.B3, s.Transform.B4, s.Transform.C1, s.Transform.C2, s.Transform.C3, s.Transform.C4, s.Transform.D1, s.Transform.D2, s.Transform.D3, s.Transform.D4);
            root.LocalTurn = new OpenTK.Matrix4(s.RootNode.Transform.A1, s.RootNode.Transform.B1, s.RootNode.Transform.C1, s.RootNode.Transform.D1, s.RootNode.Transform.A2, s.RootNode.Transform.B2, s.RootNode.Transform.C2, s.RootNode.Transform.D2, s.RootNode.Transform.A3, s.RootNode.Transform.B3, s.RootNode.Transform.C3, s.RootNode.Transform.D3, s.RootNode.Transform.A4, s.RootNode.Transform.B4, s.RootNode.Transform.C4, s.RootNode.Transform.D4);


            root.LocalTurn = ToTK(s.RootNode.Children[0].Transform);
            var lt = root.LocalTurn;

            root.LocalTurn = lt.ClearTranslation();
            root.LocalTurn = root.LocalTurn.ClearScale();
            root.LocalPos = lt.ExtractTranslation();


            root.LocalScale = lt.ExtractScale();

            root.AnimName = "Run";




            return root;


            /*
            foreach (var m in s.Meshes)
            {



                Console.WriteLine("M:" + m.Name + " Bones:" + m.BoneCount);
                Console.WriteLine("AA:" + m.HasMeshAnimationAttachments);

                var vm = new Material.Material3D();
                vm.TCol = DiffBlank;
                vm.TNorm = NormBlank;
                vm.TSpec = SpecBlank;
                var m2 = new VMesh(m.GetIndices().Length, m.VertexCount);
                ml2.Add(m2);
                // ml.Add(m.Name, m2);
                for (int b = 0; b < m.BoneCount; b++)
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
                    Console.WriteLine("Norm:" + ntt.FilePath);
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
                            vm.TCol = new Texture.VTex2D(IPath + "\\" + t1.FilePath.Replace(".dds", ".png"), Texture.LoadMethod.Single, false);
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

                ExtractBoneWeightsFromMesh(m, vertToBoneWeight);


                for (int i = 0; i < m2.NumVertices; i++)
                {
                    var v = m.Vertices[i];// * new Vector3D(15, 15, 15);
                    var n = m.Normals[i];
                    var t = m.TextureCoordinateChannels[0];
                    Vector3D tan, bi;
                    if (m.Tangents != null && m.Tangents.Count > 0)
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

                        m2.SetVertex(i, Cv(v), Cv(tan), Cv(bi), Cv(n), Cv2(new Vector3D(0, 0, 0)));
                    }
                    else
                    {
                        var tv = t[i];
                        tv.Y = 1.0f - tv.Y;
                        m2.SetVertex(i, Cv(v), Cv(tan), Cv(bi), Cv(n), Cv2(tv));
                    }

                    var weights = vertToBoneWeight[(uint)i].Select(w => w.Weight).ToArray();
                    var boneIndices = vertToBoneWeight[(uint)i].Select(w => (byte)w.VertexID).ToArray();

                    m2.SetVertexBone(i, weights.First(), boneIndices);

                    //var v = new PosNormalTexTanSkinned(pos, norm.ToVector3(), texC.ToVector2(), tan.ToVector3(), weights.First(), boneIndices);
                    //verts.Add(v);


                }
                int[] id = m.GetIndices();
                int fi = 0;
                uint[] nd = new uint[id.Length];
                for (int i = 0; i < id.Length; i += 3)
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

            foreach (var ac in root.Clips)
            {
                Console.WriteLine("Anims:" + ac);
            }
            root.AnimName = "Run";
            /*
            while (true)
            {


            }
            */
            
            return root as GraphNode3D;
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

            //root.Animator = new Animation.Animator();


      
            


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

      
                    //var v = new PosNormalTexTanSkinned(pos, norm.ToVector3(), texC.ToVector2(), tan.ToVector3(), weights.First(), boneIndices);
                    //verts.Add(v);


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

            /*
            while (true)
            {


            }
            */
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
