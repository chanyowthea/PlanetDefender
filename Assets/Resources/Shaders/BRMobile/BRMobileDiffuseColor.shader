Shader "BRMobile/DiffuseColor" {

	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_Brightness("Brightness", Range(0, 5)) = 1
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase"}
		LOD 300

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ DISABLE_LIGHTMAP
#pragma multi_compile_fog

#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"
#include "UnityLightingCommon.cginc"
#include "BRMobileCommon.cginc"

		sampler2D _MainTex;
		fixed _Brightness;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)        
		float2 uv1 : TEXCOORD2;
#endif	
		fixed3 diffuseColor : COLOR;
		UNITY_FOG_COORDS(1)
	
	};

	struct a2v {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)        
		float2 texcoord1 : TEXCOORD1;
#endif		
		float3 normal : NORMAL;
		
	};

	fixed4 _Color;

	v2f vert(a2v i) {
		v2f o;
		o.pos = UnityObjectToClipPos(i.vertex);
		o.uv0 = i.texcoord;

#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)              
		o.uv1 = i.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif		
		half3 worldNormal = UnityObjectToWorldNormal(i.normal);
		o.diffuseColor = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz)) * _LightColor0 + UNITY_LIGHTMODEL_AMBIENT;
	

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}


	half4 frag(v2f i) : COLOR{

		half4 color = tex2D(_MainTex, i.uv0) * _Color * _Brightness;

#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)
		fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1));
		color.rgb *= lm * clamp(i.diffuseColor.r, LightmapMinDiffuse, 1.0);
#else		
		color.rgb *= i.diffuseColor;
#endif

		BRFOG(i.fogCoord, color.rgb);

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

		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ DISABLE_LIGHTMAP

#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"
#include "BRMobileCommon.cginc"

	sampler2D _MainTex;
	fixed _Brightness;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)        
		float2 uv1 : TEXCOORD1;
#endif				
	};

	struct a2v {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)        
		float2 texcoord1 : TEXCOORD1;
#endif			
	};

	fixed4 _Color;

	v2f vert(a2v i) {
		v2f o;
		o.pos = UnityObjectToClipPos(i.vertex);
		o.uv0 = i.texcoord;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)              
		o.uv1 = i.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif	
		return o;
	}


	half4 frag(v2f i) : COLOR{
		half4 color = tex2D(_MainTex, i.uv0) * _Color * _Brightness;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)
		fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1));
		color.rgb *= lm;
#else
		color.rgb *= ConstantBrightness;
#endif	
		return color; 
	}
		ENDCG
	}

	}
	Fallback "BRMobile/VertexLit"
}
