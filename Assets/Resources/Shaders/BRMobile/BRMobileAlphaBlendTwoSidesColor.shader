Shader "BRMobile/AlphaBlendTwoSidesColor" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_Brightness("Brightness", Range(0, 5)) = 1
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "LightMode" = "ForwardBase"}
		LOD 300
		Cull Off

		Pass{

		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ DISABLE_LIGHTMAP
#pragma multi_compile_fog

#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"
#include "BRMobileCommon.cginc"

		sampler2D _MainTex;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)        
		float2 uv1 : TEXCOORD1;
		UNITY_FOG_COORDS(2)
#else
		UNITY_FOG_COORDS(1)
#endif		
	};

	struct a2v {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)        
		float2 texcoord1 : TEXCOORD1;
#endif		
	};

	v2f vert(a2v i) {
		v2f o;
		o.pos = UnityObjectToClipPos(i.vertex);
		o.uv0 = i.texcoord;

#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)              
		o.uv1 = i.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif	

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}


	half4 frag(v2f i) : COLOR{
		half4 color = tex2D(_MainTex, i.uv0);

#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)
		fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1));
		color.rgb *= lm;
#else
		color.rgb *= ConstantBrightness;
#endif
		BRFOG(i.fogCoord, color);

		return color;
	}
		ENDCG
	}
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "LightMode" = "ForwardBase"}
		LOD 200
		Cull Off

			Pass{

			Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ DISABLE_LIGHTMAP
#pragma multi_compile_fog

#include "UnityCG.cginc"
#pragma multi_compile_fwdbase
#include "AutoLight.cginc"
#include "BRMobileCommon.cginc"

		sampler2D _MainTex;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)        
		float2 uv1 : TEXCOORD1;
		UNITY_FOG_COORDS(2)
#else
		UNITY_FOG_COORDS(1)
#endif		
	};

	struct a2v {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)        
		float2 texcoord1 : TEXCOORD1;
#endif		
	};

	v2f vert(a2v i) {
		v2f o;
		o.pos = UnityObjectToClipPos(i.vertex);
		o.uv0 = i.texcoord;

#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)              
		o.uv1 = i.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif	

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}


	half4 frag(v2f i) : COLOR{
		half4 color = tex2D(_MainTex, i.uv0);

#if defined(LIGHTMAP_ON) && !defined(DISABLE_LIGHTMAP)
		fixed3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1));
		color.rgb *= lm;
#else
		color.rgb *= ConstantBrightness;
#endif
		BRFOG(i.fogCoord, color);

		return color;
	}
		ENDCG
		}

	}
	Fallback "BRMobile/AlphaBlendVertexLit"
}
