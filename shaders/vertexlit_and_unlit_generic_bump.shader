//=========================================================================================================================
// Optional
//=========================================================================================================================
HEADER
{
	// CompileTargets = ( IS_SM_50 && ( PC || VULKAN ) );
	Description = "SourceBox vertex lit and unlit generic bump shader. Use when the material has a bumpmap. See license.txt for license information";
}

//=========================================================================================================================
// Optional
//=========================================================================================================================
FEATURES
{
    #include "sourcebox/common/legacy_features.hlsl"

    Feature( F_CUBEMAP,                         0..1, "Rendering" );
    Feature( F_CUSTOM_CUBEMAP,                  0..1, "Rendering" );
    FeatureRule( Requires1( F_CUSTOM_CUBEMAP, F_CUBEMAP ), "" );

    Feature( F_DIFFUSELIGHTING,                 0..1, "Rendering" );
    Feature( F_LIGHTWARPTEXTURE,                0..1, "Rendering" );
    Feature( F_SELFILLUM,                       0..1, "Rendering" );
    Feature( F_SELFILLUMFRESNEL,                0..1, "Rendering" );
    Feature( F_NORMALMAPALPHAENVMAPMASK,        0..1, "Rendering" );
    Feature( F_HALFLAMBERT,                     0..1, "Rendering" );
    Feature( F_BLENDTINTBYBASEALPHA,            0..1, "Rendering" );

    Feature( F_DETAILTEXTURE,                  0..1, "Rendering" );
    Feature( F_DETAIL_BLEND_MODE, 0..11(0="RGB_EQUALS_BASE_x_DETAILx2",1="RGB_ADDITIVE",2="DETAIL_OVER_BASE",3="FADE",4="BASE_OVER_DETAIL",5="RGB_ADDITIVE_SELFILLUM",6="RGB_ADDITIVE_SELFILLUM_THRESHOLD_FADE",7="MOD2X_SELECT_TWO_PATTERNS",8="MULTIPLY",9="MASK_BASE_BY_DETAIL_ALPHA",10="SSBUMP_BUMP",11="SSBUMP_NOBUMP"), "Rendering" );
    FeatureRule( Requires1( F_DETAIL_BLEND_MODE, F_DETAILTEXTURE ), "Requires detail texture" );

    FeatureRule(Requires1(F_LIGHTWARPTEXTURE, F_DIFFUSELIGHTING), "Lightwarp requires diffuse lighting.");
    FeatureRule(Requires1(F_NORMALMAPALPHAENVMAPMASK, F_CUBEMAP), "Envmap mask requires cubemap.");

    FeatureRule(Requires1(F_SELFILLUMFRESNEL, F_SELFILLUM), "");
    // not necessary, but was a constraint of the original shader
    FeatureRule(Allow1(F_LIGHTWARPTEXTURE, F_SELFILLUMFRESNEL), "");

    FeatureRule(Allow1(F_BLENDTINTBYBASEALPHA, F_SELFILLUM), "");

    // this is enforced in SDK code
	FeatureRule( Allow1( F_ALPHA_TEST, F_SELFILLUM ), "" );
	// FeatureRule( Allow1( F_ALPHA_TEST, F_BASEALPHAENVMAPMASK ), "" );
}

//=========================================================================================================================
// Optional
//=========================================================================================================================
MODES
{
    VrForward();													// Indicates this shader will be used for main rendering
    Depth( "depth_only.shader" ); 									// Shader that will be used for shadowing and depth prepass
    ToolsVis( S_MODE_TOOLS_VIS ); 									// Ability to see in the editor
    // ToolsWireframe( "tools_wireframe.shader" ); 					// Allows for mat_wireframe to work
	// ToolsShadingComplexity( "tools_shading_complexity.shader" ); 	// Shows how expensive drawing is in debug view
}

//=========================================================================================================================
COMMON
{
    #include "system.fxc" // This should always be the first include in COMMON
    #include "sbox_shared.fxc"
    #define VS_INPUT_HAS_TANGENT_BASIS 1
    #define PS_INPUT_HAS_TANGENT_BASIS 1

    // vDetailTextureCoords in pixelinput.hlsl
    StaticCombo( S_DETAILTEXTURE                    , F_DETAILTEXTURE                   , Sys( ALL ) );
    #define S_DETAIL_TEXTURE S_DETAILTEXTURE
}

