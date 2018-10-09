// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:True,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:1,qpre:3,rntp:2,fgom:True,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0.5882353,fgcg:0.8977687,fgcb:1,fgca:1,fgde:0.01,fgrn:20,fgrf:30,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:5720,x:32725,y:32718,varname:node_5720,prsc:2|emission-6136-OUT;n:type:ShaderForge.SFN_Tex2d,id:4431,x:31573,y:32710,ptovrint:False,ptlb:Tex01,ptin:_Tex01,varname:node_4431,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:2,isnm:False|UVIN-2921-UVOUT;n:type:ShaderForge.SFN_Add,id:6224,x:29866,y:32808,varname:node_6224,prsc:2|A-4573-OUT,B-5483-UVOUT;n:type:ShaderForge.SFN_Panner,id:9819,x:30272,y:32782,varname:node_9819,prsc:2,spu:0,spv:1|UVIN-6224-OUT,DIST-4562-OUT;n:type:ShaderForge.SFN_TexCoord,id:5483,x:29799,y:32622,varname:node_5483,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:7300,x:29259,y:32689,ptovrint:False,ptlb:DisTex01,ptin:_DisTex01,varname:_node_4431_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-197-UVOUT;n:type:ShaderForge.SFN_Multiply,id:4573,x:29482,y:32737,varname:node_4573,prsc:2|A-7300-RGB,B-2430-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2430,x:29259,y:32881,ptovrint:False,ptlb:DisTex01_Power,ptin:_DisTex01_Power,varname:node_2430,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_TexCoord,id:7901,x:28821,y:32480,varname:node_7901,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:197,x:29043,y:32638,varname:node_197,prsc:2,spu:0,spv:1|UVIN-7901-UVOUT,DIST-77-OUT;n:type:ShaderForge.SFN_Time,id:1988,x:29627,y:33142,varname:node_1988,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4562,x:29840,y:33060,varname:node_4562,prsc:2|A-1988-T,B-8376-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8376,x:29644,y:33290,ptovrint:False,ptlb:Tex01_PanV,ptin:_Tex01_PanV,varname:node_8376,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Time,id:4964,x:28863,y:32795,varname:node_4964,prsc:2;n:type:ShaderForge.SFN_Multiply,id:77,x:29071,y:32837,varname:node_77,prsc:2|A-4964-T,B-733-OUT;n:type:ShaderForge.SFN_ValueProperty,id:733,x:28863,y:32992,ptovrint:False,ptlb:DisTex01_PanV,ptin:_DisTex01_PanV,varname:_PanV_Tex02,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_TexCoord,id:5507,x:29972,y:33860,varname:node_5507,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:5732,x:31478,y:33387,ptovrint:False,ptlb:Tex02,ptin:_Tex02,varname:_Tex02,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-1670-UVOUT;n:type:ShaderForge.SFN_Add,id:1968,x:30557,y:33808,varname:node_1968,prsc:2|A-4790-OUT,B-5507-UVOUT;n:type:ShaderForge.SFN_Panner,id:5310,x:30762,y:33921,varname:node_5310,prsc:2,spu:0,spv:1|UVIN-1968-OUT,DIST-2522-OUT;n:type:ShaderForge.SFN_Tex2d,id:1705,x:29714,y:33534,ptovrint:False,ptlb:DisTex02,ptin:_DisTex02,varname:_DisTex02,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6181-UVOUT;n:type:ShaderForge.SFN_Multiply,id:4790,x:29947,y:33618,varname:node_4790,prsc:2|A-1705-RGB,B-9026-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9026,x:29714,y:33767,ptovrint:False,ptlb:DisTex02_Power,ptin:_DisTex02_Power,varname:_Power_DisTex02,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_TexCoord,id:6296,x:29352,y:33570,varname:node_6296,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:6181,x:29528,y:33570,varname:node_6181,prsc:2,spu:0,spv:1|UVIN-6296-UVOUT,DIST-560-OUT;n:type:ShaderForge.SFN_Time,id:1802,x:29729,y:34223,varname:node_1802,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2522,x:29942,y:34141,varname:node_2522,prsc:2|A-1802-T,B-6092-OUT;n:type:ShaderForge.SFN_Time,id:9992,x:29139,y:33806,varname:node_9992,prsc:2;n:type:ShaderForge.SFN_Multiply,id:560,x:29352,y:33806,varname:node_560,prsc:2|A-9992-T,B-2300-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2300,x:29139,y:33978,ptovrint:False,ptlb:DisTex02_PanV,ptin:_DisTex02_PanV,varname:_PanV_DisTex03,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Add,id:2414,x:32118,y:32828,varname:node_2414,prsc:2|A-3416-OUT,B-2417-OUT;n:type:ShaderForge.SFN_Multiply,id:1794,x:32321,y:32673,varname:node_1794,prsc:2|A-7909-OUT,B-2414-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7909,x:32057,y:32616,ptovrint:False,ptlb:InstensityA,ptin:_InstensityA,varname:node_7909,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_VertexColor,id:1191,x:32249,y:33170,varname:node_1191,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6136,x:32477,y:32805,varname:node_6136,prsc:2|A-5803-OUT,B-1191-RGB,C-1191-A;n:type:ShaderForge.SFN_Multiply,id:5599,x:31812,y:32560,varname:node_5599,prsc:2|A-8196-RGB,B-4431-RGB;n:type:ShaderForge.SFN_Multiply,id:1303,x:31765,y:33412,varname:node_1303,prsc:2|A-5732-RGB,B-6233-RGB;n:type:ShaderForge.SFN_ValueProperty,id:6092,x:29729,y:34402,ptovrint:False,ptlb:Tex02_PanV,ptin:_Tex02_PanV,varname:node_6092,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Color,id:8196,x:31559,y:32507,ptovrint:False,ptlb:Color_Tex01,ptin:_Color_Tex01,varname:node_8196,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Color,id:6233,x:31714,y:33672,ptovrint:False,ptlb:Color_Tex02,ptin:_Color_Tex02,varname:node_6233,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:4833,x:32166,y:33747,ptovrint:False,ptlb:Mask_Tex,ptin:_Mask_Tex,varname:node_4833,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5803,x:32421,y:33381,varname:node_5803,prsc:2|A-1794-OUT,B-9345-OUT;n:type:ShaderForge.SFN_Rotator,id:1670,x:31441,y:33967,varname:node_1670,prsc:2|UVIN-8546-OUT,ANG-35-OUT;n:type:ShaderForge.SFN_Pi,id:9929,x:30951,y:34101,varname:node_9929,prsc:2;n:type:ShaderForge.SFN_Multiply,id:35,x:31100,y:34116,varname:node_35,prsc:2|A-9929-OUT,B-1751-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1751,x:30951,y:34224,ptovrint:False,ptlb:Tex02_UVAngle,ptin:_Tex02_UVAngle,varname:node_1751,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Rotator,id:2921,x:31290,y:32655,varname:node_2921,prsc:2|UVIN-317-OUT,ANG-8874-OUT;n:type:ShaderForge.SFN_Pi,id:8926,x:30993,y:32816,varname:node_8926,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8874,x:31155,y:32831,varname:node_8874,prsc:2|A-8926-OUT,B-7045-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7045,x:30993,y:32936,ptovrint:False,ptlb:Tex01_UVAngle,ptin:_Tex01_UVAngle,varname:_Tex02_UVAngle_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Panner,id:1080,x:30267,y:32450,varname:node_1080,prsc:2,spu:1,spv:0|UVIN-484-OUT,DIST-4562-OUT;n:type:ShaderForge.SFN_Add,id:484,x:30010,y:32385,varname:node_484,prsc:2|A-5483-UVOUT,B-4573-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:317,x:30591,y:32602,ptovrint:False,ptlb:Tex01_U/Vpan,ptin:_Tex01_UVpan,varname:node_317,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-1080-UVOUT,B-9819-UVOUT;n:type:ShaderForge.SFN_Panner,id:4519,x:30775,y:33723,varname:node_4519,prsc:2,spu:1,spv:0|UVIN-9372-OUT,DIST-2522-OUT;n:type:ShaderForge.SFN_Add,id:9372,x:30557,y:33596,varname:node_9372,prsc:2|A-4790-OUT,B-5507-UVOUT;n:type:ShaderForge.SFN_SwitchProperty,id:8546,x:31098,y:33617,ptovrint:False,ptlb:Tex02_U/Vpan,ptin:_Tex02_UVpan,varname:node_8546,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-4519-UVOUT,B-5310-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:6006,x:32214,y:33974,ptovrint:False,ptlb:AlphaInstensityA,ptin:_AlphaInstensityA,varname:_InstensityA_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:795,x:32596,y:33828,varname:node_795,prsc:2|A-6006-OUT,B-4833-R;n:type:ShaderForge.SFN_Multiply,id:9345,x:32893,y:33893,varname:node_9345,prsc:2|A-795-OUT,B-3012-OUT;n:type:ShaderForge.SFN_Multiply,id:7120,x:32012,y:33418,varname:node_7120,prsc:2|A-1303-OUT,B-6233-A;n:type:ShaderForge.SFN_Add,id:873,x:31652,y:34131,varname:node_873,prsc:2|A-5732-R,B-5732-G,C-5732-B;n:type:ShaderForge.SFN_SwitchProperty,id:7203,x:31946,y:34327,ptovrint:False,ptlb:Tex02_RGB/Alpha,ptin:_Tex02_RGBAlpha,varname:node_7203,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-873-OUT,B-5732-A;n:type:ShaderForge.SFN_Add,id:3012,x:32410,y:34317,varname:node_3012,prsc:2|A-9636-OUT,B-7203-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:9636,x:32183,y:34126,ptovrint:False,ptlb:Tex01_RGB/Alpha,ptin:_Tex01_RGBAlpha,varname:_Tex02_RGBAlpha_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-9868-OUT,B-4431-A;n:type:ShaderForge.SFN_Add,id:9868,x:31968,y:33920,varname:node_9868,prsc:2|A-4431-R,B-4431-G,C-4431-B;n:type:ShaderForge.SFN_Multiply,id:3416,x:31886,y:32312,varname:node_3416,prsc:2|A-5042-OUT,B-5599-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8975,x:31860,y:33131,ptovrint:False,ptlb:Tex02_Instensity,ptin:_Tex02_Instensity,varname:_Tex01_Instensity_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:2417,x:32086,y:33081,varname:node_2417,prsc:2|A-8975-OUT,B-7120-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5042,x:31597,y:32269,ptovrint:False,ptlb:Tex01_Instensity,ptin:_Tex01_Instensity,varname:node_5042,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:8795,x:31816,y:32512,ptovrint:False,ptlb:Tex01_Instensity_copy,ptin:_Tex01_Instensity_copy,varname:_Tex01_Instensity_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:7909-4431-5042-8196-9636-7045-317-8376-7300-733-2430-5732-8975-6233-7203-1751-8546-6092-1705-9026-2300-4833-6006;pass:END;sub:END;*/

