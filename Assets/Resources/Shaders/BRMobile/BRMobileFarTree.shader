// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "BRMobile/FarTree" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_FadeTex("Fade", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		_Color("Color", Color) = (1,1,1,1)
	}
		SubShader
	{
		//Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }

		LOD 300
		Cull Off
		//Blend SrcAlpha OneMinusSrcAlpha

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ DISABLE_LIGHTMAP
#pragma multi_compile_fog

#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"
#include "BRMobileCommon.cginc"	

		sampler2D _MainTex;
		sampler2D _FadeTex;
		fixed _Cutoff;
		float4 PosBias;
		fixed4 _Color;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float4 fadeUV : TEXCOORD1;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)        
		float2 uv1 : TEXCOORD1;
		UNITY_FOG_COORDS(3)
#else
		UNITY_FOG_COORDS(2)
#endif		
		
	};

	struct a2v {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float2 texcoord : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;
	};

	v2f vert(a2v i) {
		v2f o;
		float4 pos = i.vertex - PosBias;
		o.pos = UnityObjectToClipPos(pos);
		float2 tempUV = floor(i.texcoord);
		o.uv0 = i.texcoord - tempUV;
		o.fadeUV.xy = tempUV *  0.0625f;
		o.fadeUV.zw = o.uv0 * 64.0f;

#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)              
		o.uv1 = i.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif	

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}


	half4 frag(v2f i) : COLOR{
		half4 color = tex2D(_MainTex, i.uv0);

#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)
		fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1));
		color.rgb *= lm;
#else
		color.rgb *= 1.2f * _Color;
#endif
		BRFOG(i.fogCoord, color);

		half4 fadeValue = tex2D(_FadeTex, i.fadeUV.zw);
		float alpha = color.a - max(fadeValue.r * i.fadeUV.x, _Cutoff);
		clip(alpha);

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
	fixed _Cutoff;
	float4 PosBias;
	fixed4 _Color;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
	};

	struct a2v {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float2 texcoord : TEXCOORD0;
	};
	
	v2f vert(a2v i) {
		v2f o;
		float4 pos = i.vertex - PosBias;
		o.pos = UnityObjectToClipPos(pos);
		o.uv0 = i.texcoord;

		return o;
	}


	half4 frag(v2f i) : COLOR{
		half4 color = tex2D(_MainTex, i.uv0) * 1.2f * _Color;
		clip(color.a - _Cutoff);
		return color;
	}
		ENDCG
	}

	}
	Fallback "BRMobile/VertexLit"
}
