Shader "BRMobile/Special/Helmet" {

	Properties{
		_MainTex("Diffuse map", 2D) = "white" {}
		_AmbientLight("Ambient light", Range(0, 8)) = 0
		_SpecularColor("Specular Color", Color) = (1,1,1,1)
		_SpecularmapGlossA("Specular map(Gloss A)", 2D) = "white" {}
		_SpecularIntensity("Specular Intensity", Range(0, 32)) = 1.0
		//_Gloss("Gloss", Range(0, 10)) = 1
		_Normalmap("Normal map", 2D) = "bump" {}
		_NormalIntensity("Normal Intensity", Range(0, 2)) = 1.0
		_CubeTex("Reflection", Cube) = "_Skybox" { TexGen CubeReflect }
		_ReflectIntensity("Reflect Intensity", Range(0, 1)) = 0.5
	}

SubShader{
	Tags{ "RenderType" = "Opaque"}
	LOD 200

	Pass{
	Name "FORWARD"
	Tags{
		"LightMode" = "ForwardBase"
	}

	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma target 2.0
	#pragma multi_compile_fwdbase


	#include "UnityCG.cginc"
	#include "AutoLight.cginc"
	#include "Lighting.cginc"

	uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
	uniform sampler2D _Normalmap; uniform float4 _Normalmap_ST;
	uniform sampler2D _SpecularmapGlossA;
	uniform	samplerCUBE _CubeTex; 


	//uniform fixed _Gloss;
	uniform fixed _AmbientLight;
	uniform fixed _NormalIntensity;

	uniform fixed4 _SpecularColor;
	uniform fixed _SpecularIntensity;

	uniform fixed _ReflectIntensity;
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
		float3 Normalmap = float3(0, 0, 1 - _NormalIntensity) + _Normalmap_var * _NormalIntensity;
		float3 normalDirection = normalize(mul(Normalmap, tangentTransform));

		float diff = max(0, dot(normalDirection, _WorldSpaceLightPos0.xyz));

		float4 _SpecularmapGlossA_var = tex2D(_SpecularmapGlossA,i.uv0);
		float3 halfDirection = normalize(i.viewDir + _WorldSpaceLightPos0.xyz);

		float3 spec = pow(max(0, dot(halfDirection, normalDirection)), _SpecularmapGlossA_var.b * 640.0) * _SpecularmapGlossA_var.r * _SpecularColor * _SpecularIntensity;

		float3 texColor = tex2D(_MainTex, i.uv0).rgb;

		fixed4 refl = texCUBE(_CubeTex, reflect(normalize(-i.viewDir), normalDirection));

		/// Final Color:
		float3 finalRGB = (_AmbientLight + (diff + spec) * _LightColor0 * (1.0f - _ShadowIntensiy)) * texColor;
		finalRGB = lerp(finalRGB, refl.rgb, _ReflectIntensity * _SpecularmapGlossA_var.g);

		return float4(finalRGB, 1);
	}
		ENDCG
	}
}
}