Shader "lg/Effect_2Tex_Dis_Additive" {
    Properties {
        _InstensityA ("InstensityA", Float ) = 1
        _Tex01 ("Tex01", 2D) = "black" {}
        _Tex01_Instensity ("Tex01_Instensity", Float ) = 1
        _Color_Tex01 ("Color_Tex01", Color) = (0.5,0.5,0.5,1)
        [MaterialToggle] _Tex01_RGBAlpha ("Tex01_RGB/Alpha", Float ) = 0
        _Tex01_UVAngle ("Tex01_UVAngle", Float ) = 0
        [MaterialToggle] _Tex01_UVpan ("Tex01_U/Vpan", Float ) = 0
        _Tex01_PanV ("Tex01_PanV", Float ) = 0
        _DisTex01 ("DisTex01", 2D) = "white" {}
        _DisTex01_PanV ("DisTex01_PanV", Float ) = 0
        _DisTex01_Power ("DisTex01_Power", Float ) = 0
        _Tex02 ("Tex02", 2D) = "white" {}
        _Tex02_Instensity ("Tex02_Instensity", Float ) = 1
        _Color_Tex02 ("Color_Tex02", Color) = (0.5,0.5,0.5,1)
        [MaterialToggle] _Tex02_RGBAlpha ("Tex02_RGB/Alpha", Float ) = 0
        _Tex02_UVAngle ("Tex02_UVAngle", Float ) = 0
        [MaterialToggle] _Tex02_UVpan ("Tex02_U/Vpan", Float ) = 0
        _Tex02_PanV ("Tex02_PanV", Float ) = 0
        _DisTex02 ("DisTex02", 2D) = "white" {}
        _DisTex02_Power ("DisTex02_Power", Float ) = 0
        _DisTex02_PanV ("DisTex02_PanV", Float ) = 0
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
            Offset -1, -1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 2.0
			#pragma keepalpha
            uniform sampler2D _Tex01; uniform float4 _Tex01_ST;
            uniform sampler2D _DisTex01; uniform float4 _DisTex01_ST;
            uniform float _DisTex01_Power;
            uniform float _Tex01_PanV;
            uniform float _DisTex01_PanV;
            uniform sampler2D _Tex02; uniform float4 _Tex02_ST;
            uniform sampler2D _DisTex02; uniform float4 _DisTex02_ST;
            uniform float _DisTex02_Power;
            uniform float _DisTex02_PanV;
            uniform float _InstensityA;
            uniform float _Tex02_PanV;
            uniform float4 _Color_Tex01;
            uniform float4 _Color_Tex02;
            uniform sampler2D _Mask_Tex; uniform float4 _Mask_Tex_ST;
            uniform float _Tex02_UVAngle;
            uniform float _Tex01_UVAngle;
            uniform fixed _Tex01_UVpan;
            uniform fixed _Tex02_UVpan;
            uniform float _AlphaInstensityA;
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
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
////// Lighting:
////// Emissive:
				float2 angles = 3.141592654 * float2(_Tex01_UVAngle, _Tex02_UVAngle);
				float2 sinAngles = sin(angles);
				float2 cosAngles = cos(angles);

				float2 pans = float2(_Tex01_PanV, _Tex02_PanV) * _Time.g;
				float4 pansV = i.uv0.xyxy + float4(0, _DisTex01_PanV, 0, _DisTex02_PanV) * _Time.g;

                float4 _DisTex01_var = tex2D(_DisTex01,TRANSFORM_TEX(pansV.xy, _DisTex01));
				float4 _DisTex02_var = tex2D(_DisTex02,TRANSFORM_TEX(pansV.zw, _DisTex02));
				float4 _DisTex0102 = float4(_DisTex01_var.rg * _DisTex01_Power, _DisTex02_var.rg * _DisTex02_Power);
				float4 newUV01 = i.uv0.xyxy + _DisTex0102.xyxy + float4(pans.x, 0, 0, pans.x);
				float4 newUV02 = i.uv0.xyxy + _DisTex0102.zwzw + float4(pans.y, 0, 0, pans.y);

				float2 newUV01_final = lerp(newUV01.xy, newUV01.zw, _Tex01_UVpan) - 0.5f;
				float2 newUV02_final = lerp(newUV02.xy, newUV02.zw, _Tex02_UVpan) - 0.5f;

                float2 node_2921 = mul(newUV01_final, float2x2( cosAngles.x, -sinAngles.x, sinAngles.x, cosAngles.x)) + 0.5f;
                float4 _Tex01_var = tex2D(_Tex01,TRANSFORM_TEX(node_2921, _Tex01));

                float2 node_1670 = mul(newUV02_final, float2x2( cosAngles.y, -sinAngles.y, sinAngles.y, cosAngles.y)) + 0.5f;
                float4 _Tex02_var = tex2D(_Tex02,TRANSFORM_TEX(node_1670, _Tex02));

                float4 _Mask_Tex_var = tex2D(_Mask_Tex,TRANSFORM_TEX(i.uv0, _Mask_Tex));

                float3 emissive = (
					(_InstensityA * (_Tex01_Instensity * _Color_Tex01.rgb * _Tex01_var.rgb + _Tex02_Instensity * _Tex02_var.rgb * _Color_Tex02.rgb * _Color_Tex02.a)) * 
					((_AlphaInstensityA*_Mask_Tex_var.r) * (lerp(dot(_Tex01_var, 1), _Tex01_var.a, _Tex01_RGBAlpha) + lerp(dot(_Tex02_var, 1), _Tex02_var.a, _Tex02_RGBAlpha )))
					) * i.vertexColor.rgb * i.vertexColor.a;

				return fixed4(emissive,0.5f);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