//=========================================================================================================================

struct VertexInput
{
	#include "common/vertexinput.hlsl"
};

//=========================================================================================================================

struct PixelInput
{
	#include "common/pixelinput.hlsl"
};

//=========================================================================================================================

VS
{
	#include "common/vertex.hlsl"

	//
	// Main
	//
    
	PixelInput MainVs( INSTANCED_SHADER_PARAMS( VS_INPUT i ) )
	{
		PixelInput o = ProcessVertex( i );

		#if S_DETAILTEXTURE
            o.vDetailTextureCoords = i.vTexCoord.xy;
        #endif // S_DETAILTEXTURE

		return FinalizeVertex( o );
	}
}

//=========================================================================================================================

PS
{
    StaticCombo( S_CUBEMAP                          , F_CUBEMAP                         , Sys( ALL ) );
    StaticCombo( S_CUSTOM_CUBEMAP                   , F_CUSTOM_CUBEMAP                  , Sys( ALL ) );
    StaticCombo( S_DIFFUSELIGHTING                  , F_DIFFUSELIGHTING                 , Sys( ALL ) );
    StaticCombo( S_LIGHTWARPTEXTURE                 , F_LIGHTWARPTEXTURE                , Sys( ALL ) );
    StaticCombo( S_SELFILLUM                        , F_SELFILLUM                       , Sys( ALL ) );
    StaticCombo( S_SELFILLUMFRESNEL                 , F_SELFILLUMFRESNEL                , Sys( ALL ) );
    StaticCombo( S_NORMALMAPALPHAENVMAPMASK         , F_NORMALMAPALPHAENVMAPMASK        , Sys( ALL ) );
    StaticCombo( S_HALFLAMBERT                      , F_HALFLAMBERT                     , Sys( ALL ) );
    StaticCombo( S_BLENDTINTBYBASEALPHA             , F_BLENDTINTBYBASEALPHA            , Sys( ALL ) );

    // StaticCombo( S_DETAIL_BLEND_MODE                , F_DETAIL_BLEND_MODE               , Sys( ALL ) );
    // dynamic param to save on compile time
    int g_nDetailBlendMode < Expression(F_DETAIL_BLEND_MODE); >;
    #define DETAIL_BLEND_MODE g_nDetailBlendMode

    // DynamicCombo( D_AMBIENT_LIGHT, 0..1, Sys( ALL ) );
    #define D_AMBIENT_LIGHT                    1

    #define USE_MANUAL_CUBEMAP (S_CUBEMAP && S_CUSTOM_CUBEMAP)
    #if S_BLENDTINTBYBASEALPHA
        #define BASE_COLOR_ALPHA_NAME   TintMask
    #elif ( !S_SELFILLUM && !S_BLENDTINTBYBASEALPHA )
        #define BASE_COLOR_ALPHA_NAME   Translucency
    #elif S_SELFILLUM
        #define BASE_COLOR_ALPHA_NAME   SelfIllumMaskTexture
    #endif

    #if S_NORMALMAPALPHAENVMAPMASK
        #define NORMAL_ALPHA_NAME       EnvmapMask
    #endif // S_NORMALMAPALPHAENVMAPMASK

    float3 GetEnvmapColor( float3 envmapBase, float3 specularFactor, float fresnelRanges )
    {
        float3 specularLighting = float3(0.0, 0.0, 0.0);
        
        #if S_CUBEMAP
            specularLighting = g_flEnvMapScale * envmapBase;
            specularLighting *= specularFactor;
            specularLighting *= g_vEnvMapTint.rgb;
            float3 specularLightingSquared = specularLighting * specularLighting;
            specularLighting = lerp( specularLighting, specularLightingSquared, g_vEnvMapContrast );
            float3 greyScale = dot( specularLighting, float3( 0.299f, 0.587f, 0.114f ) );
            specularLighting = lerp( greyScale, specularLighting, g_vEnvMapSaturation );
        #endif // S_CUBEMAP

        return specularLighting;
    }

    #include "sourcebox/common/legacy_pixel.hlsl"

	float4 MainPs( PixelInput i ) : SV_Target0
	{
        ShadingLegacyConfig config = ShadingLegacyConfig::GetDefault();
        config.DoDiffuse = S_DIFFUSELIGHTING ? true : false;
        config.HalfLambert = S_HALFLAMBERT ? true : false;
        config.DoAmbientOcclusion = true;
        config.DoLightingWarp = S_LIGHTWARPTEXTURE ? true : false;
        config.DoRimLighting = false;
        config.DoSpecularWarp = false;
        config.DoSpecular = false;

        config.SelfIllum = S_SELFILLUM ? true : false;
        config.SelfIllumFresnel = S_SELFILLUMFRESNEL ? true : false;

        config.StaticLight = false;
        config.AmbientLight = D_AMBIENT_LIGHT ? true : false;

        float2 vUV = i.vTextureCoords.xy;
        Material m = GetDefaultLegacyMaterial();
        float4 baseColor = CONVERT_COLOR(Tex2DS( g_tColor, TextureFiltering, vUV ));
        #if S_DETAILTEXTURE
            // was packed into a vec4 in the sdk
            // float4 detailColor = Tex2D( g_tDetailTexture, i.vTextureCoords.zw );
            float4 detailColor = CONVERT_DETAIL(Tex2DS( g_tDetailTexture, TextureFiltering, i.vDetailTextureCoords.xy ));
            baseColor = TextureCombine( baseColor, detailColor, DETAIL_BLEND_MODE, g_flDetailBlendFactor );
        #endif // S_DETAILTEXTURE
        
        // #if S_AMBIENT_OCCLUSION
        float flAmbientOcclusion = Tex2DS( g_tAmbientOcclusionTexture, TextureFiltering, vUV ).r;
        // #endif // S_AMBIENT_OCCLUSION
        
        float4 normalTexel = Tex2DS( g_tNormal, TextureFiltering, vUV );
        // inverted normals
		normalTexel.y = 1 - normalTexel.y;

        float specularFactor = 1.0f;
        #if S_NORMALMAPALPHAENVMAPMASK
        // if ( D_NORMALMAPALPHAENVMAPMASK )
		    specularFactor = normalTexel.a;
        #endif // S_NORMALMAPALPHAENVMAPMASK

        float3 albedo = baseColor.rgb;

        #if S_BLENDTINTBYBASEALPHA
            float3 tintedColor = albedo * g_vDiffuseModulation.rgb;
            tintedColor = lerp(tintedColor, g_vDiffuseModulation.rgb, g_flTintReplacementControl);
            albedo = lerp(albedo, tintedColor, baseColor.a);
        #else // !S_BLENDTINTBYBASEALPHA
            albedo = albedo * g_vDiffuseModulation.rgb;
        #endif // S_BLENDTINTBYBASEALPHA

        m.Albedo = albedo;

        float3 tangentSpaceNormal = normalTexel.xyz * 2.0f - 1.0f;
        // m.Normal = TransformNormal( i, DecodeHemiOctahedronNormal( normalTexel.xy ) );
        m.Normal = TransformNormal( i, tangentSpaceNormal );

        // #if S_AMBIENT_OCCLUSION
        m.AmbientOcclusion = float3(flAmbientOcclusion, flAmbientOcclusion, flAmbientOcclusion);
        // #endif // S_AMBIENT_OCCLUSION
        
        m.SpecularTint = float3( 1.0, 1.0, 1.0 );

        float alpha = g_vDiffuseModulation.a;
        #if ( !S_SELFILLUM && !S_BLENDTINTBYBASEALPHA )
            alpha *= baseColor.a;
        #endif // ( !S_SELFILLUM && !S_BLENDTINTBYBASEALPHA )
        m.Opacity = alpha;
        m.SelfIllumMask = baseColor.aaa;
        m.EnvmapMask = float3(specularFactor, specularFactor, specularFactor);
        
        return FinalizeLegacyOutput(ShadingModelLegacy::Shade( i, m, config ));
	}
}