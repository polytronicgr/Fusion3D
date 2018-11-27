namespace FusionEngine.Effect
{
    public class FXNoFX : Effect3D
    {
        public FXNoFX ( ) : base ( "", "data/Shader/vsNoFX.glsl", "data/Shader/fsNoFX.glsl" )
        {
        }

        public override void SetPars ( )
        {
            SetMat ( "model", Effect.FXG.Local );
            SetMat ( "view", FXG.Cam.CamWorld );
            SetMat ( "proj", FXG.Cam.ProjMat );
            SetTex ( "tC", 0 );
        }
    }

    public class FXLightMap : Effect3D
    {
        public static float LightMod = 1.0f;
        public static float MatMod = 1.0f;

        public FXLightMap ( ) : base ( "", "data/Shader/vsLightMap.glsl", "data/Shader/fsLightMap.glsl" )
        {
        }

        public override void SetPars ( )
        {
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat ( "model", Effect.FXG.Local );
            SetMat ( "view", FXG.Cam.CamWorld );
            SetMat ( "proj", FXG.Cam.ProjMat );

            SetTex ( "tC", 0 );

            //SetTex("tSpec", 2);
        }
    }

    public class FXTerrain : Effect3D
    {
        public static float LightMod = 1.0f;
        public static float MatMod = 1.0f;

        public FXTerrain ( ) : base ( "", "data/Shader/vsTerrain.glsl", "data/Shader/fsTerrain.glsl" )
        {
        }

        public override void SetPars ( )
        {
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat ( "model", Effect.FXG.Local );
            SetMat ( "view", FXG.Cam.CamWorld );
            SetMat ( "proj", FXG.Cam.ProjMat );
            SetFloat ( "lightDepth", Settings.Quality.ShadowDepth );
            SetVec3 ( "viewPos", FXG.Cam.LocalPos );
            SetVec3 ( "lightPos", Lighting.Light3D.Active.WorldPos );
            SetVec3 ( "lightCol", Lighting.Light3D.Active.Diff * LightMod );
            SetVec3 ( "lightSpec", Lighting.Light3D.Active.Spec * LightMod );
            SetFloat ( "lightRange", Lighting.Light3D.Active.Range );
            SetFloat ( "atten", Lighting.Light3D.Active.Atten );
            SetVec3 ( "ambCE", Lighting.Light3D.Active.Amb * LightMod );
            SetFloat ( "matS", Material.Material3D.Active.Shine * MatMod );
            SetVec3 ( "matSpec", Material.Material3D.Active.Spec * MatMod );
            SetFloat ( "envS", Material.Material3D.Active.envS * MatMod );
            SetVec3 ( "matDiff", Material.Material3D.Active.Diff * MatMod );
            SetTex ( "tC", 0 );
            SetTex ( "tN", 1 );
            //SetTex("tSpec", 2);

            SetTex ( "tS", 2 );
            SetTex ( "tSpec", 3 );
        }
    }

    public class FXMultiPass3D : Effect3D
    {
        public static float LightMod = 1.0f;
        public static float MatMod = 1.0f;

        public FXMultiPass3D ( ) : base ( "", "data/Shader/vsMP1.glsl", "data/Shader/fsMP1.glsl" )
        {
        }

        public override void SetPars ( )
        {
            //SetMat("MVP", Effect.FXG.Local * FXG.Proj);
            SetMat ( "model", Effect.FXG.Local );
            SetMat ( "view", FXG.Cam.CamWorld );
            SetMat ( "proj", FXG.Cam.ProjMat );
            SetFloat ( "lightDepth", Settings.Quality.ShadowDepth );
            SetVec3 ( "viewPos", FXG.Cam.LocalPos );
            SetVec3 ( "lightPos", Lighting.Light3D.Active.WorldPos );
            SetVec3 ( "lightCol", Lighting.Light3D.Active.Diff * LightMod );
            SetVec3 ( "lightSpec", Lighting.Light3D.Active.Spec * LightMod );
            SetFloat ( "lightRange", Lighting.Light3D.Active.Range );
            SetFloat ( "atten", Lighting.Light3D.Active.Atten );
            SetVec3 ( "ambCE", Lighting.Light3D.Active.Amb * LightMod );
            SetFloat ( "matS", Material.Material3D.Active.Shine * MatMod );
            SetVec3 ( "matSpec", Material.Material3D.Active.Spec * MatMod );
            SetFloat ( "envS", Material.Material3D.Active.envS * MatMod );
            SetVec3 ( "matDiff", Material.Material3D.Active.Diff * MatMod );
            SetTex ( "tC", 0 );
            SetTex ( "tN", 1 );
            //SetTex("tSpec", 2);

            SetTex ( "tS", 2 );
            SetTex ( "tSpec", 3 );
        }
    }
}