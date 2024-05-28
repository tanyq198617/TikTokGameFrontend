// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#ifdef USE_DISSOLVE
	#define USE_UV2 
#endif

//#define USE_UV2  // UV2
//#define USE_EMISSION // 外发光
//#define USE_FULL_GLOW // 外发光
//#define USE_FULL_GLOW_MASK // 流光遮罩 让局部流光
//#define USE_EXTRA_FLOW // 额外流光
//#define USE_SKYBOX_REF // 反射加强
//#define USE_FRESNEL // 菲涅尔效果
//#define USE_ICE_EFFECT // 冰冻效果
//#define USE_DISSOLVE // 溶解
// #define USE_VERTEX // 顶点形变
// #define USE_OUT_LIGHT // 边缘光
 

/*#include "UnityPBSLighting.cginc"
#include "AutoLight.cginc"*/
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ImageBasedLighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
CBUFFER_START(UnityPerMaterial)
	float4 _Color;	
	float4 _MainTex_ST;
	float  _Smoothness;
	float _OcclusionStrength;
	float _AlphaCutoff;
	float _BumpScale;

	float _OutLightStrengh;
	float _OutLightPow;
	half4 _OutLightColor;
	float _EmissionFactor;

	float _CubeMapFactor;
  
    float4 _FullGlowTex_ST;
    half4 _FullGlowColor;
    float _FullGlowMoveU;
    float _FullGlowMoveV;
    float _FullGlowIntensity;
    float _FullGlowRange;
	float4 _FullGlowMaskTex_ST;


    float4 _IceEffectTex_ST;
    float _IceEffectAlpha;
    float4 _IceFlowTex_ST;
    float _IceTexScale;
    half4 _IceFlowColor;

	float4 _FresnelColorInner;
	float _FresnelColorInnerIntensity;
	float4 _FresnelColorOuter;
	float _FresnelColorOuterIntensity;
	float _FresnelPower;
	float _FresnelAdd;
	float _FresnelIntensity;


	float4 _ExtraFlowColor;

	float _ExtraFlowMainScale;
	float2 _ExtraFlowMainSpeed;
	float _ExtraFlowDistortTexScale;
	float2 _ExtraFlowDistortSpeed;
	float _ExtraFlowDistortIntensity;    

	// fresnel 
	float4 _ExtraFlowFresnelColor;
	float _ExtraFlowFresnelPower;
	float _ExtraFlowFresnelIntensity;

    float _DissolveThread;
    float _EdgeColorThread1;
    float _EdgeColorThread2;
    float4 _EdgeColor1;
    float4 _EdgeColor2;

    float _VertexTranslationRange;
    float _VertexTranslationThread;
    float _VertexTranslationLength;
    float3 _VertexTarget;

    float _OutlineSize;
    float4 _OutlineColor;
    float _OutlinePower;
    float _OutlineStrength;
CBUFFER_END

 
TEXTURE2D(_MainTex);            SAMPLER(sampler_MainTex);
TEXTURE2D(_MetalRoughnessOcMask);            SAMPLER(sampler_MetalRoughnessOcMask);

TEXTURE2D(_NormalMap);            SAMPLER(sampler_NormalMap);

#if USE_EMISSION
TEXTURE2D(_EmissionMap);            SAMPLER(sampler_EmissionMap);
#endif

#if USE_SKYBOX_REF
TEXTURECUBE(_CubeMap);                 SAMPLER(sampler_CubeMap);
#endif

#if USE_FULL_GLOW
TEXTURE2D(_FullGlowTex);            SAMPLER(sampler_FullGlowTex);
#if USE_FULL_GLOW_MASK
TEXTURE2D(_FullGlowMaskTex);            SAMPLER(sampler_FullGlowMaskTex);
#endif
#endif

#if USE_ICE_EFFECT
TEXTURE2D(_IceEffectTex);            SAMPLER(sampler_IceEffectTex);
TEXTURE2D(_IceFlowTex);            SAMPLER(sampler_IceFlowTex);
#endif

#ifdef USE_EXTRA_FLOW
// 主流光
TEXTURE2D(_ExtraFlowMainTex);            SAMPLER(sampler_ExtraFlowMainTex);
// mask
TEXTURE2D(_ExtraFlowMaskTex);            SAMPLER(sampler_ExtraFlowMaskTex);
#endif

// uv 扭曲
TEXTURE2D(_ExtraFlowDistortTex);            SAMPLER(sampler_ExtraFlowDistortTex);

#ifdef USE_DISSOLVE
TEXTURE2D(_DissolveTexture);            SAMPLER(sampler_DissolveTexture);
#endif

