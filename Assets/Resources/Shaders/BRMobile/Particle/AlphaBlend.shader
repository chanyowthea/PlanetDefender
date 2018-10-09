Shader "BRMobile/Particle/AlphaBlend"
{

	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_AlphaTex ("千万不要填，系统用的", 2D) = "white" {}
		_CutOff ("Cut off", float) = 0.5
		_FadeFactor("Fade Factor", float) = 1
		_ZTestMode("ZTestMode", float) = 4
	}
		
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		ColorMask RGB
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off 
		Lighting Off 
		ZWrite Off 
		ZTest [_ZTestMode]
		Fog { Mode Off }
		
		LOD 200
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _DUMMY _TINTCOLOR_ON
			#pragma multi_compile _DUMMY _CUTOFF_ON
			#pragma multi_compile _DUMMY _SEPERATE_ALPHA_TEX_ON
			#include "Assets/Resources/Shaders/BRMobile/Particle/Particles.cginc"
			ENDCG 
		}
	}

	CustomEditor "SGameParticleMaterialEditor"
}

