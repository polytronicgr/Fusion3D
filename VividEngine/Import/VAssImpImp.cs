using Assimp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vivid3D.Data;
using Vivid3D.Scene;

namespace Vivid3D.Import
{
    public class AssImpImport : Importer
    {
        public static string IPath = "";
        public static Texture.VTex2D NormBlank = null, DiffBlank, SpecBlank, MetalBlank;
        public static float ScaleX = 1, ScaleY = 1, ScaleZ = 1;
        private Animation.Animator _ta;

        public static OpenTK.Vector3 CTV ( Color4D col )
        {
            return new OpenTK.Vector3 ( col.R, col.G, col.B );
        }

        public OpenTK.Vector3 Cv ( Assimp.Vector3D o )
        {
            return new OpenTK.Vector3 ( o.X, o.Y, o.Z );
        }

        public OpenTK.Vector2 Cv2 ( Assimp.Vector3D o )
        {
            return new OpenTK.Vector2 ( o.X, o.Y );
        }

        public void ExtractBoneWeights ( Mesh mesh, IDictionary<uint, List<VertexWeight>> vertexToBoneWeight )
        {
            foreach ( Bone bone in mesh.Bones )
            {
                // int bone
            }
        }

        public override GraphNode3D LoadAnimNode ( string path )
        {
            if ( NormBlank == null )
            {
                NormBlank = new Texture.VTex2D ( "data\\tex\\normblank.png", Texture.LoadMethod.Single, false );
                DiffBlank = new Texture.VTex2D ( "data\\tex\\diffblank.png", Texture.LoadMethod.Single, false );
                SpecBlank = new Texture.VTex2D ( "data\\tex\\specblank.png", Texture.LoadMethod.Single, false );
            }

            GraphAnimEntity3D root = new GraphAnimEntity3D();
            string file = path;

            AssimpContext e = new Assimp.AssimpContext();
            Assimp.Configs.NormalSmoothingAngleConfig c1 = new Assimp.Configs.NormalSmoothingAngleConfig ( 75 );
            e.SetConfig ( c1 );

            Console.WriteLine ( "Impporting:" + file );
            Assimp.Scene s = null;
            try
            {
                s = e.ImportFile ( file, PostProcessSteps.OptimizeMeshes | PostProcessSteps.OptimizeGraph | PostProcessSteps.FindInvalidData | PostProcessSteps.FindDegenerates | PostProcessSteps.Triangulate | PostProcessSteps.ValidateDataStructure | PostProcessSteps.CalculateTangentSpace );
            }
            catch ( AssimpException ae )
            {
                Console.WriteLine ( ae );
                Console.WriteLine ( "Failed to import" );
                Environment.Exit ( -1 );
            }
            Console.WriteLine ( "Imported." );
            Dictionary<string, VMesh> ml = new Dictionary<string, VMesh>();
            List<VMesh> ml2 = new List<VMesh>();
            Console.WriteLine ( "animCount:" + s.AnimationCount );

            Matrix4x4 tf = s.RootNode.Transform;

            tf.Inverse ( );

            root.GlobalInverse = ToTK ( tf );

            Dictionary<uint , List<VertexWeight>> boneToWeight = new Dictionary<uint , List<VertexWeight>> ( );

            root.Animator = new Animation.Animator ( );

            if ( s.AnimationCount > 0 )
            {
                Console.WriteLine ( "Processing animations." );
                root.Animator.InitAssImp ( s, root );
                Console.WriteLine ( "Processed." );
                _ta = root.Animator;
            }
            Dictionary<uint , List<VertexWeight>> vertToBoneWeight = new Dictionary<uint , List<VertexWeight>> ( );

            //s.Animations[0].NodeAnimationChannels[0].
            //s.Animations[0].anim

            // root.Animator.InitAssImp(model);

            List<Vivid3D.Data.Vertex> _vertices = new List<Vertex>();
            List<Vivid3D.Data.Tri> _tris = new List<Tri>();

            List<Vertex> ExtractVertices ( Mesh m, Dictionary<uint, List<VertexWeight>> vtb )
            {
                List<Vertex> rl = new List<Vertex>();

                for ( int i = 0; i < m.VertexCount; i++ )
                {
                    Vector3D pos = m.HasVertices ? m.Vertices[i] : new Assimp.Vector3D();
                    Vector3D norm = m.HasNormals ? m.Normals[i] : new Assimp.Vector3D();
                    Vector3D tan = m.HasTangentBasis ? m.Tangents[i] : new Assimp.Vector3D();
                    Vector3D bi = m.HasTangentBasis ? m.BiTangents[i] : new Assimp.Vector3D();

                    Vertex nv = new Vertex
                    {
                        Pos = new OpenTK.Vector3 ( pos.X , pos.Y , pos.Z ) ,
                        Norm = new OpenTK.Vector3 ( norm.X , norm.Y , norm.Z ) ,
                        Tan = new OpenTK.Vector3 ( tan.X , tan.Y , tan.Z ) ,
                        BiNorm = new OpenTK.Vector3 ( bi.X , bi.Y , bi.Z )
                    };

                    if ( m.HasTextureCoords ( 0 ) )
                    {
                        Vector3D coord = m.TextureCoordinateChannels[0][i];
                        nv.UV = new OpenTK.Vector2 ( coord.X, 1 - coord.Y );
                    }

                    float [ ] weights = vtb[(uint)i].Select(w => w.Weight).ToArray();
                    byte [ ] boneIndices = vtb[(uint)i].Select(w => (byte)w.VertexID).ToArray();

                    nv.Weight = weights.First ( );
                    nv.BoneIndices = new int [ 4 ];
                    nv.BoneIndices [ 0 ] = boneIndices [ 0 ];
                    if ( boneIndices.Length > 1 )
                    {
                        nv.BoneIndices [ 1 ] = boneIndices [ 1 ];
                    }
                    if ( boneIndices.Length > 2 )
                    {
                        nv.BoneIndices [ 2 ] = boneIndices [ 2 ];
                    }
                    if ( boneIndices.Length > 3 )
                    {
                        nv.BoneIndices [ 3 ] = boneIndices [ 3 ];
                    }
                    rl.Add ( nv );
                }

                return rl;
            }

            root.Mesh = new VMesh ( );

            foreach ( Mesh m in s.Meshes )
            {
                ExtractBoneWeightsFromMesh ( m, vertToBoneWeight );
                VMesh.Subset sub = new Vivid3D.Data.VMesh.Subset
                {
                    VertexCount = m.VertexCount ,
                    VertexStart = _vertices.Count ,
                    FaceStart = _tris.Count ,
                    FaceCount = m.FaceCount
                };

                root.Mesh.Subs.Add ( sub );

                List<Vertex> verts = ExtractVertices ( m , vertToBoneWeight );

                _vertices.AddRange ( verts );

                List<short> indices = m.GetIndices ( ).Select ( i => ( short ) ( i + ( uint ) sub.VertexStart ) ).ToList ( );

                for ( int i = 0; i < indices.Count; i += 3 )
                {
                    Tri t = new Tri
                    {
                        V0 = indices [ i ] ,
                        V1 = indices [ i + 2 ] ,
                        v2 = indices [ i + 1 ]
                    };
                    _tris.Add ( t );
                }
            }

            root.Mesh.VertexData = _vertices.ToArray ( );
            root.Mesh.TriData = _tris.ToArray ( );

            root.Mesh.FinalAnim ( );
            root.Renderer = new Visuals.VRMultiPassAnim ( );

            root.Meshes.Add ( root.Mesh.Clone ( ) );
            root.Meshes [ 0 ].FinalAnim ( );

            Material.Material3D m1 = new Material.Material3D
            {
                TCol = DiffBlank ,
                TNorm = NormBlank ,
                TSpec = SpecBlank
            };
            root.Mesh.Mat = m1;
            root.Meshes [ 0 ].Mat = root.Mesh.Mat;

            Assimp.Material mat = s.Materials [ 0 ];
            TextureSlot t1;

            int sc = mat.GetMaterialTextureCount(TextureType.Unknown);
            Console.WriteLine ( "SC:" + sc );
            if ( mat.HasColorDiffuse )
            {
                m1.Diff = CTV ( mat.ColorDiffuse );
            }
            if ( mat.HasColorSpecular )
            {
                m1.Spec = CTV ( mat.ColorSpecular );
                // Console.WriteLine("Spec:" + vm.Spec);
            }

            if ( mat.HasShininess )
            {
                //vm.Shine = 0.3f+ mat.Shininess;
                // Console.WriteLine("Shine:" + vm.Shine);
            }

            //Console.WriteLine("Spec:" + vm.Spec);
            //for(int ic = 0; ic < sc; ic++)
            ///{
            if ( sc > 0 )
            {
                TextureSlot tex2 = mat.GetMaterialTextures(TextureType.Unknown)[0];
                m1.TSpec = new Texture.VTex2D ( IPath + "\\" + tex2.FilePath, Texture.LoadMethod.Single, false );
            }

            if ( mat.GetMaterialTextureCount ( TextureType.Normals ) > 0 )
            {
                TextureSlot ntt = mat.GetMaterialTextures(TextureType.Normals)[0];
                Console.WriteLine ( "Norm:" + ntt.FilePath );
                m1.TNorm = new Texture.VTex2D ( IPath + "\\" + ntt.FilePath, Vivid3D.Texture.LoadMethod.Single, false );
            }

            if ( mat.GetMaterialTextureCount ( TextureType.Diffuse ) > 0 )
            {
                t1 = mat.GetMaterialTextures ( TextureType.Diffuse ) [ 0 ];
                Console.WriteLine ( "DiffTex:" + t1.FilePath );

                if ( t1.FilePath != null )
                {
                    try
                    {
                        // Console.Write("t1:" + t1.FilePath);
                        m1.TCol = new Texture.VTex2D ( IPath + "\\" + t1.FilePath.Replace ( ".dds", ".png" ), Texture.LoadMethod.Single, false );
                        if ( File.Exists ( IPath + "norm" + t1.FilePath ) )
                        {
                            // vm.TNorm = new Texture.VTex2D(IPath + "norm" +
                            // t1.FilePath,Texture.LoadMethod.Single, false);

                            // Console.WriteLine("TexLoaded");
                        }
                    }
                    catch
                    {
                    }
                }
                if ( true )
                {
                    if ( new FileInfo ( t1.FilePath ).Exists == true )
                    {
                        //  var tex = App.AppSal.CreateTex2D();
                        //  tex.Path = t1.FilePath;
                        // tex.Load();
                        //m2.DiffuseMap = tex;
                    }
                }
            }

            //r1.LocalTurn = new OpenTK.Matrix4(s.Transform.A1, s.Transform.A2, s.Transform.A3, s.Transform.A4, s.Transform.B1, s.Transform.B2, s.Transform.B3, s.Transform.B4, s.Transform.C1, s.Transform.C2, s.Transform.C3, s.Transform.C4, s.Transform.D1, s.Transform.D2, s.Transform.D3, s.Transform.D4);
            root.LocalTurn = new OpenTK.Matrix4 ( s.RootNode.Transform.A1, s.RootNode.Transform.B1, s.RootNode.Transform.C1, s.RootNode.Transform.D1, s.RootNode.Transform.A2, s.RootNode.Transform.B2, s.RootNode.Transform.C2, s.RootNode.Transform.D2, s.RootNode.Transform.A3, s.RootNode.Transform.B3, s.RootNode.Transform.C3, s.RootNode.Transform.D3, s.RootNode.Transform.A4, s.RootNode.Transform.B4, s.RootNode.Transform.C4, s.RootNode.Transform.D4 );

            root.LocalTurn = ToTK ( s.RootNode.Children [ 0 ].Transform );
            OpenTK.Matrix4 lt = root.LocalTurn;

            root.LocalTurn = lt.ClearTranslation ( );
            root.LocalTurn = root.LocalTurn.ClearScale ( );
            root.LocalPos = lt.ExtractTranslation ( );

            root.LocalScale = lt.ExtractScale ( );

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
                            // Console.Write("t1:" + t1.FilePath);
                            vm.TCol = new Texture.VTex2D(IPath + "\\" + t1.FilePath.Replace(".dds", ".png"), Texture.LoadMethod.Single, false);
                            if (File.Exists(IPath + "norm" + t1.FilePath))
                            {
                                // vm.TNorm = new Texture.VTex2D(IPath + "norm" +
                                // t1.FilePath,Texture.LoadMethod.Single, false);

                                // Console.WriteLine("TexLoaded");
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

        public override GraphNode3D LoadNode ( string path )
        {
            if ( NormBlank == null )
            {
                NormBlank = new Texture.VTex2D ( "data\\tex\\normblank.png", Texture.LoadMethod.Single, false );
                DiffBlank = new Texture.VTex2D ( "data\\tex\\diffblank.png", Texture.LoadMethod.Single, false );
                SpecBlank = new Texture.VTex2D ( "data\\tex\\specblank.png", Texture.LoadMethod.Single, false );
            }

            GraphEntity3D root = new GraphEntity3D();
            string file = path;

            AssimpContext e = new Assimp.AssimpContext();
            Assimp.Configs.NormalSmoothingAngleConfig c1 = new Assimp.Configs.NormalSmoothingAngleConfig ( 75 );
            e.SetConfig ( c1 );

            Console.WriteLine ( "Impporting:" + file );
            Assimp.Scene s = null;
            try
            {
                s = e.ImportFile ( file, PostProcessSteps.OptimizeMeshes | PostProcessSteps.OptimizeGraph | PostProcessSteps.FindInvalidData | PostProcessSteps.FindDegenerates | PostProcessSteps.Triangulate | PostProcessSteps.ValidateDataStructure | PostProcessSteps.CalculateTangentSpace | PostProcessSteps.GenerateNormals );
                if ( s.HasAnimations )
                {
                    return LoadAnimNode ( path );
                }
            }
            catch ( AssimpException ae )
            {
                Console.WriteLine ( ae );
                Console.WriteLine ( "Failed to import" );
                Environment.Exit ( -1 );
            }
            Console.WriteLine ( "Imported." );
            Dictionary<string, VMesh> ml = new Dictionary<string, VMesh>();
            List<VMesh> ml2 = new List<VMesh>();
            Console.WriteLine ( "animCount:" + s.AnimationCount );

            Matrix4x4 tf = s.RootNode.Transform;

            tf.Inverse ( );

            root.GlobalInverse = ToTK ( tf );

            Dictionary<uint , List<VertexWeight>> boneToWeight = new Dictionary<uint , List<VertexWeight>> ( );

            //root.Animator = new Animation.Animator();

            //s.Animations[0].NodeAnimationChannels[0].
            //s.Animations[0].anim

            // root.Animator.InitAssImp(model);

            foreach ( Mesh m in s.Meshes )
            {
                Console.WriteLine ( "M:" + m.Name + " Bones:" + m.BoneCount );
                Console.WriteLine ( "AA:" + m.HasMeshAnimationAttachments );

                Material.Material3D vm = new Material.Material3D
                {
                    TCol = DiffBlank ,
                    TNorm = NormBlank ,
                    TSpec = SpecBlank
                };
                VMesh m2 = new VMesh(m.GetIndices().Length, m.VertexCount);
                ml2.Add ( m2 );
                // ml.Add(m.Name, m2);
                for ( int b = 0; b < m.BoneCount; b++ )
                {
                    string name = m.Bones[b].Name;
                }
                m2.Mat = vm;
                // root.AddMesh(m2);
                m2.Name = m.Name;
                Assimp.Material mat = s.Materials [ m.MaterialIndex ];
                TextureSlot t1;

                int sc = mat.GetMaterialTextureCount(TextureType.Unknown);
                Console.WriteLine ( "SC:" + sc );
                if ( mat.HasColorDiffuse )
                {
                    // vm.Diff = CTV ( mat.ColorDiffuse );
                    Console.WriteLine ( "Diff:" + vm.Diff );
                }
                if ( mat.HasColorSpecular )
                {
                    // vm.Spec = CTV ( mat.ColorSpecular );
                    Console.WriteLine ( "Spec:" + vm.Spec );
                }
                if ( mat.HasShininess )
                {
                    //vm.Shine = 0.3f+ mat.Shininess;
                    Console.WriteLine ( "Shine:" + vm.Shine );
                }

                Console.WriteLine ( "Spec:" + vm.Spec );
                //for(int ic = 0; ic < sc; ic++)
                ///{
                if ( sc > 0 )
                {
                    TextureSlot tex2 = mat.GetMaterialTextures(TextureType.Unknown)[0];
                    vm.TSpec = new Texture.VTex2D ( IPath + "\\" + tex2.FilePath, Texture.LoadMethod.Single, false );
                }

                if ( mat.GetMaterialTextureCount ( TextureType.Normals ) > 0 )
                {
                    TextureSlot ntt = mat.GetMaterialTextures(TextureType.Normals)[0];
                    Console.WriteLine ( "Norm:" + ntt.FilePath );
                    vm.TNorm = new Texture.VTex2D ( IPath + "\\" + ntt.FilePath, Vivid3D.Texture.LoadMethod.Single, false );
                }

                if ( mat.GetMaterialTextureCount ( TextureType.Diffuse ) > 0 )
                {
                    t1 = mat.GetMaterialTextures ( TextureType.Diffuse ) [ 0 ];
                    Console.WriteLine ( "DiffTex:" + t1.FilePath );

                    if ( t1.FilePath != null )
                    {
                        //Console.WriteLine ( "Tex:" + t1.FilePath );
                        // Console.Write("t1:" + t1.FilePath);
                        vm.TCol = new Texture.VTex2D ( IPath + "\\" + t1.FilePath.Replace ( ".dds", ".png" ), Texture.LoadMethod.Single, false );
                        if ( File.Exists ( IPath + "\\" + "norm_" + t1.FilePath ) )
                        {
                            vm.TNorm = new Texture.VTex2D ( IPath + "\\" + "norm_" + t1.FilePath, Texture.LoadMethod.Single, false );
                        }
                    }
                }

                for ( int i = 0; i < m2.NumVertices; i++ )
                {
                    Vector3D v = m.Vertices[i];// * new Vector3D(15, 15, 15);
                    Vector3D n = m.Normals[i];
                    List<Vector3D> t = m.TextureCoordinateChannels [ 0 ];
                    Vector3D tan, bi;
                    if ( m.Tangents != null && m.Tangents.Count > 0 )
                    {
                        tan = m.Tangents [ i ];
                        bi = m.BiTangents [ i ];
                    }
                    else
                    {
                        tan = new Vector3D ( 0, 0, 0 );
                        bi = new Vector3D ( 0, 0, 0 );
                    }
                    if ( t.Count ( ) == 0 )
                    {
                        m2.SetVertex ( i, Cv ( v ), Cv ( tan ), Cv ( bi ), Cv ( n ), Cv2 ( new Vector3D ( 0, 0, 0 ) ) );
                    }
                    else
                    {
                        Vector3D tv = t[i];
                        tv.Y = 1.0f - tv.Y;
                        m2.SetVertex ( i, Cv ( v ), Cv ( tan ), Cv ( bi ), Cv ( n ), Cv2 ( tv ) );
                    }

                    //var v = new PosNormalTexTanSkinned(pos, norm.ToVector3(), texC.ToVector2(), tan.ToVector3(), weights.First(), boneIndices);
                    //verts.Add(v);
                }
                int[] id = m.GetIndices();
                uint[] nd = new uint[id.Length];
                for ( int i = 0; i < id.Length; i += 3 )
                {
                    //Tri t = new Tri();
                    //t.V0 = (int)nd[i];
                    // t.V1 = (int)nd[i + 1];
                    // t.v2 = (int)nd[i + 2];

                    // nd[i] = (uint)id[i];
                    m2.SetTri ( i / 3, id [ i ], id [ i + 1 ], id [ i + 2 ] );
                }

                m2.Indices = nd;
                //m2.Scale(AssImpImport.ScaleX, AssImpImport.ScaleY, AssImpImport.ScaleZ);
                //m2.GenerateTangents ( );

                m2.Final ( );
            }

            ProcessNode ( root, s.RootNode, ml2 );

            /*
            while (true)
            {
            }
            */
            return root as GraphNode3D;
        }

        private void ExtractBoneWeightsFromMesh ( Mesh mesh, IDictionary<uint, List<VertexWeight>> vertToBoneWeight )
        {
            foreach ( Bone bone in mesh.Bones )
            {
                int boneIndex = _ta.GetBoneIndex(bone.Name);
                // bone weights are recorded per bone in assimp, with each bone containing a list of
                // the vertices influenced by it we really want the reverse mapping, i.e. lookup the
                // vertexID and get the bone id and weight We'll support up to 4 bones per vertex, so
                // we need a list of weights for each vertex
                foreach ( VertexWeight weight in bone.VertexWeights )
                {
                    if ( vertToBoneWeight.ContainsKey ( ( uint ) weight.VertexID ) )
                    {
                        vertToBoneWeight [ ( uint ) weight.VertexID ].Add ( new VertexWeight ( boneIndex, weight.Weight ) );
                    }
                    else
                    {
                        vertToBoneWeight [ ( uint ) weight.VertexID ] = new List<VertexWeight> (
                            new [ ] { new VertexWeight ( boneIndex, weight.Weight ) }
                        );
                    }
                }
            }
        }

        private void ProcessNode ( GraphEntity3D root, Assimp.Node s, List<VMesh> ml )
        {
            GraphEntity3D r1 = new GraphEntity3D();
            root.Sub.Add ( r1 );
            r1.Top = root;
            r1.Name = s.Name;
            if ( s.Name.ToLower ( ).Contains ( "root" ) )
            {
                r1.Name = r1.Name + "*";
                r1.BreakTop = true;
            }

            //r1.LocalTurn = new OpenTK.Matrix4(s.Transform.A1, s.Transform.A2, s.Transform.A3, s.Transform.A4, s.Transform.B1, s.Transform.B2, s.Transform.B3, s.Transform.B4, s.Transform.C1, s.Transform.C2, s.Transform.C3, s.Transform.C4, s.Transform.D1, s.Transform.D2, s.Transform.D3, s.Transform.D4);
            r1.LocalTurn = new OpenTK.Matrix4 ( s.Transform.A1, s.Transform.B1, s.Transform.C1, s.Transform.D1, s.Transform.A2, s.Transform.B2, s.Transform.C2, s.Transform.D2, s.Transform.A3, s.Transform.B3, s.Transform.C3, s.Transform.D3, s.Transform.A4, s.Transform.B4, s.Transform.C4, s.Transform.D4 );
            OpenTK.Matrix4 lt = r1.LocalTurn;

            r1.LocalTurn = lt.ClearTranslation ( );
            r1.LocalTurn = r1.LocalTurn.ClearScale ( );
            r1.LocalPos = lt.ExtractTranslation ( );

            r1.LocalScale = lt.ExtractScale ( );
            // r1.LocalPos = new OpenTK.Vector3(r1.LocalPos.X + 100, 0, 0);
            for ( int i = 0; i < s.MeshCount; i++ )
            {
                r1.AddMesh ( ml [ s.MeshIndices [ i ] ] );
            }
            if ( s.HasChildren )
            {
                foreach ( Node pn in s.Children )
                {
                    ProcessNode ( r1, pn, ml );
                }
            }
        }

        private OpenTK.Matrix4 ToTK ( Matrix4x4 mat )
        {
            return new OpenTK.Matrix4 ( mat.A1, mat.B1, mat.C1, mat.D1, mat.A2, mat.B2, mat.C2, mat.D2, mat.A3, mat.B3, mat.C3, mat.D3, mat.A4, mat.B4, mat.C4, mat.D4 );
        }
    }
}