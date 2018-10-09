// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "lg/Effect_Dissolve" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _DiffuseColor ("DiffuseColor", Color) = (0.5,0.5,0.5,1)
        _DiffuseBright ("DiffuseBright", Float ) = 1
        _dissolveamount ("dissolve amount", Range(0, 1)) = 0
        _dissolveRamp ("dissolveRamp", 2D) = "white" {}
        _MaskTexture ("MaskTexture", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {

            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform float _dissolveamount;
            uniform sampler2D _dissolveRamp; uniform float4 _dissolveRamp_ST;
            uniform sampler2D _MaskTexture; uniform float4 _MaskTexture_ST;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _DiffuseBright;
            uniform float4 _DiffuseColor;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _MaskTexture_var = tex2D(_MaskTexture,TRANSFORM_TEX(i.uv0, _MaskTexture));
                float clipLevel = (((1.0 - _dissolveamount)*1.2+-0.6)+_MaskTexture_var.r);
                clip(clipLevel - 0.5);

                float4 mainColor = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));

                float2 dissolvePos = float2((1.0 - saturate((clipLevel*8.0+-4.0))), 0.0);
                float4 dissolveRamp = tex2D(_dissolveRamp, TRANSFORM_TEX(dissolvePos, _dissolveRamp));
                float3 emissive = mainColor.rgb * _DiffuseColor.rgb * _DiffuseBright * 2.0 * dissolveRamp.rgb;

                float3 finalColor = mainColor + emissive;

                return fixed4(finalColor, mainColor.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