struct VertexData {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
	float2 uv : TEXCOORD0;

	UNITY_VERTEX_INPUT_INSTANCE_ID
#ifdef USE_UV2
    float2 uv2:TEXCOORD4;
#endif

#ifdef USE_EXTRA_FLOW
    float4 vertexColor : COLOR;
#endif
};

struct Interpolators {
	float4 pos : SV_POSITION;
	float4 uv : TEXCOORD0;
	float3 normal : TEXCOORD1;

	#if defined(BINORMAL_PER_FRAGMENT)
		float4 tangent : TEXCOORD2;
	#else
		float3 tangent : TEXCOORD2;
		float3 binormal : TEXCOORD3;
	#endif

	float3 worldPos : TEXCOORD4;
	UNITY_VERTEX_INPUT_INSTANCE_ID
#ifdef USE_EXTRA_FLOW
    float4 vertexColor : COLOR;
#endif

#ifdef USE_UV2
    float2 uv2: TEXCOORD5;
#endif
	half3 vertexSH : TEXCOORD6;
	half4 shadowCoord: TEXCOORD7;
	//float3 debugValue : TEXCOORD7;

};

float GetAlpha (Interpolators i) {
	float alpha = _Color.a;
	#if !defined(_SMOOTHNESS_ALBEDO)
		alpha *= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.xy).a;
	#endif
	return alpha;
}


float3 GetTangentSpaceNormal (Interpolators i) {
	float3 normal = float3(0, 0, 1);
	normal = UnpackNormalScale(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, i.uv.xy), _BumpScale);
	return normal;
}



float3 CreateBinormal (float3 normal, float3 tangent, float binormalSign) {
	return cross(normal, tangent.xyz) *
		(binormalSign * unity_WorldTransformParams.w);
}

Interpolators PBRVert (VertexData v) {
	Interpolators i;

	UNITY_SETUP_INSTANCE_ID(v);
	float3 rawWorldPos = mul(unity_ObjectToWorld, v.vertex);

#ifdef USE_VERTEX
    //目标点
    float3 targetOS = mul( unity_WorldToObject,  float4(_VertexTarget,1) ).xyz;
    float3 vertexDiff = (rawWorldPos-_VertexTarget)/_VertexTranslationRange;
    float vermask = 1.0 - pow( saturate( dot(vertexDiff,vertexDiff)), _VertexTranslationThread);
    v.vertex.xyz = lerp(v.vertex.xyz, targetOS, vermask);
    //i.debugValue = targetOS;
#endif

#ifdef USE_OUTLINE
    v.vertex.xyz += v.normal * _OutlineSize * 0.1;
#endif
	
	i.normal = TransformObjectToWorldNormal(v.normal);
	i.pos = TransformObjectToHClip(v.vertex);
	i.worldPos =  mul(unity_ObjectToWorld, v.vertex);
	OUTPUT_SH(i.normal, i.vertexSH);
#if defined(BINORMAL_PER_FRAGMENT)
	i.tangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);
#else
	i.tangent = TransformObjectToWorldDir(v.tangent.xyz);
	i.binormal = CreateBinormal(i.normal, i.tangent, v.tangent.w);
#endif


	i.shadowCoord = TransformWorldToShadowCoord(i.worldPos);

	i.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);

#ifdef USE_EXTRA_FLOW
	i.vertexColor = v.vertexColor;
#endif

#ifdef USE_UV2
    i.uv2 = v.uv2;
#endif

	return i;
}

Light CreateLight (Interpolators i, float4 shadowCoord) {
	Light light = GetMainLight(shadowCoord);

	/*#if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
		light.dir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
	#else
		light.dir = _WorldSpaceLightPos0.xyz;
	#endif

	light.color = _LightColor0.rgb ;
	light.ndotl = DotClamped(i.normal, light.dir);*/
	return light;
}


half3 CreateIndirectLight(BRDFData brdfData, half3 bakedGI, half occlusion,	half3 normalWS, half3 viewDirectionWS, half refMask)
{
	half3 reflectVector = reflect(-viewDirectionWS, normalWS);
	half NoV = saturate(dot(normalWS, viewDirectionWS));
	half fresnelTerm = Pow4(1.0 - NoV);

	half3 indirectDiffuse = bakedGI * occlusion;
	half3 indirectSpecular = GlossyEnvironmentReflection(reflectVector, brdfData.perceptualRoughness, occlusion);

	#ifdef USE_SKYBOX_REF 
	float3 cubeMapColor = SAMPLE_TEXTURECUBE_LOD(_CubeMap, sampler_CubeMap, reflectVector,  0) * _CubeMapFactor * saturate(refMask);
	indirectSpecular = lerp(indirectSpecular, cubeMapColor, refMask);    
	#endif
	
	half3 color = EnvironmentBRDF(brdfData, indirectDiffuse, indirectSpecular, fresnelTerm);
	
	return color;
}

