// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "BRMobile/ShallowWater" {
	Properties{
		_NormalTex("Normal", 2D) = "white" {}
		_Tile0X("Tile0 X", Float) = 1.0
		_Tile0Y("Tile0 Y", Float) = 1.0
		_Tile1X("Tile1 X", Float) = 1.0
		_Tile1Y("Tile1 Y", Float) = 1.0
		_SpeedX("Speed X", Float) = 1.0
		_SpeedY("Speed Y", Float) = 1.0
		_CubeTex("Reflection", Cube) = "_Skybox" { TexGen CubeReflect }
		_Color("Color", Color) = (1,1,1,1)
		_LightDir("LightDir", Vector) = (0.5, 1.0, -0.5)
	}
	
		
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 300
		 Blend SrcAlpha OneMinusSrcAlpha

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog

#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"
#include "Lighting.cginc"
#include "BRMobileCommon.cginc"

		sampler2D _MaskTex;
		sampler2D _NormalTex;
		samplerCUBE _CubeTex; 
		fixed4 _Color;
		fixed4 _AmbientColor;
		fixed4 _LightDir;
		fixed _Tile0X;
		fixed _Tile0Y;
		fixed _Tile1X;
		fixed _Tile1Y;
		fixed _SpeedX;
		fixed _SpeedY;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
		float3 color : TEXCOORD2;
		float3 worldPos : TEXCOORD3;
		float3 lightDir : TEXCOORD4;
		UNITY_FOG_COORDS(5)
	};

	struct a2v {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float3 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	v2f vert(a2v i) {
		v2f o;
		o.pos = UnityObjectToClipPos(i.vertex);

		fixed2 moveUV = fixed2(_SpeedX, _SpeedY) *_Time;
		o.uv0 = i.texcoord * float2(_Tile0X, _Tile0Y) + moveUV;
		o.uv1 = i.texcoord * float2(_Tile1X, _Tile1Y) + moveUV;
		o.color = i.color;

		float4 posWorld = mul(unity_ObjectToWorld, i.vertex);
		o.worldPos = posWorld.xyz;

		o.lightDir = normalize(_LightDir);

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}


	half4 frag(v2f i) : COLOR{
		fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
		
		fixed3 normal1 = UnpackNormal(tex2D(_NormalTex, i.uv0)).xzy;
		fixed3 normal2 = UnpackNormal(tex2D(_NormalTex, i.uv1)).xzy;

		fixed3 normal =  normalize(normal1 + normal2);
		
		fixed3 refl = texCUBE(_CubeTex, reflect(-viewDir, normal)).rgb;
		fixed3 color = refl * _Color;

		float4 finalCol = float4(color, i.color.r);

		BRFOG(i.fogCoord, finalCol.rgb);

		return finalCol;
	}
		ENDCG
	}
	}
/////////////////////////////////////////////////////////////
SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog

#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"
#include "Lighting.cginc"

	sampler2D _MaskTex;
	sampler2D _NormalTex;
	samplerCUBE _CubeTex;
	fixed4 _Color;
	fixed4 _AmbientColor;
	fixed4 _LightDir;
	fixed _Tile0X;
	fixed _Tile0Y;
	fixed _Tile1X;
	fixed _Tile1Y;
	fixed _SpeedX;
	fixed _SpeedY;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
		float3 color : TEXCOORD2;
		float3 worldPos : TEXCOORD3;
		float3 lightDir : TEXCOORD4;
		UNITY_FOG_COORDS(5)
	};

	struct a2v {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float3 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};

		v2f vert(a2v i) {
			v2f o;
			o.pos = UnityObjectToClipPos(i.vertex);

			fixed2 moveUV = fixed2(_SpeedX, _SpeedY) *_Time;
			o.uv0 = i.texcoord * float2(_Tile0X, _Tile0Y) + moveUV;
			o.uv1 = i.texcoord * float2(_Tile1X, _Tile1Y) + moveUV;
			o.color = i.color;

			float4 posWorld = mul(unity_ObjectToWorld, i.vertex);
			o.worldPos = posWorld.xyz;

			o.lightDir = normalize(_LightDir);

			UNITY_TRANSFER_FOG(o, o.pos);
			return o;
		}


		half4 frag(v2f i) : COLOR{
			fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);

			fixed3 normal1 = UnpackNormal(tex2D(_NormalTex, i.uv0)).xzy;
			fixed3 normal2 = UnpackNormal(tex2D(_NormalTex, i.uv1)).xzy;

			fixed3 normal = normalize(normal1 + normal2);

			fixed3 halfDir = (viewDir + i.lightDir) * 0.5f;
			fixed specStrength = (dot(normal, normalize(halfDir)));

			float3 color = dot(i.lightDir, normal) * _Color + pow(specStrength, 30.0);
	
			float4 finalCol = float4(color, i.color.r);

			UNITY_APPLY_FOG(i.fogCoord, finalCol);

			return finalCol;
		}
		ENDCG
	}
	}



	Fallback "BRMobile/VertexLit"
}
