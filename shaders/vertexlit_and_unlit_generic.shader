//=========================================================================================================================
// Optional
//=========================================================================================================================
HEADER
{
	// CompileTargets = ( IS_SM_50 && ( PC || VULKAN ) );
	Description = "SourceBox vertex lit and unlit generic shader. Use when the material has no bumpmap or lightwarp. See license.txt for license information";
}

//=========================================================================================================================
// Optional
//=========================================================================================================================
FEATURES
{
    #include "sourcebox/common/legacy_features.hlsl"

    Feature( F_SEPARATE_DETAIL_UVS,			0..1, "Rendering" );

    Feature( F_DETAILTEXTURE,				0..1, "Rendering" );
    Feature( F_CUBEMAP,					    0..1, "Rendering" );
    Feature( F_CUSTOM_CUBEMAP,              0..1, "Rendering" );
    FeatureRule( Requires1( F_CUSTOM_CUBEMAP, F_CUBEMAP ), "" );

    Feature( F_DIFFUSELIGHTING,			    0..1, "Rendering" );
    Feature( F_ENVMAPMASK,					0..1, "Rendering" );
    Feature( F_BASEALPHAENVMAPMASK,		    0..1, "Rendering" );
    Feature( F_SELFILLUM,					0..1, "Rendering" );
    Feature( F_VERTEXCOLOR,				    0..1, "Rendering" );

    Feature( F_SELFILLUM_ENVMAPMASK_ALPHA,  0..1, "Rendering" );
    Feature( F_DETAIL_BLEND_MODE,           0..9(0="RGB_EQUALS_BASE_x_DETAILx2",1="RGB_ADDITIVE",2="DETAIL_OVER_BASE",3="FADE",4="BASE_OVER_DETAIL",5="RGB_ADDITIVE_SELFILLUM",6="RGB_ADDITIVE_SELFILLUM_THRESHOLD_FADE",7="MOD2X_SELECT_TWO_PATTERNS",8="MULTIPLY",9="MASK_BASE_BY_DETAIL_ALPHA"), "Rendering" );
    Feature( F_SEAMLESS_BASE,               0..1, "Rendering" );
    Feature( F_SEAMLESS_DETAIL,             0..1, "Rendering" );
    Feature( F_DISTANCEALPHA,               0..1, "Rendering" );
    Feature( F_DISTANCEALPHAFROMDETAIL,     0..1, "Rendering" );
    Feature( F_SOFT_MASK,                   0..1, "Rendering" );
    Feature( F_OUTLINE,                     0..1, "Rendering" );
    Feature( F_OUTER_GLOW,                  0..1, "Rendering" );

    Feature( F_BLENDTINTBYBASEALPHA,        0..1, "Rendering" );

    FeatureRule( Allow1( F_SEPARATE_DETAIL_UVS, F_SEAMLESS_DETAIL ), "" );

    FeatureRule( Requires1( F_DETAIL_BLEND_MODE, F_DETAILTEXTURE ), "Requires detail texture" );
    FeatureRule( Requires1( F_SEAMLESS_DETAIL, F_DETAILTEXTURE ), "");

    FeatureRule( Allow1( F_ENVMAPMASK, F_SEAMLESS_BASE ), "" );
    FeatureRule( Allow1( F_ENVMAPMASK, F_SEAMLESS_DETAIL ), "" );
    FeatureRule( Allow1( F_SELFILLUM_ENVMAPMASK_ALPHA, F_SEAMLESS_BASE ), "" );
    FeatureRule( Allow1( F_SELFILLUM_ENVMAPMASK_ALPHA, F_SEAMLESS_DETAIL ), "" );
    
    FeatureRule( Allow1( F_BASEALPHAENVMAPMASK, F_ENVMAPMASK ), "" );
    FeatureRule( Allow1( F_BASEALPHAENVMAPMASK, F_SELFILLUM ), "" );
    FeatureRule( Allow1( F_SELFILLUM, F_SELFILLUM_ENVMAPMASK_ALPHA ), "" );
    FeatureRule( Requires1( F_SELFILLUM_ENVMAPMASK_ALPHA, F_ENVMAPMASK ), "");

    FeatureRule( Allow1( F_BASEALPHAENVMAPMASK, F_SEAMLESS_BASE ), "" );
    FeatureRule( Allow1( F_BASEALPHAENVMAPMASK, F_SEAMLESS_DETAIL ), "" );

    FeatureRule( Requires1( F_DISTANCEALPHAFROMDETAIL, F_DISTANCEALPHA ), "");
    FeatureRule( Requires1( F_SOFT_MASK, F_DISTANCEALPHA ), "");
    FeatureRule( Requires1( F_OUTLINE, F_DISTANCEALPHA ), "");
    FeatureRule( Requires1( F_OUTER_GLOW, F_DISTANCEALPHA ), "");

// DISTANCEALPHA-related skips
    FeatureRule( Allow1( F_DISTANCEALPHA, F_ENVMAPMASK ), "" );
    FeatureRule( Allow1( F_DISTANCEALPHA, F_BASEALPHAENVMAPMASK ), "" );
    FeatureRule( Allow1( F_DISTANCEALPHA, F_SELFILLUM ), "" );
    FeatureRule( Allow1( F_DISTANCEALPHA, F_SELFILLUM_ENVMAPMASK_ALPHA ), "" );

    FeatureRule( Allow1( F_DISTANCEALPHA, F_SEAMLESS_BASE ), "" );
    FeatureRule( Allow1( F_DISTANCEALPHA, F_SEAMLESS_DETAIL ), "" );
    FeatureRule( Allow1( F_DISTANCEALPHA, F_CUBEMAP ), "" );

    FeatureRule( Allow1( F_SEAMLESS_BASE, F_BLENDTINTBYBASEALPHA ), "" );

    // BlendTintByBaseAlpha is incompatible with other interpretations of alpha
    // SKIP: ($BLENDTINTBYBASEALPHA) && ($SELFILLUM || (($DISTANCEALPHA) && ($DISTANCEALPHAFROMDETAIL == 0)) || $BASEALPHAENVMAPMASK)
    FeatureRule( Allow1( F_BLENDTINTBYBASEALPHA, F_SELFILLUM ), "" );
    FeatureRule( Requires1( F_BLENDTINTBYBASEALPHA, F_DISTANCEALPHA == 0, F_DISTANCEALPHAFROMDETAIL ), "" );
    FeatureRule( Allow1( F_BLENDTINTBYBASEALPHA, F_BASEALPHAENVMAPMASK ), "" );


    // this is enforced in SDK code
	FeatureRule( Allow1( F_ALPHA_TEST, F_SELFILLUM ), "" );
	FeatureRule( Allow1( F_ALPHA_TEST, F_BASEALPHAENVMAPMASK ), "" );
}

