// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


#pragma fragmentoption ARB_precision_hint_fastest	

#include "UnityCG.cginc"

sampler2D _MainTex;
half _CutOff;
half4 _TintColor;
half _FadeFactor;
float4 _MainTex_ST;

#if _SEPERATE_ALPHA_TEX_ON
sampler2D _AlphaTex;
#endif

struct v2f
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	half4 color : TEXCOORD1;
};

v2f vert (appdata_full v)
{
	v2f o;
	o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
	o.pos = UnityObjectToClipPos(v.vertex);
	o.color = v.color;
#if _TINTCOLOR_ON
	o.color *= _TintColor * 2;
#endif
	return o;
}

fixed4 frag (v2f i) : COLOR
{
#if _SEPERATE_ALPHA_TEX_ON
	half4 color = half4(tex2D(_MainTex, i.uv.xy).rgb, tex2D(_AlphaTex, i.uv.xy).r);
#else
	half4 color = tex2D(_MainTex, i.uv.xy);
#endif
	
#if _CUTOFF_ON
	if (color.a < _CutOff)
		discard;
#endif
	
	color *= i.color;
	
	color.a *= _FadeFactor;
	return color;
}




