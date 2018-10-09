Shader "BRMobile/Glass" {
    Properties {
        _Color("Tint Color (RGB)", Color) = (1, 1, 1, 1)
        _DirtTex("Dirt Texture (RGB)", 2D) = "white" {}
        _Alpha("Alpha", Range(0.0, 1.0)) = 0.3

        [Header(Specular)]
        _SpecularColor("Specular Color (RGBA)", Color) = (1, 1, 1, 0.5)
        _Shininess ("Shininess", Range(0.03, 1)) = 0.078125

        [Header(Reflection)]
        [MaterialToggle(USE_REFLECTION_ON)] _Use_Reflection ("Use Reflection", Float ) = 0
        _Reflectivity("Reflections Power", Range(0.0, 1.0)) = 1.0
        [MaterialToggle(CUSTOM_CUBEMAP_ON)] _Custom_Cubemap ("Use Custom Cubemap", Float ) = 0
		[NoScaleOffset] _ReflectCube ("Custom Cubemap", Cube) = "grey" {}
    }
 
    SubShader {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" }
        LOD 300

        Pass {
        	ColorMask 0
        	ZWrite On
        }
       
        Pass {
            Name "BASE"
            Tags { "LightMode" = "ForwardBase" }
           
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
 
            CGPROGRAM
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
			#include "BRMobileCommon.cginc"
            //#pragma target 2.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile USE_REFLECTION_OFF USE_REFLECTION_ON
            #pragma multi_compile CUSTOM_CUBEMAP_OFF CUSTOM_CUBEMAP_ON
 
            fixed4 _Color;
            fixed4 _SpecularColor;
            sampler2D _DirtTex;
            float4 _DirtTex_ST;
            float _Shininess;
            fixed _Alpha;
            float _Reflectivity;

#if CUSTOM_CUBEMAP_ON       
			half4 _ReflectCube_HDR;
			UNITY_DECLARE_TEXCUBE(_ReflectCube);
#endif			


            struct a2v {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };
 
            struct v2f {
                float4 pos : SV_POSITION;
                float2 coord0 : TEXCOORD0;
                float3 norm : TEXCOORD1;
                float3 view : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
 
            v2f vert (a2v v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.coord0 = TRANSFORM_TEX(v.texcoord, _DirtTex);
                o.norm = UnityObjectToWorldNormal(v.normal);
                o.view = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }
 
            fixed4 frag(v2f i) : SV_Target {

                fixed3 viewDirection = normalize(i.view);
                fixed3 halfDirection = normalize(_WorldSpaceLightPos0.xyz + viewDirection);

                fixed4 specular;
                if (dot(i.norm, _WorldSpaceLightPos0.xyz) < 0)
                {
                	specular = fixed4(0.0, 0.0, 0.0, 0.0);
                }
                else
                {
                	specular = _LightColor0 * _SpecularColor * pow(max(0.0, dot(i.norm, halfDirection)), _Shininess * 128);
                }

                fixed3 dirt = tex2D(_DirtTex, i.coord0).rgb * _Color.rgb;

#if USE_REFLECTION_OFF
                fixed3 result = dirt; 
#else                

#if CUSTOM_CUBEMAP_ON                
            	fixed4 refl = UNITY_SAMPLE_TEXCUBE(_ReflectCube, reflect(-viewDirection, i.norm));
                refl.rgb = DecodeHDR(refl, _ReflectCube_HDR);
#else
            	fixed4 refl = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflect(-viewDirection, i.norm));
                refl.rgb = DecodeHDR(refl, unity_SpecCube0_HDR);
#endif            	
                fixed3 result = lerp(dirt, refl, _Reflectivity);
#endif
                result = specular.rgb + result * _Alpha;

				BRFOG(i.fogCoord, result);

                return fixed4(result, saturate(_Alpha + specular.a));
            }
            ENDCG
        }
    }
 
    Fallback "BRMobile/AlphaBlendColor"
}