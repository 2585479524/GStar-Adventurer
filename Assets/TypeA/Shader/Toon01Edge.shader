// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "TypeA/Toon01Edge"
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
		
		_EdgeColor("Edge Color", Color) = (0,0,0,1)
		_EdgeThickness("Edge Thickness", Range(0, 5)) = 1

		[NoScaleOffset]_MainTex("Color(RGB) Specular(A)", 2D) = "white" {}

		[Enum(not use,0,use,1)] _vc2edge("VertexColor to Edge", Float) = 0
	}


	SubShader
	{
		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque"}
		UsePass "TypeA/Toon01/FORWARDBASE"
		UsePass "TypeA/Toon01/FORWARDADD"

		//-------------------------------------------------------------------------------------
		// Toon01Edge Forward Base

		Pass
		{
			Name "FORWARDBASE"
			Tags{"LightMode" = "ForwardBase" }

			Cull Front			
			ZWrite On

			CGPROGRAM
			#pragma target 3.0
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#pragma vertex vertForwardBaseEdge
			#pragma fragment fragForwardBaseEdge

			#include "TypeAFunction.cginc"

			struct appdata
			{
				float4	vertex		: POSITION;
				float3	normal		: NORMAL;
				float4	color		: COLOR;
			};

			struct VertexOutForwardBaseEdge
			{
				float4	pos			: SV_POSITION;
				SHADOW_COORDS(0)
				UNITY_FOG_COORDS(1)
			};

			VertexOutForwardBaseEdge vertForwardBaseEdge(appdata v)
			{
				VertexOutForwardBaseEdge o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
				float2 offset = TransformViewToProjection(norm.xy);
				float2 vo_pos = o.pos.xy / o.pos.w;
				float2 vtr_pos = (o.pos.xy + offset) / o.pos.w;
				float coeff = 1 / (scraspect() * distance(vo_pos, vtr_pos));

				float4 e = v.color * 2;
				e.r = (e.r - 1) * coeff * 0.5;
				e.b *= coeff * 0.5;
				e = _vc2edge > 0 ? e : 0;

				o.pos.xy += offset * (coeff + e.r - e.b) * _EdgeThickness;
				o.pos.z += 0.0001;

				TRANSFER_SHADOW(o);
				UNITY_TRANSFER_FOG(o, o.pos);

				return o;
			}

			fixed4 fragForwardBaseEdge(VertexOutForwardBaseEdge i) : SV_Target
			{
				fixed atten = SHADOW_ATTENUATION(i);

				fixed4 c;
				c.rgb = UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb * _EdgeColor.rgb;
				c.rgb += (_Color.rgb * _EdgeColor.rgb * _LightColor0.rgb) * saturate(atten + _Shadow.rgb);
				c.a = _LightColor0.a *atten;

				UNITY_APPLY_FOG(i.fogCoord, c);

				return c;
			}
			ENDCG
		}


		//-------------------------------------------------------------------------------------
		// Toon01Edge Forward Add

		Pass
		{
			Name "FORWARDADD"
			Tags{ "LightMode" = "ForwardAdd" }
			Blend One One
			Cull Front
			Fog{ Color(0, 0, 0, 0) }
			ZWrite Off
			ZTest LEqual

			CGPROGRAM
			#pragma target 3.0
			#pragma multi_compile_fwdadd

			#pragma vertex vertForwardAddEdge
			#pragma fragment fragForwardAddEdge

			#include "TypeAFunction.cginc"


			struct VertexOutForwardAddEdge
			{
				float4	pos			: SV_POSITION;
				LIGHTING_COORDS(0, 1)
				UNITY_FOG_COORDS(2)

			};

			VertexOutForwardAddEdge vertForwardAddEdge(appdata_full v)
			{
				VertexOutForwardAddEdge o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
				float2 offset = TransformViewToProjection(norm.xy);
				float2 vo_pos = o.pos.xy / o.pos.w;
				float2 vtr_pos = (o.pos.xy + offset) / o.pos.w;
				float coeff = 1 / (scraspect() * distance(vo_pos, vtr_pos));

				float4 e = v.color * 2;
				e.r = (e.r - 1) * coeff * 0.5;
				e.b *= coeff * 0.5;
				e = _vc2edge > 0 ? e : 0;

				o.pos.xy += offset * (coeff + e.r - e.b) * _EdgeThickness;
				o.pos.z += 0.0001;

				TRANSFER_VERTEX_TO_FRAGMENT(o);
				UNITY_TRANSFER_FOG(o, o.pos);

				return o;
			}

			fixed4 fragForwardAddEdge(VertexOutForwardAddEdge i) : SV_Target
			{
				fixed atten = LIGHT_ATTENUATION(i);
				fixed4 c;
				c.rgb = (_Color.rgb * _EdgeColor.rgb * _LightColor0.rgb) * atten;
				c.a = _Color.a;

				UNITY_APPLY_FOG(i.fogCoord, c);

				return c;
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}
