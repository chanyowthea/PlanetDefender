// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "BRMobile/Special/PickupDiffuse" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Mask("Mask (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
		LOD 300

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
#include "../BRMobileCommon.cginc"

	sampler2D _MainTex;
	sampler2D _Mask;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
        float4 posWorld : TEXCOORD1;
		fixed3 diffuseColor : COLOR;
		UNITY_FOG_COORDS(2)
	};

	struct a2v {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float3 normal : NORMAL;
	};

	v2f vert(a2v i) {
		v2f o;
		o.pos = UnityObjectToClipPos(i.vertex);
		o.posWorld = mul(unity_ObjectToWorld, i.vertex);
		o.uv0 = i.texcoord;

		half3 worldNormal = UnityObjectToWorldNormal(i.normal);
		o.diffuseColor = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz)) * _LightColor0 + UNITY_LIGHTMODEL_AMBIENT;

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}


	half4 frag(v2f i) : COLOR{
		half4 color = tex2D(_MainTex, i.uv0);
		color.rgb *= i.diffuseColor;

		const float4x4 matTrans = {
                    {-0.77333,-0.0285,-0.63336,0.01409},
                    {0.44038,0.69453,-0.56895,-0.07847},
                    {-0.4561,0.7189,0.52455,-1.49408},
                    {0,0,0,1}
                };
        float2 panUV = ((mul(matTrans,float4(i.posWorld.rgb,i.posWorld.a)).rg*float2(1.2,-1.6))+_Time.g*float2(0,1.25)).rg;
        float2 uv = float2(panUV.r,saturate(panUV.g - 3.0 * floor(panUV.g/3.0)));
        float4 _Mask_var = tex2D(_Mask, uv);
        float3 emissive = (_Mask_var.rgb*float3(0.25,0.45,0.91)*1.1);

        color.rgb += emissive;
		BRFOG(i.fogCoord, color.rgb);

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

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ DISABLE_LIGHTMAP

#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"
#include "../BRMobileCommon.cginc"

		sampler2D _MainTex;
		sampler2D _Mask;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
        float4 posWorld : TEXCOORD1;
	};

	struct a2v {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
	};

	fixed4 _Color;

	v2f vert(a2v i) {
		v2f o;
		o.pos = UnityObjectToClipPos(i.vertex);
		o.uv0 = i.texcoord;
		o.posWorld = mul(unity_ObjectToWorld, i.vertex);
		return o;
	}


	half4 frag(v2f i) : COLOR{
		half4 color = tex2D(_MainTex, i.uv0);
		color.rgb *= ConstantBrightness;

		const float4x4 matTrans = {
                    {-0.77333,-0.0285,-0.63336,0.01409},
                    {0.44038,0.69453,-0.56895,-0.07847},
                    {-0.4561,0.7189,0.52455,-1.49408},
                    {0,0,0,1}
                };
        float2 panUV = ((mul(matTrans,float4(i.posWorld.rgb,i.posWorld.a)).rg*float2(1.2,-1.6))+_Time.g*float2(0,1.25)).rg;
        float2 uv = float2(panUV.r,saturate(panUV.g - 3.0 * floor(panUV.g/3.0)));
        float4 _Mask_var = tex2D(_Mask, uv);
        float3 emissive = (_Mask_var.rgb*float3(0.25,0.45,0.91)*1.1);

        color.rgb += emissive;

		return color;
	}
		ENDCG
	}

	}
	Fallback "BRMobile/VertexLit"
}
