// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BRMobile/UnlitBlend/Multiply" {
    Properties {
        _Color ("Color Tint", Color) = (1, 1, 1, 0.4)
        _MainTex ("Main Tex", 2D) = "white" {}
    }
    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

        Pass {
            Tags { "LightMode"="ForwardBase" }

            ZWrite Off

            Blend DstColor Zero

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Lighting.cginc"

            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct a2v {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(a2v v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target {                
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return fixed4(lerp(fixed3(1,1,1), texColor.rgb * _Color.rgb, texColor.a * _Color.a), texColor.a);
            }

            ENDCG
        }
    } 
    FallBack "BRMobile/AlphaBlendVertexLit"
}