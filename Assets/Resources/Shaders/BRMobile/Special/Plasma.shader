// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.6691177,fgcg:0.5510381,fgcb:0.5510381,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4060,x:32799,y:32632,varname:node_4060,prsc:2|emission-4762-OUT,alpha-8723-OUT,clip-1181-OUT;n:type:ShaderForge.SFN_Tex2d,id:8145,x:31858,y:32856,ptovrint:False,ptlb:Noise Map 01,ptin:_NoiseMap01,varname:node_8145,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-8126-OUT;n:type:ShaderForge.SFN_Panner,id:2348,x:30823,y:32540,varname:node_2348,prsc:2,spu:0,spv:0.02|UVIN-5786-UVOUT;n:type:ShaderForge.SFN_Lerp,id:1915,x:31452,y:32350,varname:node_1915,prsc:2|A-5786-UVOUT,B-9484-R,T-438-OUT;n:type:ShaderForge.SFN_Slider,id:438,x:31174,y:32214,ptovrint:False,ptlb:Distorsion,ptin:_Distorsion,varname:_WavesSize_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.65,max:1;n:type:ShaderForge.SFN_Add,id:8126,x:31669,y:32569,varname:node_8126,prsc:2|A-1915-OUT,B-2704-OUT;n:type:ShaderForge.SFN_Panner,id:4963,x:30823,y:32707,varname:node_4963,prsc:2,spu:0.02,spv:0|UVIN-5786-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:5786,x:30515,y:32434,varname:node_5786,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:9484,x:31257,y:32461,ptovrint:False,ptlb:Noise Map 02,ptin:_NoiseMap02,varname:_Diff_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5215-OUT;n:type:ShaderForge.SFN_Vector1,id:7635,x:31158,y:32841,varname:node_7635,prsc:2,v1:0;n:type:ShaderForge.SFN_Slider,id:7551,x:31799,y:33272,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_7551,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:-1,max:1;n:type:ShaderForge.SFN_Multiply,id:8723,x:32440,y:32978,varname:node_8723,prsc:2|A-8145-G,B-4616-OUT;n:type:ShaderForge.SFN_OneMinus,id:4616,x:32238,y:33112,varname:node_4616,prsc:2|IN-7551-OUT;n:type:ShaderForge.SFN_Lerp,id:5215,x:31102,y:32682,varname:node_5215,prsc:2|A-2348-UVOUT,B-4963-UVOUT,T-6219-OUT;n:type:ShaderForge.SFN_Slider,id:6219,x:30707,y:32969,ptovrint:False,ptlb:Direction Speed,ptin:_DirectionSpeed,varname:node_6219,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Lerp,id:2704,x:31411,y:32698,varname:node_2704,prsc:2|A-5786-UVOUT,B-5215-OUT,T-7635-OUT;n:type:ShaderForge.SFN_Multiply,id:4762,x:32163,y:32832,varname:node_4762,prsc:2|A-8145-RGB,B-8180-OUT,C-8571-RGB;n:type:ShaderForge.SFN_Slider,id:8180,x:31441,y:33245,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_8180,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:7.638231,max:10;n:type:ShaderForge.SFN_Color,id:8571,x:31911,y:32569,ptovrint:False,ptlb:Emission Color,ptin:_EmissionColor,varname:node_8571,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.9172413,c3:1,c4:1;n:type:ShaderForge.SFN_Slider,id:6698,x:31768,y:33383,ptovrint:False,ptlb:Opacity Cutout,ptin:_OpacityCutout,varname:_OpacityCutout_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-2,cur:-2,max:2;n:type:ShaderForge.SFN_OneMinus,id:5223,x:32254,y:33364,varname:node_5223,prsc:2|IN-6698-OUT;n:type:ShaderForge.SFN_Multiply,id:1181,x:32513,y:33227,varname:node_1181,prsc:2|A-8145-G,B-5223-OUT;proporder:8145-9484-8571-8180-438-6219-7551-6698;pass:END;sub:END;*/

