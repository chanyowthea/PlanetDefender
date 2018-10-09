Shader "BRMobile/Internal/DiffuseTwoSidesUnderShadow" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_Brightness("Brightness", Range(0, 5)) = 1
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
		LOD 200
		Cull Off

		Pass{

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fog

		#include "UnityCG.cginc"
		#pragma multi_compile_fwdbase
		#include "AutoLight.cginc"
		#include "../BRMobileCommon.cginc"

		sampler2D _MainTex;
		fixed _Brightness;

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv0 : TEXCOORD0;
			UNITY_FOG_COORDS(1)
		};

		struct a2v {
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
		};

		fixed4 _Color;

		v2f vert(a2v i) {
			v2f o;
			o.pos = UnityObjectToClipPos(i.vertex);
			o.uv0 = i.texcoord;

			UNITY_TRANSFER_FOG(o, o.pos);
			return o;
		}


		half4 frag(v2f i) : COLOR{

			half4 color = tex2D(_MainTex, i.uv0) * _Color * _Brightness;
			color.rgb *= 0.8;
			BRFOG(i.fogCoord, color);

			return color;
		}
			ENDCG
		}
	}

	Fallback "BRMobile/VertexLit"
}
