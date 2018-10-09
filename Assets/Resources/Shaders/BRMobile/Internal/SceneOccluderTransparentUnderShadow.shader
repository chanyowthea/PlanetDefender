Shader "BRMobile/Internal/SceneOccluderTransparentUnderShadow" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_Brightness("Brightness", Range(0, 5)) = 1
		[Enum(Off,0,Front,1,Back,2)] _CullMode ("Cull", Int) = 2
		_Cutoff ("Alpha cutoff", Range (0,1)) = 0.0
		//_Order("Order", Float) = 0
	}
	SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

		//Offset [_Order], [_Order]
		Cull [_CullMode] 
		ZWrite Off
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha 
		CGPROGRAM
		#pragma surface surf NoLight noforwardadd keepalpha
		#include "NoLight.cginc"
		#include "Occluder.cginc"

		sampler2D _MainTex;
		fixed4 _Color;
		fixed _Cutoff;
		fixed _Brightness;
		float3 _PlayerPos;
		float3 _CameraPos;
		float _AlphaAnimation;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutput o) {

			float3 fragDir = IN.worldPos - _CameraPos;
			float3 segDir = _PlayerPos - _CameraPos;
			float3 playerDir = _PlayerPos - IN.worldPos;

			float proj = dot(fragDir, segDir);
			float segLength = dot(segDir, segDir);
			
			float dsq;
			if (proj < 0)
			{
				dsq = dot(fragDir, fragDir);
			}
			else if (proj > segLength)
			{
				dsq = dot(playerDir, playerDir);
			}
			else
			{
				dsq = dot(fragDir, fragDir) - proj * proj / segLength;
			}
			
			if (dsq <= OccluderOuterRadius)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				if (c.a < _Cutoff)
				{
					discard;
					return;
				}
				o.Albedo = c.rgb * _Brightness;
				if (dsq <= OccluderRadius)
					o.Alpha = saturate(dsq * OccluderRadiusInverse + _AlphaAnimation);
				else
					o.Alpha = saturate((OccluderOuterRadius - dsq) * OccluderOuterRadiusFade);
				return;
			}

			discard;
		}
		ENDCG
	}
}