Shader "BRMobile/Special/Plasma" {
    Properties {
        _NoiseMap01 ("Noise Map 01", 2D) = "white" {}
        _GlowMap01 ("Glow Map 01", 2D) = "white" {}
        _GlowMap01_Flow ("Glow Speed 01", Range(0, 0.01)) = 0.005
        _GlowMap02 ("Glow Map 02", 2D) = "white" {}
        _GlowMap02_Flow ("Glow Speed 02", Range(0, 0.1)) = 0.06
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _EmissionColor ("Emission Color", Color) = (0,0.9172413,1,1)
        _Opacity ("Opacity", Range(-1, 1)) = -1
        _Distorsion ("Distorsion", Range(0, 1)) = 0
        [ToggleOff]_ShowMask("Show Mask", float) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Overlay"
            "RenderType"="Transparent"
        }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off Lighting Off 
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_custom{
                float4 vertex : POSITION;
                float3 color : COLOR;
                float2 texcoord0 : TEXCOORD0;
            };
            
            struct v2f
            {
                fixed4 vertex : SV_POSITION;
                float3 col : COLOR;
                float2 uv0 : TEXCOORD0;
                fixed4 screenPos: TEXCOORD1;
            };

            uniform float4 _MainColor;
            uniform float4 _EmissionColor;
            uniform sampler2D _NoiseMap01; uniform float4 _NoiseMap01_ST;
            uniform sampler2D _GlowMap02; uniform float4 _GlowMap02_ST;
            uniform sampler2D _GlowMap01; uniform float4 _GlowMap01_ST;
            uniform sampler2D _CameraDepthTexture;
            uniform float _Opacity;
            uniform float _ShowMask;
            uniform float _GlowMap01_Flow;
            uniform float _GlowMap02_Flow;
            uniform float _Distorsion;

            v2f vert (appdata_custom v)
            {
                v2f o;
                o.uv0 = v.texcoord0;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.col = v.color;
                //o.screenPos.z = -UnityObjectToViewPos(v.vertex.xyz).z;
                UNITY_TRANSFER_DEPTH(o.screenPos.z);// eye space depth of the vertex 
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                //back intersection
                //fixed zBuffer = LinearEyeDepth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPos)).r);
                //_MainColor += saturate(1.5 - abs(zBuffer - i.screenPos.z)) * 0.6;

                float2 _Distort01_uv = i.uv0 + float2(_GlowMap01_Flow * _Time.g, 0);
                float4 _Distort01_var = tex2D(_GlowMap01, TRANSFORM_TEX(_Distort01_uv, _GlowMap01));

                float2 _GlowMap02_uv = i.uv0 + float2(0, _GlowMap02_Flow * _Time.g);
                float4 _GlowMap02_var = tex2D(_GlowMap02, TRANSFORM_TEX(_GlowMap02_uv, _GlowMap02));

                float2 _NoiseMap01_uv = i.uv0 + lerp(0, float2(_Distort01_var.a, _Distort01_var.a), _Distorsion) + float2(0, 0.002 * _Time.g);
                float4 _NoiseMap01_var = tex2D(_NoiseMap01,TRANSFORM_TEX(_NoiseMap01_uv, _NoiseMap01));

                float alpha = saturate(_NoiseMap01_var.g * 1.5 * _GlowMap02_var - 0.5);
                float3 finalColor = _NoiseMap01_var.rgb + (1.0 - _NoiseMap01_var.g) * _EmissionColor.rgb;
                fixed4 finalRGBA = fixed4(_MainColor.rgb + finalColor * alpha, (_MainColor.a + alpha) * (1.0 - _Opacity) * i.col.r);

                //return _NoiseMap01_var;
                return _ShowMask * _GlowMap02_var + (1 - _ShowMask) * finalRGBA;
            }
            ENDCG
        }
    }
}
