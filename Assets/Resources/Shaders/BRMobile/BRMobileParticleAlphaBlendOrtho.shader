// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BRMobile/ParticlesAlphaBlendedOrtho"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_WorldPos("_WorldPos", Vector) = (0,0,0,1)
		_Near("_Near",  float) = 2.5
		_NearScale("_NearScale",  float) = 3
		_Far("_Far",  float) = 100
		_FarScale("_FarScale",  float) = 10
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _Color;
			float4 _MainTex_ST;
			float4 _WorldPos;

			float _Near;
			float _NearScale;
			float _Far;
			float _FarScale;

			v2f vert(appdata v)
			{
				v2f o;

				float4 centerW = _WorldPos;

				float4 ppos = UnityObjectToClipPos(v.vertex);
				//float s = ppos.w / unity_CameraProjection._m11;			//per vertex
				float dist = clamp(ppos.w, _Near, _Far);
				float weight = (dist - _Near) / (_Far - _Near);
				float s = lerp(_NearScale, _FarScale, weight);

				float4 wpos = mul(UNITY_MATRIX_M, v.vertex);

				wpos.xyz = centerW.xyz + s * (wpos.xyz - centerW.xyz);		

				o.vertex = mul(UNITY_MATRIX_VP, wpos);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col * i.color;
			}
			ENDCG
		}
	}
}
