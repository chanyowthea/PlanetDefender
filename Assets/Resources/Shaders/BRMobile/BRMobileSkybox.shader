// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BRMobile/Skybox"{

Properties {
	_FrontTex ("Front (+Z)", 2D) = "white" {}
	_BackTex ("Back (-Z)", 2D) = "white" {}
	_LeftTex ("Left (+X)", 2D) = "white" {}
	_RightTex ("Right (-X)", 2D) = "white" {}
	_UpTex ("Up (+Y)", 2D) = "white" {}
	_DownTex ("Down (-Y)", 2D) = "white" {}
}

SubShader {
	Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
	Cull Off ZWrite Off 

	CGINCLUDE  
    #include "UnityCG.cginc"  
  
    struct appdata_t {  
        float4 vertex : POSITION;  
        float2 texcoord : TEXCOORD0;  
    };  
    struct v2f {  
        float4 vertex : SV_POSITION;  
        float2 texcoord : TEXCOORD0;  
    };  

    v2f vert (appdata_t v)  
    {  
        v2f o;  
        o.vertex = UnityObjectToClipPos(v.vertex);  
        o.texcoord = v.texcoord;  
        return o;  
    }  

    half4 skybox_frag (v2f i, sampler2D smp)  
    {  
        half4 tex = tex2D (smp, i.texcoord);  
        return half4(tex.rgb, 1);
    }  
    ENDCG  
      
    Pass {  
        CGPROGRAM  
        #pragma vertex vert  
        #pragma fragment frag  
        sampler2D _FrontTex;  
        half4 frag (v2f i) : SV_Target { return skybox_frag(i,_FrontTex); }  
        ENDCG   
    }  
    Pass{  
        CGPROGRAM  
        #pragma vertex vert  
        #pragma fragment frag  
        sampler2D _BackTex;  
        half4 frag (v2f i) : SV_Target { return skybox_frag(i,_BackTex); }  
        ENDCG   
    }  
    Pass{  
        CGPROGRAM  
        #pragma vertex vert  
        #pragma fragment frag  
        sampler2D _LeftTex;  
        half4 frag (v2f i) : SV_Target { return skybox_frag(i,_LeftTex); }  
        ENDCG  
    }  
    Pass{  
        CGPROGRAM  
        #pragma vertex vert  
        #pragma fragment frag  
        sampler2D _RightTex;  
        half4 frag (v2f i) : SV_Target { return skybox_frag(i,_RightTex); }  
        ENDCG  
    }     
    Pass{  
        CGPROGRAM  
        #pragma vertex vert  
        #pragma fragment frag  
        sampler2D _UpTex;  
        half4 frag (v2f i) : SV_Target { return skybox_frag(i,_UpTex); }  
        ENDCG  
    }     
    Pass{  
        CGPROGRAM  
        #pragma vertex vert  
        #pragma fragment frag  
        sampler2D _DownTex;  
        half4 frag (v2f i) : SV_Target { return skybox_frag(i,_DownTex); }  
        ENDCG  
    }  
}
}