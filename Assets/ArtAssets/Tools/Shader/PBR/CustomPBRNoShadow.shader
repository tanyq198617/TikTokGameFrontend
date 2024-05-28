Shader "PBR/CustomPBRNoShadow" {

	Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

        _AddColor("AddColor",Color) = (0,0,0,0)
        _NormalMap("法线贴图",2D) = "bump"{}
        _BumpScale("法线贴图强度",Range(-2.0,2.0)) = 1.0

        [NoScaleOffset]
        _MetalRoughnessOcMask("金属粗糙度oc遮罩贴图", 2D) = "black" {}
        _Smoothness("Smoothness系数", Range(0.0, 1.0)) = 0.5
        _OcclusionStrength("oc系数",  Range(0,1)) = 0      
     
        /* 阴影*/
        _ShadowOffset("Shadow Offset" , Vector) = (0.1, 0.1, 0, 0) //阴影偏移
        _ShadowColor("Shadow Color", Color) = (0, 0, 0, 0.4) //阴影颜色
 

        /* 自发光*/
        [Toggle(USE_EMISSION)] _Emission("自发光",Float) = 0
        _EmissionMap("自发光 贴图", 2D) = "white" {}        
        _EmissionFactor("自发光颜色",Range(0,5)) = 1

        /* 反射加强*/
        [Header(SkyboxReflect)]
        [Toggle(USE_SKYBOX_REF)] _UseSkyBoxRef ("使用天空盒反射", Float) = 0   
        [NoScaleOffset] 
        _CubeMap("反射球贴图",cube) = "Skybox"{}
        _CubeMapFactor("反射系数",Range(0,5)) = 1   

        /* 全身流光*/
        [Header(Fresnel)]
        [Toggle(USE_FRESNEL)] _UseFresnel ("菲涅尔效果", Float) = 0
        _FresnelPower("菲涅尔范围Power", Range(0.01 , 10)) = 2
        _FresnelAdd("菲涅尔范围Add", Range(-5, 5)) = 0.18
        _FresnelIntensity("菲涅尔强度",Range(-10, 10)) = 1
        _FresnelColorOuter("菲涅尔边缘颜色", Color) = (1,1,1,1)
        _FresnelColorInner("菲涅尔内部颜色", Color) = (1,1,1,1)
        _FresnelColorOuterIntensity("菲涅尔边缘颜色强度",Range(-5, 5)) = 1.0
        _FresnelColorInnerIntensity("菲涅尔内部颜色强度",Range(-5, 5)) =1.0


        /* 全身流光*/
        [Header(FullFlow)]
        [Toggle(USE_FULL_GLOW)]  _UseFullGlowEffect("全身流光", Float) = 0
        _FullGlowColor("全身流光颜色", Color) = (0.53,0.64,1,1)
        [NoScaleOffset]
        _FullGlowTex("全身流光纹理", 2D) = "white" {}//流光纹理
        _FullGlowIntensity("全身流光强度",Float) = 1
        _FullGlowRange("全身流光范围",Float)=1
        _FullGlowMoveU("全身流光移动速度U", Float) = 0.15
        _FullGlowMoveV("全身流光移动速度V", Float) = -0.12
        [Toggle(USE_FULL_GLOW_SIN_TIME)] _UseFullGlowEffectSinTime("使用sintime,默认使用正常time", Float) = 0
        //流关遮罩
        [Toggle(USE_FULL_GLOW_MASK)] _UseFullGlowEffectMask("全身流光遮罩", Float) = 0
        _FullGlowMaskTex("全身流光遮罩纹理", 2D) = "white" {}


        /* 冰冻*/
        [Header(IceEffect)]
        [Toggle(USE_ICE_EFFECT)] _UseIceEffect("冰冻效果", Float) = 0
        [NoScaleOffset]
        _IceEffectTex ("冰冻特效纹理", 2D) = "white" {} //冰冻特效纹理
        _IceTexScale ("冰冻特效坐标纹理缩放", Float) = 2.5
        [NoScaleOffset]
        _IceFlowTex ("冰冻流动纹理", 2D) = "white" {}
        _IceFlowColor ("冰冻流动纹理颜色", Color) = (1, 1, 1)
        _IceEffectAlpha ("特效透明度", Range(0, 1)) = 1
     

        /* 额外的Flow*/
        [Header(ExtraFlow)]
        [Toggle(USE_EXTRA_FLOW)] _UseExtraFlow("开启额外流光", Float) = 0
        _ExtraFlowColor ("额外流光 颜色", Color) = (1, 1, 1)   
        [NoScaleOffset]
        _ExtraFlowMainTex ("额外流光 主帖图", 2D) = "white" {} 
        _ExtraFlowMainScale ("额外流光 贴图缩放", Range(0.01, 10)) = 1
        _ExtraFlowMainSpeed ("额外流光 流动速度", Vector) = (1, 1, 0, 0)
        // uv 扭曲   
        [NoScaleOffset]
        _ExtraFlowDistortTex ("额外流光 流动扭曲贴图", 2D) = "white" {} 
        _ExtraFlowDistortTexScale ("额外流光 流动扭曲贴图缩放", Range(0.01, 10)) = 1
        _ExtraFlowDistortSpeed ("额外流光 流动扭曲速度", Vector) = (1, 1, 0, 0)
        _ExtraFlowDistortIntensity ("额外流光 流动扭曲强度", Range(0, 1)) = 1
        // mask   
        [NoScaleOffset]
        _ExtraFlowMaskTex ("额外流光 遮罩贴图", 2D) = "white" {} 
        // fresnel 
        _ExtraFlowFresnelColor ("额外流光 Fresnel 颜色", Color) = (1, 1, 1)
        _ExtraFlowFresnelPower ("额外流光 Fresnel 衰减", Range(0.1, 10)) = 1
        _ExtraFlowFresnelIntensity ("额外流光 Fresnel 强度", Range(0, 4)) = 1


        /* 溶解*/
        [Header(Dissolve)]
        [Toggle(USE_DISSOLVE)] _UseDissolve("溶解效果", Float) = 0 
        [NoScaleOffset]
        _DissolveTexture("溶解贴图",2D)="white"{}
        _DissolveThread("溶解阈值",Range(0,1)) = 0
        _EdgeColorThread1("溶解边缘范围",Range(0,1)) = 0.4
        [HDR]_EdgeColor1("溶解边缘颜色",Color) = (1,1,1,1)
        
        /* 顶点偏移*/
        [Header(Vertex)]
        [Toggle(USE_VERTEX)] _UseVertex("顶点拉伸效果", Float) = 0
        _VertexTarget("目标点位置",vector)=(0,0,0,0)
        _VertexTranslationRange("拉伸范围", Float) = 12
        _VertexTranslationThread("拉伸平滑",Range(1,1000))=500

        [Header(Outline)]
        [Toggle(USE_OUTLINE)] _UseOutline("外发光效果", Float) = 0
        _OutlineSize("外发光范围",Range(0,5))=1.0
        [HDR] _OutlineColor("外发光颜色",Color)=(1,1,1,1)
        _OutlinePower("外发光渐变",Range(0.01,3))=1.0
        _OutlineStrength("外发光强度",Range(0.01,3))=1.00


		_AlphaCutoff ("Alpha Cutoff", Range(0, 1)) = 0.5

		[HideInInspector] _SrcBlend ("_SrcBlend", Float) = 5
		[HideInInspector] _DstBlend ("_DstBlend", Float) = 10
		[HideInInspector] _ZWrite ("_ZWrite", Float) = 1
	}

	HLSLINCLUDE

	#define BINORMAL_PER_FRAGMENT
	#define FOG_DISTANCE

	ENDHLSL

	//  美术需求： 
	// 1. 不需要多光源
	// 2. 阴影特殊实现
	// 3. 不需要烘培
	// 4. 不需要Lightmap
	// 5. 不需要兼容Prob等 反射
	SubShader {

        //UsePass "QianYu/3D/Shadow/SimpleShadow"
		Pass {
            Name "NormalImplement"
			Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True" "LightMode" = "UniversalForward"}
			LOD 100
			Blend [_SrcBlend] [_DstBlend]
			ZWrite [_ZWrite]

			HLSLPROGRAM

			#pragma target 3.0

			#pragma shader_feature _ _RENDERING_CUTOUT _RENDERING_FADE _RENDERING_TRANSPARENT
            #pragma shader_feature __ USE_EMISSION
            #pragma shader_feature __ USE_FULL_GLOW
            #pragma shader_feature __ USE_FULL_GLOW_MASK 
            #pragma shader_feature __ USE_UNDERE_WATER_EFFECT
            #pragma shader_feature __ USE_SKYBOX_REF
            #pragma shader_feature __ USE_FRESNEL    
            #pragma shader_feature __ USE_EXTRA_FLOW   


			//#pragma multi_compile _ VERTEXLIGHT_ON
            #pragma multi_compile __ USE_ICE_EFFECT 
            #pragma multi_compile __ USE_DISSOLVE 
            #pragma multi_compile __ USE_VERTEX 
            #pragma multi_compile_instancing

			#pragma vertex PBRVert
			#pragma fragment PBRFrag

			#include "CGInclude/CustomLighting.hlsl"
			ENDHLSL
		} 
		Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
	}
    FallBack "Universal Render Pipeline/Unlit"

	//CustomEditor "GamesTan.CustomLightingShaderGUI"
}