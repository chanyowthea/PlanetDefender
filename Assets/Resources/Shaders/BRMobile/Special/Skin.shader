// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BRMobile/Special/Skin" {

	Properties{
		_MainTex("Diffuse map", 2D) = "white" {}
		_Ambientlight("Ambient light", Range(0, 8)) = 0
		_SpecularColor("Specular Color", Color) = (1,1,1,1)
		_SpecularmapGlossA("Specular map(Gloss A)", 2D) = "white" {}
		_SpecularIntensity("Specular Intensity", Range(0, 8)) = 0.9
		_Gloss("Gloss", Range(0, 128)) = 64
		_Normalmap("Normal map", 2D) = "bump" {}
		_NormalIntensity("Normal Intensity", Range(0, 2)) = 0.79
		_Contrast("Contrast", Range(-1, 1)) = 1.0
	}

SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 600
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
	uniform sampler2D _SpecularmapGlossA; uniform float4 _SpecularmapGlossA_ST;
	uniform float4 _SpecularColor;
	uniform float _SpecularIntensity;
	uniform float _Gloss;
	uniform sampler2D _Normalmap; uniform float4 _Normalmap_ST;
	uniform float _NormalIntensity;
	uniform float _Ambientlight;
	uniform float _Contrast;
	uniform float _SkinExtraAmbient;

	struct VertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 tangent : TANGENT;
		float2 texcoord0 : TEXCOORD0;
	};

	struct VertexOutput {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float3 viewDir : TEXCOORD1;
		float3 normalDir : TEXCOORD2;
		float3 tangentDir : TEXCOORD3;
		float3 bitangentDir : TEXCOORD4;
		LIGHTING_COORDS(5,6)
	};

	VertexOutput vert(VertexInput v) {
		VertexOutput o = (VertexOutput)0;
		o.uv0 = v.texcoord0;
		o.normalDir = UnityObjectToWorldNormal(v.normal);
		o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
		o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
		float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
		o.viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);
		o.pos = UnityObjectToClipPos(v.vertex);
		TRANSFER_VERTEX_TO_FRAGMENT(o);
		return o;
	}

	float4 frag(VertexOutput i) : COLOR{
		float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
		float3 _Normalmap_var = UnpackNormal(tex2D(_Normalmap, i.uv0));
		float3 Normalmap = float3(0, 0, 1 - _NormalIntensity) + _Normalmap_var * _NormalIntensity;// lerp(float3(0, 0, 1), _Normalmap_var.rgb, _NormalIntensity);
		float3 normalDirection = normalize(mul(Normalmap, tangentTransform)); // Perturbed normals
																			  //float3 lightDirection = _WorldSpaceLightPos0.xyz;
		float3 halfDirection = normalize(i.viewDir + _WorldSpaceLightPos0.xyz);
		////// Lighting:
		float attenuation = LIGHT_ATTENUATION(i);
		float3 attenColor = attenuation * _LightColor0.xyz;
		///////// Gloss:
		////// Specular:
		float NdotL = max(0, dot(normalDirection, _WorldSpaceLightPos0.xyz));
		float NdotV = max(0, dot(normalDirection, i.viewDir));

		float forwardLight = (_Contrast + 1.0) * NdotL - _Contrast * NdotL * NdotL;
		float3 Diffusemap =  tex2D(_MainTex, i.uv0).rgb;
		float3 diffuse = (forwardLight * attenColor + _Ambientlight * LOBBY_AMBIENT_SKIN + _SkinExtraAmbient) * Diffusemap;

		float4 _SpecularmapGlossA_var = tex2D(_SpecularmapGlossA,i.uv0);
		float3 Specularmap = _SpecularColor.rgb * _SpecularmapGlossA_var.rgb * 2 * _SpecularIntensity * (1 - NdotV);
		float3 specular = pow(max(0, dot(halfDirection, normalDirection)), _Gloss) * Specularmap;

		/// Final Color:
		float3 finalColor = diffuse + specular;
		float4 finalRGBA = float4(saturate(finalColor),0);

		return finalRGBA;
	}
		ENDCG
	}


	Pass{
		Name "FORWARD_DELTA"
		Tags{
		"LightMode" = "ForwardAdd"
	}
		Blend One One

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#define UNITY_PASS_FORWARDADD
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma multi_compile_fwdadd
		#pragma target 2.0

		uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
		uniform sampler2D _SpecularmapGlossA; uniform float4 _SpecularmapGlossA_ST;
		uniform float4 _SpecularColor;
		uniform float _SpecularIntensity;
		uniform float _Gloss;
		uniform sampler2D _Normalmap; uniform float4 _Normalmap_ST;
		uniform float _NormalIntensity;

		struct VertexInput {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 tangent : TANGENT;
			float2 texcoord0 : TEXCOORD0;
		};
		struct VertexOutput {
			float4 pos : SV_POSITION;
			float2 uv0 : TEXCOORD0;
			float3 viewDir : TEXCOORD1;
			float3 normalDir : TEXCOORD2;
			float3 tangentDir : TEXCOORD3;
			float3 bitangentDir : TEXCOORD4;
		};

		VertexOutput vert(VertexInput v) {
			VertexOutput o = (VertexOutput)0;
			o.uv0 = v.texcoord0;
			o.normalDir = UnityObjectToWorldNormal(v.normal);
			o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
			o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
			float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
			o.viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}
		float4 frag(VertexOutput i) : COLOR{
			float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
			float3 _Normalmap_var = UnpackNormal(tex2D(_Normalmap, i.uv0));
			float3 Normalmap = float3(0, 0, 1 - _NormalIntensity) + _Normalmap_var * _NormalIntensity;// lerp(float3(0, 0, 1), _Normalmap_var.rgb, _NormalIntensity);
			float3 normalDirection = normalize(mul(Normalmap, tangentTransform));

			float3 halfDirection = normalize(i.viewDir + _WorldSpaceLightPos0.xyz);

			float4 _MainTex_var = tex2D(_MainTex,i.uv0);
			float3 diffuse = _LightColor0.rgb * max(0, dot(normalDirection, _WorldSpaceLightPos0.xyz)) * 0.4 * _MainTex_var.rgb;
			float4 _SpecularmapGlossA_var = tex2D(_SpecularmapGlossA,i.uv0);
			float3 specular = pow(max(0,dot(halfDirection, normalDirection)), _Gloss) * _SpecularColor.rgb * _SpecularmapGlossA_var.rgb * _SpecularIntensity;

			float3 finalColor = diffuse + specular;
			return float4(saturate(finalColor), 0); 
		}
		ENDCG
	}
}
/////////////////////////////////////////////////////////////////////////////

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
	uniform sampler2D _SpecularmapGlossA; uniform float4 _SpecularmapGlossA_ST;
	uniform float4 _SpecularColor;
	uniform float _SpecularIntensity;
	uniform float _Gloss;
	uniform sampler2D _Normalmap; uniform float4 _Normalmap_ST;
	uniform float _NormalIntensity;
	uniform float _Ambientlight;
	uniform float _Contrast;

	uniform float _ShadowIntensiy;

	struct VertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 tangent : TANGENT;
		float2 texcoord0 : TEXCOORD0;
	};

	struct VertexOutput {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float3 viewDir : TEXCOORD1;
		float3 normalDir : TEXCOORD2;
		float3 tangentDir : TEXCOORD3;
		float3 bitangentDir : TEXCOORD4;
		LIGHTING_COORDS(5,6)
	};

	VertexOutput vert(VertexInput v) {
		VertexOutput o = (VertexOutput)0;
		o.uv0 = v.texcoord0;
		o.normalDir = UnityObjectToWorldNormal(v.normal);
		o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
		o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
		float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
		o.viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);
		o.pos = UnityObjectToClipPos(v.vertex);
		TRANSFER_VERTEX_TO_FRAGMENT(o);
		return o;
	}

	float4 frag(VertexOutput i) : COLOR{
		float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
		float3 _Normalmap_var = UnpackNormal(tex2D(_Normalmap, i.uv0));
		float3 Normalmap = float3(0, 0, 1 - _NormalIntensity) + _Normalmap_var * _NormalIntensity;// lerp(float3(0, 0, 1), _Normalmap_var.rgb, _NormalIntensity);
		float3 normalDirection = normalize(mul(Normalmap, tangentTransform)); // Perturbed normals
																			  //float3 lightDirection = _WorldSpaceLightPos0.xyz;
		float3 halfDirection = normalize(i.viewDir + _WorldSpaceLightPos0.xyz);
		////// Lighting:
		float attenuation = LIGHT_ATTENUATION(i);
		float3 attenColor = attenuation * _LightColor0.xyz;
		///////// Gloss:
		////// Specular:
		float NdotL = max(0, dot(normalDirection, _WorldSpaceLightPos0.xyz));
		float NdotV = max(0, dot(normalDirection, i.viewDir));

		float forwardLight = (_Contrast + 1.0) * NdotL - _Contrast * NdotL * NdotL;
		float3 Diffusemap =  tex2D(_MainTex, i.uv0).rgb;
		float3 diffuse = (forwardLight * attenColor* (1.0f - _ShadowIntensiy) + _Ambientlight * INGAME_AMBIENT_SKIN) * Diffusemap;

		float4 _SpecularmapGlossA_var = tex2D(_SpecularmapGlossA,i.uv0);
		float3 Specularmap = _SpecularColor.rgb * _SpecularmapGlossA_var.rgb * 2 * _SpecularIntensity * (1 - NdotV);
		float3 specular = pow(max(0, dot(halfDirection, normalDirection)), _Gloss) * Specularmap;

		/// Final Color:
		float3 finalColor = diffuse + specular;
		float4 finalRGBA = float4(saturate(finalColor),0);

		return finalRGBA;
	}
		ENDCG
	}
}

	Fallback "BRMobile/VertexLit"
}
