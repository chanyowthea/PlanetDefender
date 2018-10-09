Shader "BRMobile/Special/UVScrolling" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_ScrollXSpeed("X Scroll Speed", Range(0, 100)) = 2
		_ScrollYSpeed("Y Scroll Speed", Range(0, 100)) = 2
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 300

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd

		sampler2D _MainTex;
		fixed _ScrollXSpeed;
		fixed _ScrollYSpeed;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed2 scrolledUV = IN.uv_MainTex;
			fixed u = _ScrollXSpeed * _Time;
			fixed v = _ScrollYSpeed * _Time;
			scrolledUV += fixed2(u, v);

			fixed4 c = tex2D(_MainTex, scrolledUV);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}
