// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "lg/Effect_SpecularPanU"
{ 
    Properties  
    {          
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
        _MainTex ("Base_01 (RGB)", 2D) = "white" {} 
        _GlossTex ("Gloass Map", 2D) = "white" {}
		_Cube("Reflection Cubemap", Cube) = "" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}

		_Emission_Tex("Emission_Tex", 2D) = "black" {}
		_Emission_Tex_Instensity("Emission_Tex_Instensity", Float) = 1
		_Emission_Tex_Color("Emission_Tex_Color", Color) = (0.5,0.5,0.5,1)
		_Emission_Tex_UVAngle("Emission_Tex_UVAngle", Float) = 0
		_Emission_Tex_PanV("Emission_Tex_PanV", Float) = 0
		_Dis_Emission_Tex("Dis_Emission_Tex", 2D) = "white" {}
		_Dis_Emission_Tex_Power("Dis_Emission_Tex_Power", Float) = 0
		_Dis_Emission_Tex_PanV("Dis_Emission_Tex_PanV", Float) = 0
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_CustomAmbient("Custom Ambient", Color) = (0.5, 0.5, 0.5, 1)
		[MaterialToggle(SECOND_UV_ON)] _SecondUV("Use Second UV", Float) = 0
	}

    SubShader
    {
		Tags { "RenderType"="Opaque" }
		LOD 300

        Pass  
        { 
            CGPROGRAM 
    
            #pragma vertex Vert 
            #pragma fragment Frag 
            #pragma target 2.0
			#pragma multi_compile SECOND_UV_OFF SECOND_UV_ON
      
            #include "UnityCG.cginc" 
			#include "Lighting.cginc"
  
            sampler2D _MainTex; 
			fixed4 _MainTex_ST;
			sampler2D _GlossTex;
			sampler2D _BumpMap;
			samplerCUBE _Cube;

			uniform fixed4 _Color;
			uniform fixed4 _ReflectColor;
			uniform half _Shininess;

			uniform sampler2D _Emission_Tex; uniform float4 _Emission_Tex_ST;
			uniform sampler2D _Dis_Emission_Tex; uniform float4 _Dis_Emission_Tex_ST;
			uniform float _Dis_Emission_Tex_Power;
			uniform float _Emission_Tex_PanV;
			uniform float _Dis_Emission_Tex_PanV;
			uniform float4 _Emission_Tex_Color;
			uniform sampler2D _Mask_Tex; uniform float4 _Mask_Tex_ST;
			uniform float _Emission_Tex_UVAngle;
			uniform float _Emission_Tex_Instensity;
			
			uniform fixed4 _CustomAmbient;
						
			struct appdata_t 
			{
				fixed4 vertex : POSITION;
				float3 normal : NORMAL;
                float4 tangent : TANGENT;
				fixed2 texcoord : TEXCOORD0;
#if SECOND_UV_ON
				fixed2 texcoord1 : TEXCOORD1;
#endif				

			};
            struct V2F 
            { 
                fixed4 pos : POSITION;
				fixed2 uv0 : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
				float3 tangentDir : TEXCOORD3;
				float3 bitangentDir : TEXCOORD4;
#if SECOND_UV_ON
				fixed2 uv1 : TEXCOORD5;
#endif				
            }; 
                
          
            V2F Vert(appdata_t v) 
            { 
                V2F o; 
      
                o.pos = UnityObjectToClipPos(v.vertex); 
                o.uv0 = v.texcoord;
#if SECOND_UV_ON
                o.uv1 = v.texcoord1;
#endif                
				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.tangentDir = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
				o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
				float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);

                return o; 
            } 
              
            half4 Frag(V2F i):COLOR 
            { 
            	float3x3 tangentTransform = float3x3(i.tangentDir, i.bitangentDir, i.normalDir);
				float3 Normalmap = UnpackNormal(tex2D(_BumpMap, i.uv0));
				float3 normalDirection = normalize(mul(Normalmap, tangentTransform));

                fixed4 mainColor = tex2D(_MainTex, i.uv0) * _Color;

				float diff = max(0, dot(normalDirection, _WorldSpaceLightPos0.xyz));
				float3 halfDirection = normalize(i.viewDir + _WorldSpaceLightPos0.xyz);

				fixed4 gloss = tex2D(_GlossTex, i.uv0);
				float spec = pow(max(0, dot(halfDirection, normalDirection)), _Shininess * 128) * gloss.r;

				fixed4 reflcol = texCUBE(_Cube, reflect(normalize(-i.viewDir), normalDirection));
				reflcol *= gloss.r;

#if SECOND_UV_ON
				float maskEmission = tex2D(_Mask_Tex, TRANSFORM_TEX(i.uv1, _Mask_Tex)).r;
#else
				float maskEmission = tex2D(_Mask_Tex, TRANSFORM_TEX(i.uv0, _Mask_Tex)).r;
#endif

				float angle = 3.141592654 * _Emission_Tex_UVAngle;
				float cosAngle = cos(angle);
				float sinAngle = sin(angle);

#if SECOND_UV_ON
				float2 disUV = TRANSFORM_TEX(i.uv1, _Dis_Emission_Tex) + float2(0, _Time.g * _Dis_Emission_Tex_PanV);
				float4 disTex = tex2D(_Dis_Emission_Tex, disUV);
#else
				float2 disUV = TRANSFORM_TEX(i.uv0, _Dis_Emission_Tex) + float2(0, _Time.g * _Dis_Emission_Tex_PanV);
				float4 disTex = tex2D(_Dis_Emission_Tex, disUV);
#endif

#if SECOND_UV_ON
				float2 emissionUV = TRANSFORM_TEX(i.uv1, _Emission_Tex) + disTex.rg * _Dis_Emission_Tex_Power + float2(_Time.g * _Emission_Tex_PanV, 0);
				emissionUV = mul(emissionUV - 0.5f, float2x2(cosAngle, -sinAngle, sinAngle, cosAngle)) + 0.5f;
				float4 emissionTex = tex2D(_Emission_Tex, emissionUV);
#else
				float2 emissionUV = TRANSFORM_TEX(i.uv0, _Emission_Tex) + disTex.rg * _Dis_Emission_Tex_Power + float2(_Time.g * _Emission_Tex_PanV, 0);
				emissionUV = mul(emissionUV - 0.5f, float2x2(cosAngle, -sinAngle, sinAngle, cosAngle)) + 0.5f;
				float4 emissionTex = tex2D(_Emission_Tex, emissionUV);
#endif				
				float3 emissive = maskEmission * _Emission_Tex_Instensity * _Emission_Tex_Color.rgb * emissionTex.rgb;

				fixed3 finalColor = (mainColor.rgb * diff + spec) * _LightColor0 + mainColor.rgb * _CustomAmbient + reflcol.rgb * _ReflectColor.rgb + emissive;

				return fixed4(finalColor, 1.0f);
      
            } 
            ENDCG 
        } 
    }  
	FallBack "lg/Effect_DiffusePanU"
}