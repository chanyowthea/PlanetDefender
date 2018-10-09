// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "lg/Effect_2Tex_Emit" {
    Properties {
        _InstensityA ("InstensityA", Float ) = 1
        _Tex01 ("Tex01", 2D) = "black" {}
        _Tex01_Instensity ("Tex01_Instensity", Float ) = 1
        _Color_Tex01 ("Color_Tex01", Color) = (0.5,0.5,0.5,1)
        [MaterialToggle] _Tex01_RGBAlpha ("Tex01_RGB/Alpha", Float ) = 0
        _Tex01_PanV ("Tex01_PanV", Float ) = 0
        _Tex02 ("Tex02", 2D) = "white" {}
        _Tex02_Instensity ("Tex02_Instensity", Float ) = 1
        _Color_Tex02 ("Color_Tex02", Color) = (0.5,0.5,0.5,1)
        [MaterialToggle] _Tex02_RGBAlpha ("Tex02_RGB/Alpha", Float ) = 0
        _Tex02_PanV ("Tex02_PanV", Float ) = 0
        _Mask_Tex ("Mask_Tex", 2D) = "white" {}
        _AlphaInstensityA ("AlphaInstensityA", Float ) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent+1"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One, Zero One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 2.0

            uniform float _InstensityA;
            uniform float _AlphaInstensityA;

            uniform sampler2D _Tex01; uniform float4 _Tex01_ST;
            uniform float _Tex01_PanV;
            uniform float4 _Color_Tex01;

            uniform sampler2D _Tex02; uniform float4 _Tex02_ST;
            uniform float _Tex02_PanV;
            uniform float4 _Color_Tex02;

            uniform sampler2D _Mask_Tex; uniform float4 _Mask_Tex_ST;

            uniform fixed _Tex02_RGBAlpha;
            uniform fixed _Tex01_RGBAlpha;
            uniform float _Tex02_Instensity;
            uniform float _Tex01_Instensity;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }

            float4 frag(VertexOutput i) : COLOR {

                float2 rel = i.uv0 - 0.5;
                float radius = sqrt(rel.x * rel.x + rel.y * rel.y) * 1.414;
                float theta = atan2(rel.y, rel.x);

                if (theta < 0.0)
                    theta += 6.28318530718;
                theta /= 6.28318530718;

                float2 newUV = float2(theta, radius);
                float2 newUV01 = float2(newUV.x, newUV.y + _Tex01_PanV * _Time.g);
                float2 newUV02 = float2(newUV.x, newUV.y + _Tex02_PanV * _Time.g);

                float4 _Tex01_var = tex2D(_Tex01,TRANSFORM_TEX(newUV01, _Tex01));
                float4 _Tex02_var = tex2D(_Tex02,TRANSFORM_TEX(newUV02, _Tex02));

                float4 _Mask_Tex_var = tex2D(_Mask_Tex,TRANSFORM_TEX(newUV, _Mask_Tex));

                float3 emissive = (
                    (_InstensityA * (_Tex01_Instensity * _Tex01_var.rgb * _Color_Tex01.rgb * _Color_Tex01.a  + _Tex02_Instensity * _Tex02_var.rgb * _Color_Tex02.rgb * _Color_Tex02.a)) * 
                    (_AlphaInstensityA * _Mask_Tex_var.r * (lerp(dot(_Tex01_var, 1), _Tex01_var.a, _Tex01_RGBAlpha) + lerp(dot(_Tex02_var, 1), _Tex02_var.a, _Tex02_RGBAlpha)))
                    ) * i.vertexColor.rgb * i.vertexColor.a;

                return fixed4(emissive, 1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
