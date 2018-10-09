Shader "BRMobile/Internal/SceneOccluderClippedUnderShadow" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_Brightness("Brightness", Range(0, 5)) = 1
		[Enum(Off,0,Front,1,Back,2)] _CullMode ("Cull", Int) = 2
		_Cutoff ("Alpha cutoff", Range (0,1)) = 0.0
		//_Order("Order", Float) = 0
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }

		//Offset [_Order], [_Order]
		Cull [_CullMode] 
		ZWrite On
		CGPROGRAM
		#pragma surface surf NoLight noforwardadd alphatest:_Cutoff
		#include "NoLight.cginc"
		#include "Occluder.cginc"

		sampler2D _MainTex;
		fixed4 _Color;
		fixed _Brightness;
		float3 _PlayerPos;
		float3 _CameraPos;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutput o) {

			const float ratio = OccluderRadius;
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
			
			if (dsq < ratio)
			{
				discard; 
				return;
			}

			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * _Brightness;
			o.Alpha = c.a;
		}
		ENDCG
	}
}