void InitializeFragmentNormal(inout Interpolators i) {
	float3 tangentSpaceNormal = GetTangentSpaceNormal(i);
	#if defined(BINORMAL_PER_FRAGMENT)
		float3 binormal = CreateBinormal(i.normal, i.tangent.xyz, i.tangent.w);
	#else
		float3 binormal = i.binormal;
	#endif
	
	i.normal = normalize(
		tangentSpaceNormal.x * i.tangent +
		tangentSpaceNormal.y * binormal +
		tangentSpaceNormal.z * i.normal
	);
}
float3 CalcExtraFlowVal(in Interpolators i, float nv) {

#ifdef USE_EXTRA_FLOW
    float2 rawUV = i.uv;
    float2 finalUV = rawUV;

    // UV distort
	float distortUVFactor = SAMPLE_TEXTURE2D(_ExtraFlowDistortTex, sampler_ExtraFlowDistortTex, rawUV*_ExtraFlowDistortTexScale + _Time.g * _ExtraFlowDistortSpeed ).r *_ExtraFlowDistortIntensity ;
	finalUV = distortUVFactor * rawUV + (_Time.g *_ExtraFlowMainSpeed + rawUV * _ExtraFlowMainScale);

	float4 flowTexVal = SAMPLE_TEXTURE2D(_ExtraFlowMainTex, sampler_ExtraFlowMainTex, finalUV);

	// fresnel 
    float4 fresnelVal = _ExtraFlowFresnelColor * pow(1 - saturate(nv), _ExtraFlowFresnelPower)* _ExtraFlowFresnelIntensity;

    // mask
    float mask = SAMPLE_TEXTURE2D(_ExtraFlowMaskTex, sampler_ExtraFlowMaskTex, rawUV).r;
    float4 finalCol = fresnelVal  * i.vertexColor * flowTexVal *_ExtraFlowColor;
    float alpha =  mask * i.vertexColor.a  * finalCol.a;
    return (finalCol.rgb * alpha);
#else
    return 1;
#endif 
}

float4 PBRFrag (Interpolators i) : SV_TARGET {
	UNITY_SETUP_INSTANCE_ID(i);
	float alpha = GetAlpha(i);
	#if defined(_RENDERING_CUTOUT)
		clip(alpha - _AlphaCutoff);
	#endif

	//return float4(i.debugValue,1);
	InitializeFragmentNormal(i);

	float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
	float3 reflectionDir = reflect(-viewDir, i.normal);
    float nv = max(saturate(dot(i.normal, viewDir)), 0.000001);


	float4 metalRoughnessOcMask = SAMPLE_TEXTURE2D(_MetalRoughnessOcMask, sampler_MetalRoughnessOcMask, i.uv.xy);
	float metal = metalRoughnessOcMask.r;
	float smoothness = (1 - metalRoughnessOcMask.g)*_Smoothness ;
	float occlusion = 1-(1-metalRoughnessOcMask.b) * _OcclusionStrength;
	float rawMask = metalRoughnessOcMask.a;

	float3 rawAlbedo = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.xy).rgb * _Color.rgb;

    //溶解边缘
#ifdef USE_DISSOLVE
    half4 dissolveTexture = SAMPLE_TEXTURE2D(_DissolveTexture, sampler_DissolveTexture, i.uv2);
    clip((dissolveTexture.r - _DissolveThread));
    half edge = saturate(_DissolveThread / dissolveTexture.r)/*-_EdgeColorThread1*/;
    if (edge > 1-_EdgeColorThread1)
    {
        return _EdgeColor1;
    }
#endif


