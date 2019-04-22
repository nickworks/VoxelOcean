Shader "Custom/UnderWaterEffect"
{
    Properties
    {
	   _MainTex("Noise", 2D) = "white" {} // screen camera REFERENCE DO NOT GIVE ANYTHING TO THIS
		_NormalMap("Normal Map", 2D) = "bump" {} // Normal Map 2d
		_NoiseScale("NoiseScale", float) = 1 //Scale of the noise 
		_NoiseFrequency("NoiseFrequency", float) = 1 //frequency of the noise volume
		_NoiseSpeed("NoiseSpeed", float) = 1 // speed at which the noise moves
		_PixelOffset("PixelOffset", float) = 0.005 //offset controls how it looks
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
       
		Pass {
	  CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				#define M_PI 3.1415926535897932384626433832795
		sampler2D _CameraDepthTexture; //depth of camera reference to camera
		uniform float _PixelOffset, _NoiseScale, _NoiseSpeed, _NoiseFrequency; //noise settings

		sampler2D _NormalMap; //noise map
		sampler2D _MainTex; //ref to camera
		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 vertex : SV_POSITION;
			float4 scrPos : TEXCOORD1;
		};

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.scrPos = ComputeScreenPos(o.vertex);
			o.uv = v.uv;
			return o;
		}





			fixed4 frag(v2f i) : COLOR
		{

		
		float3 spos = float3 (i.scrPos.x, i.scrPos.y, 0) * _NoiseFrequency; //gets screen position
		fixed4 noise1 = tex2D(_NormalMap, i.uv + float2(_Time.y * .05, _Time.x * .06)); // Gets noise sampler and converts to float
		spos.x += _Time.x * _NoiseSpeed; // X coords
		spos.y += _Time.y * _NoiseSpeed; //Y coords
		spos.z += _Time.z * _NoiseSpeed; //Z COORDS
		float noise = _NoiseScale * ((noise1 * _Time.x * (spos) + 1) / 4);
		float4 noiseToDirection = float4(cos(noise * (M_PI * 2)), sin(noise*(M_PI*2)), 0, 0);
		fixed4 col = tex2Dproj(_MainTex, i.scrPos + (normalize(noiseToDirection) * _PixelOffset));
		return col;
		}
		ENDCG
			}
    }
 
}
