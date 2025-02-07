// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
// Mesh Animator shader source Copyright (c) 2020 JS Technologies, LLC

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Mesh Animator/Unlit/Texture" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 100

    Pass {
        CGPROGRAM
            #pragma fragment frag addshadow
            #pragma target 3.5
            #pragma multi_compile_fog
			#pragma multi_compile_instancing

            #include "UnityCG.cginc"
			
            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            CBUFFER_START(UnityPerMaterial) //解决SRP Batcher not compatible
            sampler2D _MainTex;
            float4 _MainTex_ST;
            CBUFFER_END
			
			// start MeshAnimator
			#include "MeshAnimator.cginc"
			#pragma vertex vert
			struct appdata_ma {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
				uint vertexId : SV_VertexID;
                UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert (appdata_ma v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(ApplyMeshAnimation(v.vertex, v.vertexId));
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_DEPTH(v); 
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}
			// end MeshAnimator

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord);
                UNITY_APPLY_FOG(i.fogCoord, col);
                UNITY_OPAQUE_ALPHA(col.a);
                return col;
            }
        ENDCG
    }
}

}