#if USE_FULL_GLOW
    float2 rawUV = i.uv;
    //全身流光
  #if USE_FULL_GLOW_SIN_TIME
    float tFg = _SinTime.w;
  #else
    float tFg = _Time.g;
  #endif
    float2 moveUV = (rawUV + tFg * float2(_FullGlowMoveU, _FullGlowMoveV));
    float4 fullGlowTexVal = SAMPLE_TEXTURE2D(_FullGlowTex, sampler_FullGlowTex, TRANSFORM_TEX(moveUV, _FullGlowTex));
    fullGlowTexVal = saturate(pow(fullGlowTexVal, _FullGlowRange)) ;

  #if USE_FULL_GLOW_MASK
    float4 fullGlowMaskVal = SAMPLE_TEXTURE2D(_FullGlowMaskTex, sampler_FullGlowMaskTex, TRANSFORM_TEX(i.uv, _FullGlowMaskTex));
    _FullGlowColor.a = _FullGlowColor.a * fullGlowMaskVal.r * fullGlowMaskVal.a;
  #endif
    rawAlbedo.rgb = saturate(rawAlbedo.rgb + fullGlowTexVal.rgb* _FullGlowColor.rgb  * fullGlowTexVal.a * _FullGlowColor.a * _FullGlowIntensity);
#endif


	// Unity PBR 
	float3 specularTint;
	float oneMinusReflectivity;

	specularTint = lerp(kDieletricSpec.rgb, rawAlbedo, metal);
	oneMinusReflectivity = OneMinusReflectivityMetallic(metal);
	float3 albedo =  rawAlbedo * oneMinusReflectivity;
	
#if defined(_RENDERING_TRANSPARENT)
	albedo *= alpha;
	alpha = 1 - oneMinusReflectivity + alpha * oneMinusReflectivity;
#endif

    float refMask = rawMask * metal;
    refMask = clamp(refMask,0, refMask);     
	
	
	BRDFData brdfData;
	InitializeBRDFData(albedo, metal, specularTint, smoothness, alpha, brdfData);
	Light mainLight = CreateLight(i, i.shadowCoord);
	half3 bakedGI = SampleSHPixel(i.vertexSH, i.normal);
	MixRealtimeAndBakedGI(mainLight, i.normal, bakedGI);

	half3 color = CreateIndirectLight(brdfData, bakedGI, occlusion,  i.normal, viewDir, refMask);
	color += LightingPhysicallyBased(brdfData,  mainLight, i.normal, viewDir);
	//float3 testColor = color;
	//边缘发光
#if USE_OUT_LIGHT
	half3 outLight = pow(1 - saturate(dot(viewDir, i.normal)), _OutLightPow) * _OutLightColor.rgb * _OutLightStrengh;
	color.rgb += outLight;
#endif

	float4 finalColor = float4(color, 1.0);


#ifdef USE_ICE_EFFECT
    float2 scale_uv = i.uv * _IceTexScale;
    float4 iceEffectCol = SAMPLE_TEXTURE2D(_IceEffectTex, sampler_IceEffectTex, TRANSFORM_TEX(scale_uv, _IceEffectTex));
    float4 _IceFlowCol = SAMPLE_TEXTURE2D(_IceFlowTex, sampler_IceFlowTex, TRANSFORM_TEX(i.uv + _Time.g * float2(0.1, 0), _IceFlowTex)) + _IceFlowColor;
    finalColor.rgb = finalColor.rgb * (1 - _IceEffectAlpha) + iceEffectCol.rgb * _IceEffectAlpha * _IceFlowCol.rgb;  //mainColor  todo
#endif

#ifdef USE_FRESNEL
   float4 fresnelVal = lerp((_FresnelColorInner * _FresnelColorInnerIntensity), 
            				(_FresnelColorOuter * _FresnelColorOuterIntensity), 
            				(pow(1- saturate(nv), _FresnelPower)+ _FresnelAdd)* _FresnelIntensity
            				);
	finalColor.rgb += fresnelVal.rgb;
#endif
        
#ifdef USE_EMISSION    
	float3 emission = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, i.uv.xy).rgb * _EmissionFactor;
	finalColor.rgb += emission;
#endif

#ifdef USE_EXTRA_FLOW
	float3 extraFlowVal = CalcExtraFlowVal(i,nv);
	finalColor.rgb += extraFlowVal;
#endif

#if defined(_RENDERING_FADE) || defined(_RENDERING_TRANSPARENT)
	finalColor.a = alpha;
#endif
	//finalColor.rgb = float3(mainLight.shadowAttenuation, 0.0, 0.0);
	return finalColor;
}


half4 OutlineFrag(Interpolators i) :COLOR
{
#ifdef USE_OUTLINE
    float3 viewdir = normalize(i.worldPos.xyz - _WorldSpaceCameraPos.xyz);
    float ndv = saturate(dot(normalize(i.normal), viewdir));
    float4 out_color = _OutlineColor;
    out_color.a = pow(ndv, _OutlinePower);
    out_color.a *= _OutlineStrength * ndv;
    return out_color;
#else
    return float4(0,0,0,0);
#endif
}

#endif