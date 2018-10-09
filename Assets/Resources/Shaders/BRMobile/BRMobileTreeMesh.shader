// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "BRMobile/TreeMesh" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.8
		_Color ("Main Color", Color) = (1,1,1,1)
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }

		LOD 300
		Cull Off

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ DISABLE_LIGHTMAP
#pragma multi_compile_fog

#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"
#include "UnityLightingCommon.cginc"
#include "BRMobileCommon.cginc"

	sampler2D _MainTex;
	
	fixed4 _Color;
	fixed _Cutoff;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)        
		float2 uv1 : TEXCOORD1;
		float3 diffuseColor : TEXCOORD2;
		UNITY_FOG_COORDS(3)
#else
		float3 diffuseColor : TEXCOORD1;
		UNITY_FOG_COORDS(2)
#endif	
	};

	struct a2v {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float3 color: COLOR;
		float2 texcoord : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;
	};

	v2f vert(a2v i) {
		v2f o;
		half4 newPos = i.vertex;
		o.pos = UnityObjectToClipPos(newPos);
		o.uv0 = i.texcoord;
		
		half3 worldNormal = UnityObjectToWorldNormal(i.normal);
		o.diffuseColor = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz)) * _LightColor0 + UNITY_LIGHTMODEL_AMBIENT;
		o.diffuseColor = o.diffuseColor * i.color.b + (1.0f - i.color.b);

#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)              
		o.uv1 = i.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif	

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}


	half4 frag(v2f i) : COLOR{
		half4 color =  tex2D(_MainTex, i.uv0) * _Color;

#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)
		fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1));
		color.rgb *= lm * i.diffuseColor;
#else
		color.rgb *= i.diffuseColor * 1.2f;
#endif
		BRFOG(i.fogCoord, color);

		clip(color.a - _Cutoff);

		return color;
	}
		ENDCG
	}
	}
		////////////////////////////////////////////////////////////////////////////

		SubShader
	{
		Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
		LOD 200
		Cull Off

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile LIGHTMAP_ON

#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"

		sampler2D _MainTex;
		
		fixed4 _Color;
		fixed _Cutoff;

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv0 : TEXCOORD0;
			float3 diffuseColor : TEXCOORD1;
		};

		struct a2v {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float3 color: COLOR;
			float2 texcoord : TEXCOORD0;
			float2 texcoord1 : TEXCOORD1;
		};

	v2f vert(a2v i) {
		v2f o;
		half4 newPos = i.vertex;
		o.pos = UnityObjectToClipPos(newPos);
		o.uv0 = i.texcoord;

		return o;
	}


	half4 frag(v2f i) : COLOR{
		half4 color = tex2D(_MainTex, i.uv0) * _Color;
		color.rgb *= 1.2f;
		clip(color.a - _Cutoff);

		return color;
	}
		ENDCG
	}

	}
	Fallback "BRMobile/VertexLit"
}
