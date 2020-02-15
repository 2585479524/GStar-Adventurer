#ifndef TYPEA_FUNCTION_INCLUDED
#define TYPEA_FUNCTION_INCLUDED

#include "UnityCG.cginc"
#include "UnityLightingCommon.cginc"
#include "AutoLight.cginc"

#include "TypeAConfig.cginc"

half4 toondiffuse(float3 normal, float3 lightDir)
{
	float d = max(0, dot(normal, lightDir));

	half4 a = _Ambient;
	float td = smoothstep(_AmbientBorder - _AmbientBlur, _AmbientBorder + _AmbientBlur, d);
	half4 rd = lerp(a, _Diffuse, td);

	return  rd;
}

half4 toonspecular(float3 normal, float3 lightDir, float3 viewDir)
{
	float3 h = normalize(viewDir + lightDir);
	float nh = max(0, dot(normal, h));
	float s = pow(nh, 64 );
	half4 a = 0.0;
	float ts = smoothstep((1 - _HighlightPower), (1 - _HighlightPower) + _HighlightHardness, s);
	half4 rs = lerp(a, _Highlight, ts);

	return  rs;
}

float3 FresnelTerm(float3 rs, float lh)
{
	return rs + rs * (1 - lh);
}

half anisotropicspecular(float3 normal, float3 tangent, float3 lightDir, float3 viewDir)
{

	float nu = _NU * 100;
	float nv = _NV * 100;
	float rs = _HighlightPower;

	float3 h = normalize(viewDir + lightDir);
	float3 binormal = cross(normal, tangent);
	float hn = max(0, dot(h, normal));
	float hu = dot(h, tangent);
	float hv = dot(h, binormal);
	float hk = max(0, dot(h, lightDir));
	float nk1 = max(0, dot(normal, lightDir));
	float nk2 = max(0, dot(normal, viewDir));
	float exponent = (nu * ((hu * hu) / (1 - hn * hn))) + (nv * ((hv * hv)) / (1 - hn * hn));
	float term1 = sqrt((nu + 1) * (nv + 1)) / (8 * _PI);
	float term2 = pow(hn, exponent) / (hk * max(nk1, nk2));

	float as = term1 * term2 *FresnelTerm(rs, hk);

	return  as;
}


half4 toonanisspecular(float3 normal, float3 tangent, float3 lightDir, float3 viewDir)
{
	tangent = cross(normal, normalize(_Rotation));

	float as = anisotropicspecular(normal, tangent, lightDir, viewDir);
	half4 a = 0.0;
	float ts = smoothstep((1 - _HighlightPower), (1 - _HighlightPower) + (1 - _HighlightHardness), as);
	half4 rspec = lerp(a, _Highlight, ts);

	return  rspec;
}


half scraspect()
{
	half asp = sqrt(_ScreenParams.x * _ScreenParams.y) / 2.0;
	return asp;
}


#endif

