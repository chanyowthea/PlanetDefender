// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "BRMobile/Tree" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_FadeTex("Fade", 2D) = "white" {}
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
#pragma multi_compile RUNTIMEVEG_OFF RUNTIMEVEG_ON
#pragma multi_compile_fog

#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"
#include "UnityLightingCommon.cginc"
#include "BRMobileCommon.cginc"

	sampler2D _MainTex;
	sampler2D _FadeTex;
	float4 moveVec;

	float4x4 posMatrix0;
	float4x4 posMatrix1;

#if RUNTIMEVEG_OFF
	static float4 rotateMatrix[4] = {
		float4(1.0f, 0.0f, 0.0f, 1.0f),
		float4(0.5, 0.8660254, -0.8660254, 0.5),
		float4(-0.5, 0.8660254, -0.8660254, -0.5),
		float4(-1.0, 0.0, 0.0, -1.0)
	};
#else
	float4x4 rotateMatrix;
#endif

	static float scaleVec = 0.9f;

	fixed4 _Color;
	fixed _Cutoff;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float3 diffuseColor : TEXCOORD1;
		float4 fadeFactor : TEXCOORD2;
		UNITY_FOG_COORDS(3)
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
		int id = floor(i.texcoord1.x + 0.0);
		half4 tmpPos = posMatrix0[0];
		if (id == 1) tmpPos = posMatrix0[1];
		else if (id == 2) tmpPos = posMatrix0[2];
		else if (id == 3) tmpPos = posMatrix0[3];
		else if (id == 4) tmpPos = posMatrix1[0];
		else if (id == 5) tmpPos = posMatrix1[1];
		else if (id == 6) tmpPos = posMatrix1[2];
		else if (id == 7) tmpPos = posMatrix1[3];

#if RUNTIMEVEG_OFF
		tmpPos = 0.0f;
#endif

		int transID =  floor(tmpPos.w);
		o.fadeFactor.xy = (tmpPos.w - transID) * 2.0f;
		o.fadeFactor.zw = i.texcoord * 64.0f;

		half4 rotVec = rotateMatrix[0];
		if(transID == 1) rotVec = rotateMatrix[1];
		else if (transID == 2) rotVec = rotateMatrix[2];
		else if (transID == 3) rotVec = rotateMatrix[3];

		half4 newPos = i.vertex;
		newPos.xyz *= scaleVec;
		newPos.xzy = half3(dot(newPos.xz, rotVec.xy), dot(newPos.xz, rotVec.zw), newPos.y);
		newPos = newPos + half4(tmpPos.xyz, 0.0f);

		o.pos = UnityObjectToClipPos(newPos);
		o.uv0 = i.texcoord;
		
		half3 worldNormal = UnityObjectToWorldNormal(i.normal);
		worldNormal.xz = half2(dot(worldNormal.xz, rotVec.xy), dot(worldNormal.xz, rotVec.zw));
		worldNormal = normalize(worldNormal);
		o.diffuseColor = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz)) * _LightColor0 + UNITY_LIGHTMODEL_AMBIENT;
		o.diffuseColor = o.diffuseColor * i.color.b + (1.0f - i.color.b);

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}


	half4 frag(v2f i) : COLOR{
		half4 color =  tex2D(_MainTex, i.uv0) * _Color;
		color.rgb *= i.diffuseColor * 1.2f ;

		BRFOG(i.fogCoord, color);

		half4 fadeValue = tex2D(_FadeTex, i.fadeFactor.zw);
		float alpha = color.a - max(fadeValue.r * i.fadeFactor.x, _Cutoff);
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
		float4 moveVec;
		
		float4x4 posMatrix0;
		float4x4 posMatrix1;
		float4x4 rotateMatrix;

		static float scaleVec = 0.9f;
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
		int id = floor(i.texcoord1.x + 0.0);
		half4 tmpPos = posMatrix0[0];
		if (id == 1) tmpPos = posMatrix0[1];
		else if (id == 2) tmpPos = posMatrix0[2];
		else if (id == 3) tmpPos = posMatrix0[3];
		else if (id == 4) tmpPos = posMatrix1[0];
		else if (id == 5) tmpPos = posMatrix1[1];
		else if (id == 6) tmpPos = posMatrix1[2];
		else if (id == 7) tmpPos = posMatrix1[3];

		int transID = floor(tmpPos.w);
		half4 rotVec = rotateMatrix[0];
		if (transID == 1) rotVec = rotateMatrix[1];
		else if (transID == 2) rotVec = rotateMatrix[2];
		else if (transID == 3) rotVec = rotateMatrix[3];

		half4 newPos = i.vertex;
		newPos.xyz *= scaleVec;
		newPos.xzy = half3(dot(newPos.xz, rotVec.xy), dot(newPos.xz, rotVec.zw), newPos.y + dot(moveVec.xy, i.color.rg));
		newPos = newPos + half4(tmpPos.xyz, 0.0f);

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
