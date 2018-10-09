// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
///*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5607843,fgcg:0.454902,fgcb:0.3803922,fgca:0.578,fgde:0.005,fgrn:30,fgrf:1500,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:-1,ofsu:-1,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32861,y:32736,varname:node_3138,prsc:2|emission-1833-OUT;n:type:ShaderForge.SFN_Tex2d,id:5346,x:32339,y:32878,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_5346,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6132-OUT;n:type:ShaderForge.SFN_Panner,id:1937,x:31357,y:33150,varname:node_1937,prsc:2,spu:0,spv:1.25|UVIN-5683-OUT;n:type:ShaderForge.SFN_Multiply,id:1833,x:32610,y:32916,varname:node_1833,prsc:2|A-5346-RGB,B-9524-RGB,C-273-OUT;n:type:ShaderForge.SFN_Slider,id:273,x:32495,y:33213,ptovrint:False,ptlb:Brightness,ptin:_Brightness,varname:node_273,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:3;n:type:ShaderForge.SFN_Multiply,id:5683,x:31878,y:32898,varname:node_5683,prsc:2|A-7521-OUT,B-8997-OUT;n:type:ShaderForge.SFN_Append,id:8997,x:31662,y:32898,varname:node_8997,prsc:2|A-8447-X,B-8447-Y;n:type:ShaderForge.SFN_Vector4Property,id:8447,x:31448,y:32911,ptovrint:False,ptlb:Params,ptin:_Params,varname:node_8447,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2,v2:5,v3:0,v4:0;n:type:ShaderForge.SFN_Matrix4x4,id:591,x:31789,y:32394,varname:node_591,prsc:2,m00:-0.77333,m01:-0.0285,m02:-0.63336,m03:0.01409,m10:0.44038,m11:0.69453,m12:-0.56895,m13:-0.07847,m20:-0.4561,m21:0.7189,m22:0.52455,m23:-1.49408,m30:0,m31:0,m32:0,m33:1;n:type:ShaderForge.SFN_MultiplyMatrix,id:7014,x:31980,y:32511,varname:node_7014,prsc:2|A-591-OUT,B-6418-OUT;n:type:ShaderForge.SFN_Append,id:6418,x:31699,y:32566,varname:node_6418,prsc:2|A-7690-XYZ,B-7690-W;n:type:ShaderForge.SFN_ComponentMask,id:7521,x:31941,y:32692,varname:node_7521,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7014-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:7690,x:31483,y:32494,varname:node_7690,prsc:2;n:type:ShaderForge.SFN_Fmod,id:2789,x:31748,y:33216,varname:node_2789,prsc:2|A-3508-G,B-8447-Z;n:type:ShaderForge.SFN_Color,id:9524,x:32287,y:33245,ptovrint:False,ptlb:Tint Color,ptin:_TintColor,varname:node_9524,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Clamp01,id:8764,x:31907,y:33216,varname:node_8764,prsc:2|IN-2789-OUT;n:type:ShaderForge.SFN_ComponentMask,id:3508,x:31515,y:33150,varname:node_3508,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-1937-UVOUT;n:type:ShaderForge.SFN_Append,id:6132,x:32132,y:33111,varname:node_6132,prsc:2|A-3508-R,B-8764-OUT;proporder:5346-273-8447-9524;pass:END;sub:END;*/

Shader "BRMobile/Special/PickupObject" {
    Properties {
        _Mask ("Mask", 2D) = "white" {}
        _Brightness ("Brightness", Range(0, 3)) = 1
        _Params ("Params", Vector) = (2,5,0,0)
        _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            Offset -1, -1
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 2.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _Brightness;
            uniform float4 _Params;
            uniform float4 _TintColor;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_9977 = _Time + _TimeEditor;
                float4x4 node_591 = {
                    {-0.77333,-0.0285,-0.63336,0.01409},
                    {0.44038,0.69453,-0.56895,-0.07847},
                    {-0.4561,0.7189,0.52455,-1.49408},
                    {0,0,0,1}
                };
                float2 node_3508 = ((mul(node_591,float4(i.posWorld.rgb,i.posWorld.a)).rg*float2(_Params.r,_Params.g))+node_9977.g*float2(0,1.25)).rg;
                float2 node_6132 = float2(node_3508.r,saturate(node_3508.g - _Params.b * floor(node_3508.g/_Params.b)));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(node_6132, _Mask));
                float3 emissive = (_Mask_var.rgb*_TintColor.rgb*_Brightness);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
