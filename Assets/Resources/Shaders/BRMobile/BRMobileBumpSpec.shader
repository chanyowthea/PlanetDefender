// Simplified Bumped Specular shader. Differences from regular Bumped Specular one:
// - no Main Color nor Specular Color
// - specular lighting directions are approximated per vertex
// - writes zero to alpha channel
// - Normalmap uses Tiling/Offset of the Base texture
// - no Deferred Lighting support
// - no Lightmap support
// - supports ONLY 1 directional light. Other lights are completely ignored.

Shader "BRMobile/Bumped Specular" {
Properties {
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
    _SpecularColor("Specular Color (RGBA)", Color) = (1, 1, 1, 1)
	_SpecularMask("Specular Mask", 2D) = "white" {}
	_SpecularIntensity ("Specular Intensity", Range (0, 10)) = 1
}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 300
	
CGPROGRAM
#pragma surface surf MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview interpolateview

fixed4 _SpecularColor; 
fixed _SpecularIntensity;

inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
	fixed diff = max (0, dot (s.Normal, lightDir));
	fixed nh = max (0, dot (s.Normal, halfDir));
	fixed spec = pow (nh, s.Specular*128) * s.Gloss;
	
	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + _SpecularIntensity * _SpecularColor.rgb * _LightColor0.rgb * spec) * atten;
	UNITY_OPAQUE_ALPHA(c.a);
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
	fixed4 mask = tex2D(_SpecularMask, IN.uv_MainTex);
	o.Albedo = tex.rgb;
	o.Gloss = tex.a * mask.r;
	o.Specular = _Shininess;
	o.Normal = UnpackNormal (tex2D(_BumpMap, IN.uv_MainTex));
}
ENDCG
}

FallBack "BRMobile/Bumped Diffuse"
}
