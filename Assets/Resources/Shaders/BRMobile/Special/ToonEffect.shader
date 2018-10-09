// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "BRMobile/Special/ToonEffect" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Main Color",color) = (1,1,1,1)//物体的颜色
		_Outline("Thick of Outline",range(0,0.1)) = 0.02//挤出描边的粗细
		_Factor("Factor",range(0,1)) = 0.5//挤出多远
		_ToonEffect("Toon Effect",range(0,1)) = 0.5//卡通化程度（二次元与三次元的交界线）
		_Steps("Steps of toon",range(0,9)) = 3//色阶层数
		_RimPower("RimPower",range(0,1)) = 0.5//挤出多远
		_ToonRimStep("ToonRimStep",range(0,9)) = 3//挤出多远
		_ShadowHeight("shadow height", range(0,0.2)) = 0.05	//阴影高度
		_ShadowAlpha("shadow alpha", range(0,1)) = 0.3	//阴影alpha
	}
	SubShader{
		Pass {//平行光的的pass渲染
			Tags{ "LightMode" = "ForwardBase" }
			Cull Back
			ZWrite On
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#pragma multi_compile_fog 

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
					UNITY_FOG_COORDS(4)
				};

				v2f vert(appdata_full v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);//切换到世界坐标
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.normal = v.normal;
					o.lightDir = ObjSpaceLightDir(v.vertex);
					o.viewDir = ObjSpaceViewDir(v.vertex);
					UNITY_TRANSFER_FOG(o, o.pos);
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
					c = _Color * (diff)* rim * mc * _LightColor0;//进行最终颜色混合
					UNITY_APPLY_FOG(i.fogCoord, c);
					return c;
				}
			ENDCG
		}
		Pass {//处理光照前的pass渲染
			Tags{ "LightMode" = "Always" }
			Cull Front
			ZTest Less
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#pragma multi_compile_fog 
				float _Outline;
				float _Factor;
				sampler2D _MainTex;
				half4 _MainTex_ST;
				float4 _Color;
				struct v2f {
					float4 pos:SV_POSITION;
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
				};

				v2f vert(appdata_full v) {
					v2f o;
					o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);

					half4 projSpacePos = UnityObjectToClipPos(v.vertex);
					half4 projSpaceNormal = normalize(UnityObjectToClipPos(half4(v.normal, 0)));
					half4 scaledNormal = _Outline * _Factor * projSpaceNormal; // * projSpacePos.w;

					scaledNormal.z += 0.01;
					o.pos = projSpacePos + scaledNormal;
					UNITY_TRANSFER_FOG(o, o.pos);
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

					float4 c = float4(0.8 * newMapColor.rgb * diffuseMapColor.rgb, diffuseMapColor.a) * _Color;
					UNITY_APPLY_FOG(i.fogCoord, c);
					return c;
				}
			ENDCG
		}//end of pass
		/*Pass
		{
			Tags{ "RenderType" = "Transparent" "IgnoreProjector" = "True" "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha
			Stencil{
				Ref 0
				Comp Equal
				Pass IncrSat
				ZFail Keep
			}
			ZTest Less

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog 
				#include "UnityCG.cginc"
				struct v2f
				{
					float4 pos : SV_POSITION;
					UNITY_FOG_COORDS(0)
				};
				half _ShadowHeight;
				fixed _ShadowAlpha;

				v2f vert(float4 vertex:POSITION)
				{
					v2f o;
					float3 litDir = WorldSpaceLightDir(vertex);//世界空间主光照相对于当前物体的方向
														//litDir = mul(_World2Ground,float4(litDir,0)).xyz;//光源方向转换到接受阴影的平面空间
					litDir = normalize(litDir);	// 归一
					float4 vt = mul(unity_ObjectToWorld, vertex); //将当前物体转换到世界空间
															//				vt = mul(_World2Ground,vt);		// 将物体在世界空间的矩阵转换到地面空间
					vt.xz = vt.xz - (vt.y / litDir.y)*litDir.xz;// 用三角形相似计算沿光源方向投射后的XZ
					vt.y = _ShadowHeight;// 使阴影保持在接受平面上
											//				vt = mul(_Ground2World, vt); // 阴影顶点矩阵返回到世界空间
					vt = mul(unity_WorldToObject, vt); // 返回到物体的坐标

					o.pos = mul(UNITY_MATRIX_MVP, vt);//输出到裁剪空间

					UNITY_TRANSFER_FOG(o, o.pos);
					return o;
				}
				float4 frag(v2f i) : COLOR
				{
					//return smoothstep(0,1,i.atten / 2);
					float4 c = float4(0,0,0,_ShadowAlpha);
					UNITY_APPLY_FOG(i.fogCoord, c);
					return c;
				}
			ENDCG
		}*/
	}
}