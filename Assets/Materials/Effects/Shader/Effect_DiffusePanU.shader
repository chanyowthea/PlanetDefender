// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "lg/Effect_DiffusePanU"
{ 
    Properties  
    {          
        _MainTex ("Base_01 (RGB)", 2D) = "white" {} 
		_Emission_Tex("Emission_Tex", 2D) = "black" {}
		_Emission_Tex_Instensity("Emission_Tex_Instensity", Float) = 1
		_Emission_Tex_Color("Emission_Tex_Color", Color) = (0.5,0.5,0.5,1)
		_Emission_Tex_PanV("Emission_Tex_PanV", Float) = 0
		_Dis_Emission_Tex("Dis_Emission_Tex", 2D) = "white" {}
		_Dis_Emission_Tex_Power("Dis_Emission_Tex_Power", Float) = 0
		_Dis_Emission_Tex_PanV("Dis_Emission_Tex_PanV", Float) = 0
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
	}

    SubShader
    {
		Tags { "RenderType"="Opaque" }

        Pass  
        { 
            CGPROGRAM 
    
            #pragma vertex Vert 
  
            #pragma fragment Frag 
      
            #include "UnityCG.cginc" 
  
            sampler2D _MainTex; 
			fixed4 _MainTex_ST;

			uniform sampler2D _Emission_Tex; uniform float4 _Emission_Tex_ST;
			uniform sampler2D _Dis_Emission_Tex; uniform float4 _Dis_Emission_Tex_ST;
			uniform float _Dis_Emission_Tex_Power;
			uniform float _Emission_Tex_PanV;
			uniform float _Dis_Emission_Tex_PanV;
			uniform float4 _Emission_Tex_Color;
			uniform sampler2D _Mask_Tex;
			uniform float _Emission_Tex_Instensity;
			
						
			struct appdata_t 
			{
				fixed4 vertex : POSITION;
				fixed2 texcoord : TEXCOORD0;
				fixed2 texcoord1 : TEXCOORD1;
			};
            struct V2F 
            { 
                fixed4 pos:POSITION;
				fixed2 uv0 : TEXCOORD0;
				fixed2 uv1 : TEXCOORD1;          
            }; 
                
          
            V2F Vert(appdata_t v) 
            { 
                  
                V2F output; 
      
                output.pos = UnityObjectToClipPos(v.vertex); 
      
				//tiling && offset 
                output.uv0 = v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
				output.uv1 = v.texcoord;

                return output; 
            } 
              
          
            half4 Frag(V2F i):COLOR 
            { 
                fixed4 mainColor = tex2D(_MainTex,i.uv0);

				float4 _Mask_Tex_var = tex2D(_Mask_Tex, i.uv1);
				float node_4562 = _Time.g *_Emission_Tex_PanV;
				float2 node_197 = i.uv0 + float2(0, _Time.g * _Dis_Emission_Tex_PanV);
				float4 _Dis_Emission_Tex_var = tex2D(_Dis_Emission_Tex, TRANSFORM_TEX(node_197, _Dis_Emission_Tex));
				float2 node_4573 = _Dis_Emission_Tex_var.rg * _Dis_Emission_Tex_Power;
				float2 node_2921 = i.uv0 + node_4573 + float2(node_4562, 0);
				float4 _Emission_Tex_var = tex2D(_Emission_Tex, TRANSFORM_TEX(node_2921, _Emission_Tex));
				float3 emissive = _Mask_Tex_var.r * _Emission_Tex_Instensity * _Emission_Tex_Color.rgb * _Emission_Tex_var.rgb;

				fixed3 finalColor = mainColor.rgb + emissive;

				return fixed4(finalColor, 1.0f);
      
            } 
            ENDCG 
        } 
    }  
	FallBack "Mobile/VertexLit"
}