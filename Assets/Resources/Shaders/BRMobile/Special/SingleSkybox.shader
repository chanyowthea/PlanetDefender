// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BRMobile/Special/SingleSkybox" {
Properties {
	_Tint ("Tint Color", Color) = (.5, .5, .5, .5)
	_DownTex ("Down (-Y)", 2D) = "white" {}
}

SubShader {
	Tags { "Queue"="Background" "RenderType"="Background" }
	Cull Off ZWrite Off Fog { Mode Off }
	
	CGINCLUDE
	#include "UnityCG.cginc"

	fixed4 _Tint;
	
	struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
	};
	struct v2f {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
	};
	v2f vert (appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = v.texcoord;
		return o;
	}
	fixed4 skybox_frag (v2f i, sampler2D smp)
	{
		fixed4 tex = tex2D (smp, i.texcoord);
		fixed4 col;
		col.rgb = tex.rgb + _Tint.rgb - unity_ColorSpaceGrey;
		col.a = tex.a * _Tint.a;
		return col;
	}
	ENDCG

	Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		sampler2D _DownTex;
		fixed4 frag (v2f i) : SV_Target { return skybox_frag(i,_DownTex); }
		ENDCG
	}
}
}
