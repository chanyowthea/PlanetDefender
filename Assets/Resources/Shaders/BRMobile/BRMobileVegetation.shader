Shader "BRMobile/Vegetation" {

Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_Brightness("Brightness", Range(0, 5)) = 1
	_ShadowColor ("Shadow Color", Color) = (1,1,1,1)
	_ShadowBrightness("Shadow Brightness", Range(0, 5)) = 1
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_ShadowTex("Shadow Map", 2D) = "black" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
}
SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 300
	Cull Off
	Lighting Off

	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile GRASS_RECV_SHADOW_OFF GRASS_RECV_SHADOW_ON
			#pragma target 2.0
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float2 worldUV: TEXCOORD1;
				UNITY_FOG_COORDS(2)
			};

			#include "BRMobileCommon.cginc"

			sampler2D _MainTex;
			sampler2D _ShadowTex;
			float4 _MainTex_ST;
			fixed _Cutoff;
			fixed4 _Color;
			fixed _Brightness;
			fixed4 _ShadowColor;
			fixed  _ShadowBrightness;
			fixed4 _Bias;
			fixed4 _WindVec;

			v2f vert (appdata_t v)
			{
				v2f o;
				float4 vertex = v.vertex + float4(v.vertex.y * _Bias.xyz, 0.0f);
				vertex.xyz += v.vertex.y *  _WindVec.xyz;
				o.vertex = UnityObjectToClipPos(vertex);
				float3 worldPos = mul(unity_ObjectToWorld, vertex).xyz;
				
				o.worldUV.x = (worldPos.x - 470.0f) * 0.0005263f + 0.5f;   //  0.0005263f = (1 / 1900) 
				o.worldUV.y = (worldPos.z - 110.0f) * 0.0005263f + 0.5f;

				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2Dbias(_MainTex, float4(i.texcoord.xy, 0.0f, -1.0f)) *_Color;
#if GRASS_RECV_SHADOW_ON
				fixed shadow = tex2D(_ShadowTex, i.worldUV).r;
				shadow = 1.0 - clamp(shadow, 0.0f, 1.0f);
				col.rgb = lerp(col.rgb * _Brightness, col.rgb * _ShadowColor * _ShadowBrightness, shadow);
#else
				col.rgb = col.rgb * _Brightness;
#endif

				clip(col.a - _Cutoff);

				BRFOG(i.fogCoord, col);

				return col;
			}
		ENDCG
	}
}


SubShader{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 200
		Cull Off
		Lighting Off

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_fog

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		float3 worldPos: TEXCOORD1;
		UNITY_FOG_COORDS(2)
	};

#include "BRMobileCommon.cginc"

	sampler2D _MainTex;
	float4 _MainTex_ST;
	fixed _Cutoff;
	fixed4 _Color;
	fixed _Brightness;
	fixed4 _Bias;
	fixed4 _WindVec;

	v2f vert(appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		float4 vertex = v.vertex + float4(v.vertex.y * _Bias.xyz, 0.0f);
		vertex.xyz += v.vertex.y *  _WindVec.xyz;
		o.vertex = UnityObjectToClipPos(vertex);
		o.worldPos = mul(unity_ObjectToWorld, vertex).xyz;
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2Dbias(_MainTex, float4(i.texcoord.xy, 0.0f, -1.0f)) *_Color;
	col.rgb *= _Brightness;
	clip(col.a - _Cutoff);

	BRFOG(i.fogCoord, col);

	return col;
	}
		ENDCG
	}
}

FallBack "BRMobile/CutOffTwoSidesColor"

}
