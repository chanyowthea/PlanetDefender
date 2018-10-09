#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced '_WorldSpaceCameraPos.w' with '1.0'

// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "BRMobile/Internal/Grass" {
	Properties {
		//_WavingTint ("Fade Color", Color) = (.7,.6,.5, 0)
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		//_WaveAndDistance ("Wave and distance", Vector) = (12, 3.6, 1, 1)
		_Cutoff ("Cutoff", float) = 0.5
		[MaterialToggle(CUSTOM_MIPMAPS)] _Custom_Mipmaps ("Use Custom Mipmaps", Float ) = 0
	}
	
SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 300
	Cull Off
	Lighting Off

	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile _ CUSTOM_MIPMAPS
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
        		float3 worldPos : TEXCOORD1;
#if CUSTOM_MIPMAPS				
				float camDist : TEXCOORD2;
#endif			
			};

      		#include "../DFMVertexFragmentShaderCommon.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Cutoff;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
       			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
#if CUSTOM_MIPMAPS				
				float3 camDir = _WorldSpaceCameraPos - o.worldPos;
				o.camDist =  dot(camDir, camDir);
#endif				
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;
#if CUSTOM_MIPMAPS				
				if (i.camDist > 100.0)
				 	col = tex2D(_MainTex, float2(i.texcoord.x * 0.25 + 0.75, i.texcoord.y * 0.25));
				else if (i.camDist > 36.0)
					col = tex2D(_MainTex, float2(i.texcoord.x * 0.5 + 0.5, i.texcoord.y * 0.5));
				else
#endif					
					col = tex2D(_MainTex, i.texcoord);

				col.rgb *= i.color.r;
				clip(col.a - _Cutoff);
        		fogColor(i, col);
				return col;
			}
		ENDCG
	}
}

FallBack "BRMobile/CutOffTwoSidesColor"
}
