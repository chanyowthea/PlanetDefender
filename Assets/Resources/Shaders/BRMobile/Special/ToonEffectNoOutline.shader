// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BRMobile/Special/ToonEffectNoOutline" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Main Color",color) = (1,1,1,1)//物体的颜色
		//_Outline("Thick of Outline",range(0,0.1)) = 0.02//挤出描边的粗细
		_Factor("Factor",range(0,1)) = 0.5//挤出多远
		_ToonEffect("Toon Effect",range(0,1)) = 0.5//卡通化程度（二次元与三次元的交界线）
		_Steps("Steps of toon",range(0,9)) = 3//色阶层数
			_RimPower("RimPower",range(0,1)) = 0.5//挤出多远
			_ToonRimStep("ToonRimStep",range(0,9)) = 3//挤出多远
	}
		SubShader{
	pass {//平行光的的pass渲染
		Tags{ "LightMode" = "ForwardBase" }
			Cull Back
			ZWrite On
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

			float4 _LightColor0;
		float4 _Color;
		float _Steps;
		float _ToonEffect;
		sampler2D _MainTex;
		half4 _MainTex_ST;

		float _RimPower;

		float _ToonRimStep;

		struct v2f {
			float4 pos:SV_POSITION;
			float2 uv : TEXCOORD0;
			float3 lightDir:TEXCOORD1;
			float3 viewDir:TEXCOORD2;
			float3 normal:TEXCOORD3;
			UNITY_FOG_COORDS(1)
		};

		v2f vert(appdata_full v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);//切换到世界坐标
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.normal = v.normal;
			o.lightDir = ObjSpaceLightDir(v.vertex);
			o.viewDir = ObjSpaceViewDir(v.vertex);
			UNITY_TRANSFER_FOG(o, o.vertex);
			return o;
		}
		float4 frag(v2f i) :COLOR
		{
			half4 mc = tex2D(_MainTex, i.uv);
			float4 c = 1;
			float3 N = normalize(i.normal);
			float3 viewDir = normalize(i.viewDir);
			float3 lightDir = normalize(i.lightDir);
			float diff = max(0,dot(N,i.lightDir));
			diff = (diff + 1) / 2;
			diff = smoothstep(0,1,diff);
			float toon = floor(diff*_Steps) / _Steps;
			diff = lerp(diff,toon,_ToonEffect);
			float rim = 1.0 - saturate(dot(N, normalize(viewDir)));//求出正常的边缘光程度
			rim = rim + 1;//使之加亮
			rim = pow(rim, _RimPower);//外部变量_RimPower控制边缘光亮度大小
			float toonRim = floor(rim * _ToonRimStep) / _ToonRimStep;//再对边缘光进行离散化
			rim = lerp(rim, toonRim, _ToonEffect);//调节卡通与现实的比重
			c = _Color * (diff)* rim * mc * 2;//进行最终颜色混合
			UNITY_APPLY_FOG(i.fogCoord, c);
			return c;
		}
			ENDCG
	}//
		/*pass {//处理光照前的pass渲染
			Tags{ "LightMode" = "Always" }
				Cull Front
				ZTest Less
				CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
				float _Outline;
			float _Factor;
			sampler2D _MainTex;
			half4 _MainTex_ST;
			float4 _Color;
			struct v2f {
				float4 pos:SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata_full v) {
				v2f o;
				o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);

				half4 projSpacePos = mul(UNITY_MATRIX_MVP, v.vertex);
				half4 projSpaceNormal = normalize(mul(UNITY_MATRIX_MVP, half4(v.normal, 0)));
				half4 scaledNormal = _Outline * _Factor * projSpaceNormal; // * projSpacePos.w;

				scaledNormal.z += 0.001;
				o.pos = projSpacePos + scaledNormal;
				return o;
			}
			float4 frag(v2f i) :COLOR
			{
				float4 diffuseMapColor = tex2D(_MainTex, i.uv);

				float maxChan = max(max(diffuseMapColor.r, diffuseMapColor.g), diffuseMapColor.b);
				float4 newMapColor = diffuseMapColor;

				maxChan -= (1.0 / 255.0);
				float3 lerpVals = saturate((newMapColor.rgb - float3(maxChan, maxChan, maxChan)) * 255.0);
				newMapColor.rgb = lerp(newMapColor.rgb, newMapColor.rgb, lerpVals);

				return float4(0.8 * newMapColor.rgb * diffuseMapColor.rgb, diffuseMapColor.a) * _Color;
			}
				ENDCG
		}//end of pass*/
	/*pass {//附加点光源的pass渲染
		Tags{ "LightMode" = "ForwardAdd" }
			Blend One One
			Cull Back
			ZWrite Off
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

			float4 _LightColor0;
		float4 _Color;
		float _Steps;
		float _ToonEffect;

		struct v2f {
			float4 pos:SV_POSITION;
			float3 lightDir:TEXCOORD0;
			float3 viewDir:TEXCOORD1;
			float3 normal:TEXCOORD2;
		};

		v2f vert(appdata_full v) {
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
			o.normal = v.normal;
			o.viewDir = ObjSpaceViewDir(v.vertex);
			o.lightDir = _WorldSpaceLightPos0 - v.vertex;

			return o;
		}
		float4 frag(v2f i) :COLOR
		{
			float4 c = 1;
			float3 N = normalize(i.normal);
			float3 viewDir = normalize(i.viewDir);
			float dist = length(i.lightDir);//求出距离光源的距离
			float3 lightDir = normalize(i.lightDir);
			float diff = max(0,dot(N,i.lightDir));
			diff = (diff + 1) / 2;
			diff = smoothstep(0,1,diff);
			float atten = 1 / (dist);//根据距光源的距离求出衰减
			float toon = floor(diff*atten*_Steps) / _Steps;
			diff = lerp(diff,toon,_ToonEffect);

			half3 h = normalize(lightDir + viewDir);//求出半角向量
			float nh = max(0, dot(N, h));
			float spec = pow(nh, 32.0);//求出高光强度
			float toonSpec = floor(spec*atten * 2) / 2;//把高光也离散化
			spec = lerp(spec,toonSpec,_ToonEffect);//调节卡通与现实高光的比重


			c = _Color*_LightColor0*(diff + spec);//求出最终颜色
			return c;
		}
			ENDCG
	}//*/
	}
}