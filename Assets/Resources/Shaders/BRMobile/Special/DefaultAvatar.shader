// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "BRMobile/Special/DefaultAvatar" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Ambientlight("Ambient light", Range(0, 8)) = 0
	}
	
SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		Pass{
		Name "FORWARD"
		Tags{
		"LightMode" = "ForwardBase"
		}

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag


		#pragma multi_compile_fwdbase

		#pragma target 2.0
		#define UNITY_PASS_FORWARDBASE

		#include "UnityCG.cginc"
		#include "AutoLight.cginc"
		#include "Lighting.cginc"
		#include "AvatarCommon.cginc"

		uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
		uniform float _Ambientlight;

		struct VertexInput {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 texcoord0 : TEXCOORD0;
		};

		struct VertexOutput {
			float4 pos : SV_POSITION;
			float2 uv0 : TEXCOORD0;
			fixed3 diffuseColor : COLOR;
		};

		VertexOutput vert(VertexInput v) {
			VertexOutput o = (VertexOutput)0;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv0 = v.texcoord0;

			half3 worldNormal = UnityObjectToWorldNormal(v.normal);
			o.diffuseColor = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz)) * _LightColor0;

			return o;
		}

		float4 frag(VertexOutput i) : COLOR{

			float3 color = tex2D(_MainTex, i.uv0).rgb;
			float3 diffuse = (i.diffuseColor + _Ambientlight * INGAME_AMBIENT_3P) * color;

			/// Final Color:
			float4 finalRGBA = float4(diffuse,0);

			return finalRGBA;
		}
			ENDCG
		}
	}
	Fallback "BRMobile/VertexLit"
}
