// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BRMobile/SplattedTerrainLightmap" 
{
    Properties 
    {
        _Color("Tint Color", color) = (1, 1, 1, 1)
        _MaskMap ("Mask Map (RGBA)", 2D) = "black" {}

        _Splat1Map ("Layer 1 (R)", 2D) = "white" {}
        _Splat1Map_uvScale("", float) = 1   

        _Splat2Map ("Layer 2 (G)", 2D) = "white" {}
        _Splat2Map_uvScale("", float) = 1   

        _Splat3Map ("Layer 3 (B)", 2D) = "white" {}
        _Splat3Map_uvScale("", float) = 1   

        _Splat4Map ("Layer 4 (A)", 2D) = "white" {}
        _Splat4Map_uvScale("", float) = 1   

        _SplatLightMap("Lightmap", 2D) = "white" {}
        _SplatLightMap_uvScale("", float) = 1

        //_ShadowIntense ("Shadow Intense", Range(0, 1)) = 0
    }

    SubShader 
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" "Queue" = "Transparent-2"}
        LOD 300
        Offset 0.15, 0.15

        Pass {

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_fog
        #pragma multi_compile_fwdbase
        #pragma multi_compile _ DISABLE_LIGHTMAP

        #include "AutoLight.cginc"
        #include "UnityCG.cginc"
        #include "UnityShadowLibrary.cginc"
        #include "UnityLightingCommon.cginc"
		#include "BRMobileCommon.cginc"

        struct v2f {
        float4 pos : SV_POSITION;
        float2 uv0 : TEXCOORD0;
        float2 uv1 : TEXCOORD1;
        UNITY_FOG_COORDS(2) 
      };

      struct a2v {
        float4 vertex : POSITION;
        float2 texcoord : TEXCOORD0;

      };

        fixed4 _Color;
        sampler2D _MaskMap;
        float4 _MaskMap_ST;

        // sampler2D unity_Lightmap;
        sampler2D _Splat1Map; 
        half _Splat1Map_uvScale;
        sampler2D _Splat2Map; 
        half _Splat2Map_uvScale;
        sampler2D _Splat3Map; 
        half _Splat3Map_uvScale;
        sampler2D _Splat4Map; 
        half _Splat4Map_uvScale;
        
#if !defined(DISABLE_LIGHTMAP)        
        sampler2D _SplatLightMap;
        half _SplatLightMap_uvScale;
#endif

      v2f vert(a2v i) {
        v2f o; 
        o.pos = UnityObjectToClipPos(i.vertex);
        o.uv0 = TRANSFORM_TEX(i.texcoord, _MaskMap);
        float3 worldPos = mul(unity_ObjectToWorld, i.vertex).xyz;
        o.uv1 = worldPos.xz * 0.025 - floor(_WorldSpaceCameraPos.xz * 0.025);
        UNITY_TRANSFER_FOG(o,o.pos);  

        return o;
      }

      inline half4 mixColor(float2 uv, half4 splat_control, half inverse)
      {
        fixed4 mainTex = splat_control.r * inverse * tex2D (_Splat1Map, uv * _Splat1Map_uvScale);
        mainTex += splat_control.g * inverse * tex2D (_Splat2Map, uv * _Splat2Map_uvScale);
        mainTex += splat_control.b * inverse * tex2D (_Splat3Map, uv * _Splat3Map_uvScale);
        mainTex += splat_control.a * inverse * tex2D (_Splat4Map, uv * _Splat4Map_uvScale);
        mainTex.rgb *= _Color.rgb;
        return mainTex;
      }

      half4 frag(v2f i) : COLOR {

        half4 albedo;
        half4 splat_control = tex2D (_MaskMap, i.uv0);
        half inverse = 1.0 / (splat_control.r + splat_control.g + splat_control.b + splat_control.a);
        albedo = mixColor(i.uv1, splat_control, inverse);
        half3 mainTex = albedo.rgb * 2.5f;

#if !defined(DISABLE_LIGHTMAP)        
        mainTex *= tex2D(_SplatLightMap, i.uv0 * _SplatLightMap_uvScale);
#endif 

		BRFOG(i.fogCoord, mainTex);

		fixed4 c =  fixed4(mainTex, 1.0);

        return c;
      }
        ENDCG
    }
    }

    SubShader 
    {
        Tags { "RenderType"="Opaque" "LightMode"="ForwardBase" "Queue" = "Transparent-2"}
        LOD 200
        Offset 0.15, 0.15

        Pass {

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_fwdbase
        #pragma multi_compile _ DISABLE_LIGHTMAP

        #include "UnityCG.cginc"
        #include "UnityLightingCommon.cginc"

        struct v2f {
        float4 pos : SV_POSITION;
        float2 uv0 : TEXCOORD0;
        float2 uv1 : TEXCOORD1;
      };

      struct a2v {
        float4 vertex : POSITION;
        float2 texcoord : TEXCOORD0;
      };

        fixed4 _Color;
        sampler2D _MaskMap;
        float4 _MaskMap_ST;

        sampler2D _Splat1Map; 
        half _Splat1Map_uvScale;
        sampler2D _Splat2Map; 
        half _Splat2Map_uvScale;
        sampler2D _Splat3Map; 
        half _Splat3Map_uvScale;
        sampler2D _Splat4Map; 
        half _Splat4Map_uvScale;

#if !defined(DISABLE_LIGHTMAP)        
        sampler2D _SplatLightMap;
        half _SplatLightMap_uvScale;
#endif        

      v2f vert(a2v i) {
        v2f o;
        o.pos = UnityObjectToClipPos(i.vertex);
        o.uv0 = TRANSFORM_TEX(i.texcoord, _MaskMap);
        float3 worldPos = mul(unity_ObjectToWorld, i.vertex).xyz;
        o.uv1 = worldPos.xz * 0.025 - floor(_WorldSpaceCameraPos.xz * 0.025);
        return o;
      }

      inline half4 mixColor(float2 uv, half4 splat_control)
      {
        fixed4 mainTex = splat_control.r * tex2D (_Splat1Map, uv * _Splat1Map_uvScale);
        mainTex += splat_control.g * tex2D (_Splat2Map, uv * _Splat2Map_uvScale);
        mainTex += splat_control.b * tex2D (_Splat3Map, uv * _Splat3Map_uvScale);
        mainTex += splat_control.a * tex2D (_Splat4Map, uv * _Splat4Map_uvScale);
        mainTex.rgb *= _Color.rgb;
        return mainTex;
      }

      half4 frag(v2f i) : COLOR {

        half4 albedo;
        half4 splat_control = tex2D (_MaskMap, i.uv0);
        albedo = mixColor(i.uv1, splat_control);
        half3 mainTex = albedo.rgb * 2.5f;

#if !defined(DISABLE_LIGHTMAP)        
        mainTex *= tex2D(_SplatLightMap, i.uv0 * _SplatLightMap_uvScale);
#endif        

        fixed4 c =  fixed4(mainTex, 1.0);
        return c;
      }
        ENDCG
    }
    } 

    FallBack Off
}
