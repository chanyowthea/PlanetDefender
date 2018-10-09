Shader "BRMobile/Special/Water" {
	Properties{
		_NormalTex0("Normal0", 2D) = "white" {}
		_NormalTex1("Normal1", 2D) = "white" {}
		_Tile0("Tile0 ", Float) = 0.1
		_Tile1("Tile1 ", Float) = 0.1
		_SpeedX0("Speed0 X", Float) = 1.0
		_SpeedY0("Speed0 Y", Float) = 1.0
		_SpeedX1("Speed1 X", Float) = 1.0
		_SpeedY1("Speed1 Y", Float) = 1.0
		_CubeTex("Reflection", Cube) = "_Skybox" { TexGen CubeReflect }
		_MaskTex("Mask", 2D) = "black" {}
		_Color("Color", Color) = (1,1,1,1)

		_DiffuseLow("DiffuseTex(Low)", 2D) = "white" {}
		_TileLow("TileLow ", Float) = 0.1
		_SpeedXLow("Speed Low X", Float) = 1.0
		_SpeedYLow("Speed Low Y", Float) = 1.0
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
#include "../BRMobileCommon.cginc"

	sampler2D _NormalTex0;
	sampler2D _NormalTex1;
	samplerCUBE _CubeTex;
	sampler2D _MaskTex;
	fixed4 _Color;
	fixed4 _AmbientColor;
	fixed4 _LightDir;
	fixed _Tile0;
	fixed _Tile1;
	fixed _SpeedX0;
	fixed _SpeedY0;
	fixed _SpeedX1;
	fixed _SpeedY1;
	float _TimeOverride;
	float4 _MapPos;

	struct v2f {
		float4 pos : SV_POSITION;
		float3 color : TEXCOORD0;
		float3 worldPos : TEXCOORD1;
		float4 scrollUV : TEXCOORD2;
		float2 maskUV : TEXCOORD3;
		UNITY_FOG_COORDS(4)
	};

	struct a2v {
		float4 vertex : POSITION;
		float3 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	v2f vert(a2v v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.color = v.color;

		float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
		o.worldPos = posWorld.xyz;
		float2 worldUV = posWorld.xz * 0.025 - floor(_WorldSpaceCameraPos.xz* 0.025);
		o.scrollUV = float4(worldUV.x, worldUV.y, worldUV.x, worldUV.y) * fixed4(_Tile0, _Tile0, _Tile1, _Tile1);

		float4 moveUV = fixed4(_SpeedX0, _SpeedY0, _SpeedX1, _SpeedY1) *_TimeOverride;
		o.scrollUV = o.scrollUV + moveUV;

		o.maskUV = float2(posWorld.x - _MapPos.x, posWorld.z - _MapPos.y) * _MapPos.z + 0.5f;

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}

	half4 frag(v2f i) : COLOR{
		fixed3 eyeDir = (_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
		fixed eyeLen = length(eyeDir);
		eyeDir = normalize(eyeDir);

		fixed3 normal1 = UnpackNormal(tex2D(_NormalTex0, i.scrollUV.xy)).xzy;
		fixed3 normal2 = UnpackNormal(tex2D(_NormalTex1, i.scrollUV.zw)).xzy;
		fixed3 normal = normalize(normal1 + normal2);

		fixed3 refl = texCUBE(_CubeTex, reflect(-eyeDir, normal)).rgb;
		fixed3 color = refl * _Color ;
		color = lerp(color, _Color, saturate(eyeLen * 0.01f));

		fixed mask = 1.0f - tex2D(_MaskTex, i.maskUV).r;
		mask = saturate(mask - 0.1f);
		mask = mask * mask * mask;

		float  cos_angle = clamp(1.0 - dot(normal, eyeDir), 0.5f, 1.0f);
		cos_angle *= cos_angle;

		float alpha = _Color.a * cos_angle * 3.5f * mask;
		alpha = max(alpha, _MapPos.w);

		float4 finalCol = float4(color, alpha);

		BRFOG(i.fogCoord, finalCol.rgb);

		return finalCol;
	}
		ENDCG
	}
	}

	////////////////////////////////////////////////

	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
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
#include "../BRMobileCommon.cginc"

	sampler2D _DiffuseLow;
	fixed4 _Color;
	fixed _TileLow;
	fixed _SpeedXLow;
	fixed _SpeedYLow;
	float _TimeOverride;

	struct v2f {
		float4 pos : SV_POSITION;
		float3 color : TEXCOORD0;
		float3 worldPos : TEXCOORD1;
		float2 scrollUV : TEXCOORD2;
		UNITY_FOG_COORDS(3)
	};

	struct a2v {
		float4 vertex : POSITION;
		float3 color : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	v2f vert(a2v v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.color = v.color;

		float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
		o.worldPos = posWorld.xyz;

		float2 worldUV = posWorld.xz * 0.025 - floor(_WorldSpaceCameraPos.xz* 0.025);
		float2 tmpSpeed = float2(_SpeedXLow, _SpeedYLow) * _TimeOverride;
		float2 tmpWorldUV = worldUV * _TileLow;
		o.scrollUV = tmpWorldUV + tmpSpeed;

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}


	half4 frag(v2f i) : COLOR{
		fixed3 eyeDir = (_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
		fixed eyeLen = length(eyeDir);

		fixed3 color = tex2D(_DiffuseLow, i.scrollUV).rgb;
		color = lerp(color, _Color, saturate(eyeLen * 0.01f));
		float4 finalCol = float4(color, _Color.a * 0.95f);

		return finalCol;
	}
		ENDCG
	}
	}

	FallBack "BRMobile/Diffuse"
}