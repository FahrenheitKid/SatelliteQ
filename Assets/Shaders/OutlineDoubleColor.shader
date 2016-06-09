Shader "Custom/Outline Double Color" 
{
	Properties
	{
		_Color("Main Color", Color) = (.5,.5,.5,1)
		_Outline("Outline Width", Range(0.0, 0.03)) = 0.005
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Color1("First Color", Color) = (0.8, 0, 0, 0.5)
		_Color2("Second Color", Color) = (0.8, 0.8, 0, 0.5)
	}
 
CGINCLUDE
#include "UnityCG.cginc"
 
struct appdata 
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
};
 
struct v2f 
{
	float4 pos : POSITION;
	float4 color : COLOR;
};
 
uniform float _Outline;
uniform float4 _Color1;
uniform float4 _Color2;
 
v2f vert(appdata_full v) 
{
	v2f a;
	a.pos = mul(UNITY_MATRIX_MVP, v.vertex);
 
	float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
	float2 offset = TransformViewToProjection(norm.xy);

	a.pos.xy += offset * a.pos.z * _Outline;
	a.color = lerp(_Color1, _Color2, v.texcoord.y);
	return a;
}
ENDCG
 
	SubShader 
	{
		Tags { "Queue" = "Transparent" }
 
		// note that a vertex shader is specified here but its using the one above
		Pass 
		{
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Off
			ZWrite Off
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha
 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
 
			half4 frag(v2f i) : COLOR 
			{
				return i.color;
			}
			ENDCG
		}
 
 
		CGPROGRAM
		#pragma surface surf Lambert
		
		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};
		sampler2D _MainTex;
		sampler2D _BumpMap;
		uniform float3 _Color;

		void surf(Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	}
 
	Fallback "Standard"
}