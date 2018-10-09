// Simplified Bumped Specular shader. Differences from regular Bumped Specular one:
// - no Main Color nor Specular Color
// - specular lighting directions are approximated per vertex
// - writes zero to alpha channel
// - Normalmap uses Tiling/Offset of the Base texture
// - no Deferred Lighting support
// - no Lightmap support
// - supports ONLY 1 directional light. Other lights are completely ignored.

Shader "BRMobile/Special/Hair" {
Properties {
	_MainTex ("Base (RGBA))", 2D) = "white" {}
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
    _SpecularColor("Specular Color (RGBA)", Color) = (1, 1, 1, 1)
	_SpecularMask ("Specular Mask", 2D) = "white" {}
	//_BumpMap ("Normalmap", 2D) = "bump" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	_AmbientLight ("Ambient", Range(0, 1)) = 0.2
}
SubShader { 
	Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
	LOD 250
	Cull Off

CGPROGRAM
#pragma surface surf MobileBlinnPhong exclude_path:prepass alphatest:_Cutoff nolightmap halfasview

fixed4 _SpecularColor; 
fixed _AmbientLight;
fixed _ShadowIntensiy;

inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
	fixed diff = max (0, dot (s.Normal, lightDir));
	fixed nh = max (0, dot (s.Normal, halfDir));
	fixed spec = pow (nh, s.Specular*128) * s.Gloss;
	
	fixed4 c;
	c.rgb = _AmbientLight * s.Albedo + (s.Albedo * _LightColor0.rgb * diff + _SpecularColor.rgb * _SpecularColor.a * _LightColor0.rgb * spec) * atten;
	c.a = s.Alpha;
	return c;
}

sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _SpecularMask;
half _Shininess;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 gloss = tex2D(_SpecularMask, IN.uv_MainTex);
	o.Albedo = tex.rgb * (1.0f - _ShadowIntensiy);
	o.Alpha = tex.a;
	o.Gloss = gloss.r;
	o.Specular = _Shininess;
	//o.Normal = UnpackNormal (tex2D(_BumpMap, IN.uv_MainTex));
}
ENDCG
}

FallBack "BRMobile/VertexLit"
}
