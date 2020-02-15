// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TypeA/Toon01"
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
		_HighlightHardness("Highlight Hardness", Range(0.001, 1)) = 0.1

		_RimIntensity("Rim Intensity", Range(0, 1)) = 0.5
		_RimPower("Rim Power", Range(0.5, 10.0)) = 3.0

		[NoScaleOffset]_MainTex("Color(RGB) Specular(A)", 2D) = "white" {}
	}


	SubShader
	{
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque"}

		//-------------------------------------------------------------------------------------
		// Toon01 Forward Base
		pass
		{
			Name "FORWARDBASE"
			Tags{ "LightMode" = "Forwardbase" }

			Cull Off
			ZWrite On


			CGPROGRAM
			#pragma target 3.0
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#pragma vertex vertForwardBase
			#pragma fragment fragForwardBase

			#include "TypeAFunction.cginc"

			struct VertexOutForwardBase
			{
				float4	pos			: SV_POSITION;
				float3	lightDir	: TEXCOORD0;
				float3	viewDir		: TEXCOORD1;
				float3	normal		: TEXCOORD2;
				float2	uv			: TEXCOORD3;
				SHADOW_COORDS(4)
				UNITY_FOG_COORDS(5)
			};

			VertexOutForwardBase vertForwardBase(appdata_base v)
			{
				VertexOutForwardBase o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.lightDir = mul((float3x3)UNITY_MATRIX_IT_MV, ObjSpaceLightDir(v.vertex));
				o.viewDir = mul((float3x3)UNITY_MATRIX_IT_MV, ObjSpaceViewDir(v.vertex));
				o.normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				o.uv = v.texcoord.xy;
				TRANSFER_SHADOW(o);
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}

			float4 fragForwardBase(VertexOutForwardBase i, fixed facing : VFACE) : SV_Target
			{
				i.lightDir = normalize(i.lightDir);
				i.viewDir = normalize(i.viewDir);
				i.normal = normalize(i.normal);
			
				half atten = SHADOW_ATTENUATION(i);

				float4 tex = tex2D(_MainTex, i.uv);
				half  gloss = tex.a;
				tex *= _Color;

				half4 tdiff = toondiffuse(i.normal, i.lightDir);
				half4 tspec = toonspecular(i.normal, i.lightDir, i.viewDir) * gloss;

				half rim = 1.0 - saturate(dot(i.viewDir, i.normal));
				half4 r_c = (_Highlight * pow(rim, _RimPower)) * _RimIntensity;

				half3 f = saturate((atten + _Shadow.rgb) * facing) + frac(clamp(facing, _Ambient.rgb, 1));
				half4 c;
				c.rgb = UNITY_LIGHTMODEL_AMBIENT.rgb * tex.rgb;
				c.rgb += (tex.rgb * _LightColor0.rgb * tdiff.rgb + tspec + r_c.rgb) * f;
				c.a = tex.a + _LightColor0.a * atten;

				UNITY_APPLY_FOG(i.fogCoord, c);

				return c;
			}
			ENDCG
		}

		//-------------------------------------------------------------------------------------
		// Toon01 Forward Add
		Pass
		{
			Name "FORWARDADD"
			Tags{ "LightMode" = "ForwardAdd" }

			Blend One One
			Cull Off
			Fog{ Color(0, 0, 0, 0) }
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#pragma target 3.0
			#pragma multi_compile_fwdadd_fullshadows

			#pragma vertex vertForwardAdd
			#pragma fragment fragForwardAdd

			#include "TypeAFunction.cginc"


			struct VertexOutForwardAdd
			{
				float4	pos			: SV_POSITION;
				float3	lightDir	: TEXCOORD0;
				float3	viewDir		: TEXCOORD1;
				float3	normal		: TEXCOORD2;
				float2	uv			: TEXCOORD3;
				LIGHTING_COORDS(4, 5)
				UNITY_FOG_COORDS(6)
			};

			VertexOutForwardAdd vertForwardAdd(appdata_tan v)
			{
				VertexOutForwardAdd o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.lightDir = mul((float3x3)UNITY_MATRIX_IT_MV, ObjSpaceLightDir(v.vertex));
				o.viewDir = mul((float3x3)UNITY_MATRIX_IT_MV, ObjSpaceViewDir(v.vertex));
				o.normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
				o.uv = v.texcoord.xy;

				TRANSFER_VERTEX_TO_FRAGMENT(o);
				UNITY_TRANSFER_FOG(o, o.pos);

				return o;
			}

			float4 fragForwardAdd(VertexOutForwardAdd i, fixed facing : VFACE) : SV_Target
			{
				i.lightDir = normalize(i.lightDir);
				i.viewDir = normalize(i.viewDir);
				i.normal = normalize(i.normal);
				half atten = LIGHT_ATTENUATION(i);

				float4 tex = tex2D(_MainTex, i.uv);
				half  gloss = tex.a;
				tex *= _Color;

				half4 tdiff = toondiffuse(i.normal, i.lightDir);
				half4 tspec = toonspecular(i.normal, i.lightDir, i.viewDir) * gloss;

				half rim = 1.0 - saturate(dot(i.viewDir, i.normal));
				half4 r_c = (_Highlight * pow(rim, _RimPower)) * _RimIntensity;

				half4 c;
				c.rgb = (tex.rgb * _LightColor0.rgb * tdiff.rgb + tspec.rgb + r_c.rgb) * atten;
				c.a = tex.a + (_LightColor0.a * atten);

				UNITY_APPLY_FOG(i.fogCoord, c);

				return c;
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}
