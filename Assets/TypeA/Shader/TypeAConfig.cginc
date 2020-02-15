#ifndef TYPEA_CONFIG_INCLUDED
#define TYPEA_CONFIG_INCLUDED

#define _PI 3.14159265

float	_AmbientBorder;
float	_AmbientBlur;
float	_HighlightPower;
float	_HighlightHardness;

float	_NU;
float	_NV;
float3	_Rotation;

half4	_Color;
half4	_Shadow;
half4	_Ambient;
half4	_Diffuse;
half4	_Highlight;

half	_RimIntensity;
half	_RimPower;

half4	_EdgeColor;
float	_EdgeThickness;

fixed	_vc2edge;

sampler2D _MainTex;

#endif
