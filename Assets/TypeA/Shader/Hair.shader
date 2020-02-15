// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TypeA/Hair"
{
	Properties
	{
		_Color("Main Color", Color) = (1, 1, 1, 1)
		_Shadow("Shadow", Color) = (0.0, 0.0, 0.0, 1)
		_Ambient("Ambient", Color) = (0.3, 0.3, 0.3, 1)
		_Diffuse("Diffuse", Color) = (0.5, 0.5, 0.5, 1)
		_Highlight("Highlight", Color) = (0.8, 0.8, 0.8, 1)

		_AmbientBorder("Ambient border", Range(0.01, 1)) = 0.3
		_AmbientBlur("Ambient blur", Range(0, 0.5)) = 0.1
		_HighlightPower("Highlight Power", Range(0, 1)) = 0.1
		_HighlightHardness("Highlight Hardness", Range(0, 1)) = 0.1

		_NU("NU", Range(0.001, 1)) = 0.1
		_NV("NV", Range(0.001, 1)) = 0.1
		_Rotation("Rotation", Vector) = (0 , 1 , 0)

		_RimIntensity("Rim Intensity", Range(0, 1)) = 0.5
		_RimPower("Rim Power", Range(0.5, 10.0)) = 3.0

		_EdgeColor("Edge Color", Color) = (0,0,0,1)
		_EdgeThickness("Edge Thickness", Range(0, 5)) = 1
		[Enum(not use,0,use,1)] _vc2edge("VertexColor to Edge", Float) = 0

		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }

		UsePass "TypeA/Toon01Edge/FORWARDBASE"
		UsePass "TypeA/Toon01Edge/FORWARDADD"

		//-------------------------------------------------------------------------------------
		// Toon01 Hair Forward Base
		Pass
		{
			Name "FORWARDBASE"
			Tags{ "LightMode" = "Forwardbase" }

			Cull Off
			ZWrite On

			CGPROGRAM
			#pragma target 3.0
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#pragma vertex vertForwardBaseHair
			#pragma fragment fragForwardBaseHair

			#include "TypeAFunction.cginc"


			struct VertexOutForwardBaseHair
			{
				float4	pos			: SV_POSITION;
				float3	lightDir	: TEXCOORD0;
				float3	viewDir		: TEXCOORD1;
				float3	normal		: TEXCOORD2;
				float3	tangent		: TEXCOORD3;
				float2	uv			: TEXCOORD4;
				SHADOW_COORDS(5)
				UNITY_FOG_COORDS(6)
			};

			VertexOutForwardBaseHair vertForwardBaseHair(appdata_tan v)
			{
				VertexOutForwardBaseHair o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.lightDir = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, ObjSpaceLightDir(v.vertex)));
				o.viewDir = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, ObjSpaceViewDir(v.vertex)));
				o.normal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
				o.tangent = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.tangent));
				o.uv = v.texcoord.xy;

				TRANSFER_SHADOW(o);
				UNITY_TRANSFER_FOG(o, o.pos);

				return o;
			}

			float4 fragForwardBaseHair(VertexOutForwardBaseHair i, fixed facing : VFACE) : SV_Target
			{
				half atten = SHADOW_ATTENUATION(i);

				float4 tex = tex2D(_MainTex, i.uv);
				half  gloss = tex.a;
				tex *= _Color;

				half4 tdiff = toondiffuse(i.normal, i.lightDir);
				half4 hspec = toonanisspecular(i.normal, i.tangent, i.lightDir, i.viewDir) * gloss;

				half rim = 1.0 - saturate(dot(i.normal, i.viewDir));
				half4 r_c = (_Highlight * pow(rim, _RimPower)) * _RimIntensity;

				half3 f = saturate((atten + _Shadow.rgb) * facing) + frac(clamp(facing, _Ambient.rgb, 1));
				half3 ac = tex.rgb * _LightColor0.rgb * tdiff.rgb + r_c.rgb + hspec.rgb;
				half4 c;
				c.rgb = UNITY_LIGHTMODEL_AMBIENT.rgb * tex.rgb;
				c.rgb += ac * f;
				c.a = tex.a + _LightColor0.a * atten;

				UNITY_APPLY_FOG(i.fogCoord, c);

				return c;
			}
			ENDCG
		}

		//-------------------------------------------------------------------------------------
		// Toon01 Hair Forward Add
		Pass
		{
			Name "FORWARDADD"
			Tags{ "LightMode" = "ForwardAdd" }

			Blend One One
			Cull off
			Fog{ Color(0, 0, 0, 0) }
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#pragma target 3.0
			#pragma multi_compile_fwdadd_fullshadows_fullshadows

			#pragma vertex vertForwardAddHair
			#pragma fragment fragForwardAddHair

			#include "TypeAFunction.cginc"


			struct VertexOutForwardAddHair
			{
				float4	pos			: SV_POSITION;
				float3	lightDir	: TEXCOORD0;
				float3	viewDir		: TEXCOORD1;
				float3	normal		: TEXCOORD2;
				float3	tangent		: TEXCOORD3;
				float2	uv			: TEXCOORD4;
				LIGHTING_COORDS(5, 6)
				UNITY_FOG_COORDS(7)
			};

			VertexOutForwardAddHair vertForwardAddHair(appdata_tan v)
			{
				VertexOutForwardAddHair o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.lightDir = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, ObjSpaceLightDir(v.vertex)));
				o.viewDir = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, ObjSpaceViewDir(v.vertex)));
				o.normal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
				o.tangent = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.tangent));
				o.uv = v.texcoord.xy;

				TRANSFER_VERTEX_TO_FRAGMENT(o);
				UNITY_TRANSFER_FOG(o, o.pos);

				return o;
			}

			float4 fragForwardAddHair(VertexOutForwardAddHair i, fixed facing : VFACE) : SV_Target
			{
				half atten = LIGHT_ATTENUATION(i);

				float4 tex = tex2D(_MainTex, i.uv);
				half  gloss = tex.a;
				tex *= _Color;

				half4 tdiff = toondiffuse(i.normal, i.lightDir);
				half4 hspec = toonanisspecular(i.normal, i.tangent, i.lightDir, i.viewDir) * gloss;

				half rim = 1.0 - saturate(dot(i.normal, i.viewDir));
				half4 r_c = (_Highlight * pow(rim, _RimPower)) * _RimIntensity;

				half4 c;
				c.rgb = (tex.rgb * _LightColor0.rgb * tdiff.rgb + r_c.rgb + (hspec.rgb * 0.75 )) * atten;
				c.a = tex.a + _LightColor0.a * atten;

				UNITY_APPLY_FOG(i.fogCoord, c);

				return c;
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}