//=========================================================================================================================
// Optional
//=========================================================================================================================
MODES
{
    VrForward();
    Depth( "depth_only.shader" );
    ToolsVis( S_MODE_TOOLS_VIS );
    // ToolsWireframe( "tools_wireframe.shader" );
	// ToolsShadingComplexity( "tools_shading_complexity.shader" );
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
    #define S_DETAIL_TEXTURE (S_DETAILTEXTURE || S_OUTER_GLOW)

    // Seamless disabled to speed up compile
    // StaticCombo( S_SEAMLESS_BASE                , F_SEAMLESS_BASE                , Sys( ALL ) );
    // StaticCombo( S_SEAMLESS_DETAIL              , F_SEAMLESS_DETAIL              , Sys( ALL ) );
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
    StaticCombo( S_CUBEMAP					    , F_CUBEMAP					     , Sys( ALL ) );
    StaticCombo( S_CUSTOM_CUBEMAP				, F_CUSTOM_CUBEMAP				 , Sys( ALL ) );
    StaticCombo( S_DIFFUSELIGHTING			    , F_DIFFUSELIGHTING			     , Sys( ALL ) );
    StaticCombo( S_ENVMAPMASK					, F_ENVMAPMASK					 , Sys( ALL ) );
    StaticCombo( S_BASEALPHAENVMAPMASK		    , F_BASEALPHAENVMAPMASK		     , Sys( ALL ) );
    StaticCombo( S_SELFILLUM					, F_SELFILLUM					 , Sys( ALL ) );
    // StaticCombo( S_VERTEXCOLOR				    , F_VERTEXCOLOR				     , Sys( ALL ) );
    #define S_VERTEXCOLOR 0
    StaticCombo( S_SELFILLUM_ENVMAPMASK_ALPHA   , F_SELFILLUM_ENVMAPMASK_ALPHA   , Sys( ALL ) );
    StaticCombo( S_DISTANCEALPHA                , F_DISTANCEALPHA                , Sys( ALL ) );
    StaticCombo( S_DISTANCEALPHAFROMDETAIL      , F_DISTANCEALPHAFROMDETAIL      , Sys( ALL ) );
    StaticCombo( S_SOFT_MASK                    , F_SOFT_MASK                    , Sys( ALL ) );
    StaticCombo( S_OUTLINE                      , F_OUTLINE                      , Sys( ALL ) );
    StaticCombo( S_OUTER_GLOW                   , F_OUTER_GLOW                   , Sys( ALL ) );
    StaticCombo( S_BLENDTINTBYBASEALPHA         , F_BLENDTINTBYBASEALPHA         , Sys( ALL ) );
    
    // StaticCombo( S_DETAIL_BLEND_MODE            , F_DETAIL_BLEND_MODE            , Sys( ALL ) );
    int g_nDetailBlendMode < Expression(F_DETAIL_BLEND_MODE); >;
    #define DETAIL_BLEND_MODE g_nDetailBlendMode

    float3 g_vDetailTint                        < UiType( Color ); UiGroup( "Attributes,11/1000" ); Default3(1.0f, 1.0f, 1.0f); >;
    float4 g_vOutlineParams                     < UiType( Color ); UiGroup( "Attributes,11/1001" ); Default4(1.0f, 1.0f, 1.0f, 1.0f); >;
    float4 g_vOutlineColor                      < UiType( Color ); UiGroup( "Attributes,11/1002" ); Default4(1.0f, 1.0f, 1.0f, 1.0f); >;
    float g_flSoftMaskMax                       < UiGroup( "Attributes,11/1003" ); Default(1.0f); >;
    float g_flSoftMaskMin                       < UiGroup( "Attributes,11/1004" ); Default(0.0f); >;

    float2 g_vGlowUVOffset                      < UiGroup( "Attributes,11/1004" ); Default2(0.0f, 0.0f); >;
    float g_flOuterGlowMinDValue                < UiGroup( "Attributes,11/1005" ); Default(0.0f); >;
    float g_flOuterGlowMaxDValue                < UiGroup( "Attributes,11/1005" ); Default(1.0f); >;
    float4 g_vGlowColor                         < UiType( Color ); UiGroup( "Attributes,11/1006" ); Default4(1.0f, 1.0f, 1.0f, 1.0f); >;
    float g_flVertexAlpha                       < UiGroup( "Attributes,11/1007" ); Default(0.0f); >;
    
    #define USE_MANUAL_CUBEMAP (S_CUBEMAP && S_CUSTOM_CUBEMAP)
    #if S_DISTANCEALPHA && (S_DISTANCEALPHAFROMDETAIL == 0)
        #define BASE_COLOR_ALPHA_NAME   DistAlphaMask
    #elif S_BASEALPHAENVMAPMASK
        #define BASE_COLOR_ALPHA_NAME   EnvmapMask
    #elif S_BLENDTINTBYBASEALPHA
        #define BASE_COLOR_ALPHA_NAME   TintMask
    #elif ( !S_BASEALPHAENVMAPMASK && !S_SELFILLUM && !S_BLENDTINTBYBASEALPHA )
        #define BASE_COLOR_ALPHA_NAME   Translucency
    #elif S_SELFILLUM
        #define BASE_COLOR_ALPHA_NAME   SelfIllumMaskTexture
    #else
    #endif

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

    // This stuff needs to happen BEFORE the envmap color is added, so we hook into the custom lighting model.
    #define CUSTOM_DIFFUSE_MODULATE
    #include "sourcebox/common/texcombine.hlsl"
    float3 CustomDiffuseModulate( float3 diffuse, float3 albedo, float3 selfillumMask, float4 detailColor )
    {
        #if S_DETAILTEXTURE
            diffuse.xyz = 
                TextureCombinePostLighting( diffuse.xyz, detailColor, DETAIL_BLEND_MODE, g_flDetailBlendFactor );
        #endif

        #if S_SELFILLUM_ENVMAPMASK_ALPHA
            float3 selfIllumComponent = g_vSelfIllumTint * albedo;
            float Adj_Alpha = selfillumMask.r;
            diffuse.xyz=( max( 0, 1-Adj_Alpha ) * diffuse.xyz) + Adj_Alpha * selfIllumComponent;
        #else
            #if ( S_SELFILLUM )
                diffuse.xyz = lerp( diffuse.xyz, g_vSelfIllumTint * albedo, selfillumMask );
            #endif // S_SELFILLUM
        #endif

        return diffuse;
    }

    #include "sourcebox/common/legacy_pixel.hlsl"
    

	float4 MainPs( PixelInput i ) : SV_Target0
	{
        bool bDetailTexture = S_DETAILTEXTURE ? true : false;
        bool bCubemap = S_CUSTOM_CUBEMAP ? true : false;
        bool bDiffuseLighting = S_DIFFUSELIGHTING ? true : false;
        bool bHasNormal = bCubemap || bDiffuseLighting;
        bool bEnvmapMask = S_ENVMAPMASK ? true : false;
        bool bBaseAlphaEnvmapMask = S_BASEALPHAENVMAPMASK ? true : false;
        bool bSelfIllum = S_SELFILLUM ? true : false;
        bool bVertexColor = S_VERTEXCOLOR ? true : false;
        // bool bFlashlight = S_FLASHLIGHT ? true : false;
        bool bBlendTintByBaseAlpha = S_BLENDTINTBYBASEALPHA ? true : false;
        
        
        float4 baseColor = float4( 1.0f, 1.0f, 1.0f, 1.0f );
        #if S_SEAMLESS_BASE
            baseColor =
                i.vSeamlessWeights.x * CONVERT_COLOR(Tex2DS( g_tColor, TextureFiltering, i.vTextureCoords.yz ))+
                i.vSeamlessWeights.y * CONVERT_COLOR(Tex2DS( g_tColor, TextureFiltering, i.vTextureCoords.zx ))+
                i.vSeamlessWeights.z * CONVERT_COLOR(Tex2DS( g_tColor, TextureFiltering, i.vTextureCoords.xy ));
        #else // !S_SEAMLESS_BASE
            baseColor = CONVERT_COLOR(Tex2DS( g_tColor, TextureFiltering, i.vTextureCoords.xy ));

        // #if S_SRGB_INPUT_ADAPTER
        //     baseColor.rgb = GammaToLinear( baseColor.rgb );
        // #endif

        #endif // !S_SEAMLESS_BASE

        // #if S_AMBIENT_OCCLUSION
        float flAmbientOcclusion = Tex2DS( g_tAmbientOcclusionTexture, TextureFiltering, i.vTextureCoords.xy ).r;
        // #endif // S_AMBIENT_OCCLUSION

        #if S_DISTANCEALPHA && (S_DISTANCEALPHAFROMDETAIL == 0)
            float distAlphaMask = baseColor.a;
        #endif

        #if S_DETAILTEXTURE
            #if S_SEAMLESS_DETAIL
                float4 detailColor = 
                        i.vSeamlessWeights.x * CONVERT_DETAIL(Tex2DS( g_tDetailTexture, TextureFiltering, i.vDetailTextureCoords.yz ))+
                        i.vSeamlessWeights.y * CONVERT_DETAIL(Tex2DS( g_tDetailTexture, TextureFiltering, i.vDetailTextureCoords.zx ))+
                        i.vSeamlessWeights.z * CONVERT_DETAIL(Tex2DS( g_tDetailTexture, TextureFiltering, i.vDetailTextureCoords.xy ));
            #else
                float4 detailColor = CONVERT_DETAIL(Tex2DS( g_tDetailTexture, TextureFiltering, i.vDetailTextureCoords.xy ));
            #endif
            detailColor.rgb *= g_vDetailTint;

            #if S_DISTANCEALPHA && (S_DISTANCEALPHAFROMDETAIL == 1)
                float distAlphaMask = detailColor.a;
                detailColor.a = 1.0;									// make tcombine treat as 1.0
            #endif
            baseColor = 
                TextureCombine( baseColor, detailColor, DETAIL_BLEND_MODE, g_flDetailBlendFactor );
        #endif

        // #if S_DISTANCEALPHA
        #if S_DISTANCEALPHA && (S_DISTANCEALPHAFROMDETAIL == 0 || (S_DETAILTEXTURE && S_DISTANCEALPHAFROMDETAIL == 1)) 
            // now, do all distance alpha effects
            if ( S_OUTLINE )
            {
                float4 oFactors = smoothstep(g_vOutlineParams.xyzw, g_vOutlineParams.wzyx, distAlphaMask );
                baseColor = lerp( baseColor, g_vOutlineColor, oFactors.x * oFactors.y );
            }

            float mskUsed;
            if ( S_SOFT_MASK )
            {
                mskUsed = smoothstep( g_flSoftMaskMin, g_flSoftMaskMax, distAlphaMask );
                baseColor.a *= mskUsed;
            }
            else
            {
                mskUsed = distAlphaMask >= 0.5;
                if ( S_DETAILTEXTURE )
                    baseColor.a *= mskUsed;
                else
                    baseColor.a = mskUsed;
            }
            

            #if ( S_OUTER_GLOW )
                #if S_DISTANCEALPHAFROMDETAIL
                    float4 glowTexel = 	CONVERT_DETAIL(Tex2DS( g_tDetailTexture, TextureFiltering, i.vDetailTextureCoords.xy+g_vGlowUVOffset ));
                #else
                    float4 glowTexel = 	CONVERT_COLOR(Tex2DS( g_tColor, TextureFiltering, i.vDetailTextureCoords.xy+g_vGlowUVOffset ));
                #endif
                float4 glowc = g_vGlowColor*smoothstep( g_flOuterGlowMinDValue, g_flOuterGlowMaxDValue, glowTexel.a );
                baseColor = lerp( glowc, baseColor, mskUsed );
            #endif // S_OUTER_GLOW

        #endif  // DISTANCEALPHA

        float3 specularFactor = 1.0f;
        float4 envmapMaskTexel;

        #if S_ENVMAPMASK
            envmapMaskTexel = Tex2DS( g_tEnvMapMask, TextureFiltering, i.vTextureCoords.xy );
            specularFactor *= envmapMaskTexel.xyz;	
        #endif // S_ENVMAPMASK

        if( bBaseAlphaEnvmapMask )
        {
            specularFactor *= 1.0 - baseColor.a; // this blows!
        }

        // float3 diffuseLighting = float3( 1.0f, 1.0f, 1.0f );
        // if( bDiffuseLighting || bVertexColor && !( bVertexColor && bDiffuseLighting ) )
        // {
        //     diffuseLighting = i.vVertexColor.rgb;
        // }

        float3 albedo = baseColor.rgb;
        if (bBlendTintByBaseAlpha)
        {
            float3 tintedColor = albedo * g_vDiffuseModulation.rgb;
            tintedColor = lerp(tintedColor, g_vDiffuseModulation.rgb, g_flTintReplacementControl);
            albedo = lerp(albedo, tintedColor, baseColor.a);
        }
        else
        {
            albedo = albedo * g_vDiffuseModulation.rgb;
        }

        float alpha = g_vDiffuseModulation.a;
        if ( !bBaseAlphaEnvmapMask && !bSelfIllum && !bBlendTintByBaseAlpha )
        {
            alpha *= baseColor.a;
        }

        // if( bFlashlight ) ...

        if( bVertexColor && bDiffuseLighting )
        {
            albedo *= i.vVertexColor.rgb;
        }

	    alpha = lerp( alpha, alpha * i.vVertexColor.a, g_flVertexAlpha );

        // setup selfillum controls
        float3 vSelfIllumMask;
        #if S_SELFILLUM_ENVMAPMASK_ALPHA
            // range of alpha:
            // 0 - 0.125 = lerp(diffuse,selfillum,alpha*8)
            // 0.125-1.0 = selfillum*(1+alpha-0.125)*8 (over bright glows)
            float Adj_Alpha=8*envmapMaskTexel.a;
            vSelfIllumMask = float3(Adj_Alpha, 0, 0);
        #else
            if ( bSelfIllum )
            {
                vSelfIllumMask = Tex2DS( g_tSelfIllumMaskTexture, TextureFiltering, i.vTextureCoords.xy ).rgb;
                vSelfIllumMask = g_bSelfIllumMaskControl ? vSelfIllumMask : baseColor.aaa;
            }
        #endif

        ShadingLegacyConfig config = ShadingLegacyConfig::GetDefault();
        config.DoDiffuse = S_DIFFUSELIGHTING ? true : false;
        config.HalfLambert = false;
        config.DoAmbientOcclusion = true;
        config.DoLightingWarp = false;
        config.DoRimLighting = false;
        config.DoSpecularWarp = false;
        config.DoSpecular = false;

        // we're doing selfillum ourselves, because of the envmap mask option
        config.SelfIllum = false;
        config.SelfIllumFresnel = false;

        config.StaticLight = false;
        config.AmbientLight = true;

        float2 vUV = i.vTextureCoords.xy;
        Material m = GetDefaultLegacyMaterial();
        m.Albedo = albedo;

        float3 tangentSpaceNormal = float3( 0.0, 0.0, 1.0 );
        // m.Normal = TransformNormal( i, DecodeHemiOctahedronNormal( normalTexel.xy ) );
        m.Normal = TransformNormal( i, tangentSpaceNormal );

        // #if S_AMBIENT_OCCLUSION
        m.AmbientOcclusion = float3(flAmbientOcclusion, flAmbientOcclusion, flAmbientOcclusion);
        // #endif // S_AMBIENT_OCCLUSION
        
        m.SpecularTint = float3( 1.0, 1.0, 1.0 );
        m.Opacity = alpha;
        m.SelfIllumMask = vSelfIllumMask;
        m.EnvmapMask = specularFactor;
        #if S_DETAILTEXTURE
            m.DiffuseModControls = detailColor;
        #endif

        float4 output = ShadingModelLegacy::Shade( i, m, config );
        output.a = alpha;

        // shadingmodel hack, from above - replace diffuse lighting with albedo for UnlitGeneric.
        // hopefully this optimizes out the shadingmodel entirely
        if( !bDiffuseLighting && !bVertexColor || ( bVertexColor && bDiffuseLighting ) )
        {
            output.rgb = albedo;
        }

        return FinalizeLegacyOutput(output);
	}
}