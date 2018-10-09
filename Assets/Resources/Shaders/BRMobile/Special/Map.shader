// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BRMobile/Special/Map 1"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
		_CenterPos("Center Position", Vector) = (0.5, 0.5, 0, 0)
		_LineWidth("LineWidth",Float) = 0.002
		_Radius("Radius", Float) = 0.1
		_Color("Color", Color) = (1,1,1,1)
		_Transparency("Transparency", Float) = 1
	}

	SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Offset -1, -1
			Fog { Mode Off }
			//ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
			float2 _ClipArgs0 = float2(1000.0, 1000.0);

			struct appdata_t
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 worldPos : TEXCOORD1;
			};

			float2 _CenterPos;
			float _LineWidth;
			float _Radius;
			float _OuterRadius;
			fixed4 _Color;
			v2f o;
			float _Transparency;

			v2f vert (appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;
				o.worldPos = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
				return o;
			}

			half4 frag (v2f IN) : SV_Target
			{
				// Softness factor
				float2 factor = (float2(1.0, 1.0) - abs(IN.worldPos)) * _ClipArgs0;
			
				// Sample the texture
				half4 col = half4(0, 0, 0, 0);

				float2 dir = IN.texcoord - _CenterPos;
				float sqrDist = dot(dir, dir);

				if (sqrDist < _LineWidth && sqrDist > _Radius)
				{
					col = _Color;
				}
				
			    col.a *= clamp( min(factor.x, factor.y), 0.0, 1.0 *_Transparency);
				return col;
			}
			ENDCG
		}
	}
}
