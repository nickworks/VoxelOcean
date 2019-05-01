Shader "Custom/SinWaveUnderWater"
{
    Properties
    {
		_MainTex("Camera", 2D) = "white" {} // screen camera REFERENCE DO NOT GIVE ANYTHING TO THIS
		_NormalMap("Normal Map", 2D) = "bump" {} // Normal Map 2d reference to a bump map / normal map, it is technically a height map
		_NoiseScale("NoiseScale", float) = 1 //Scale of the noise 
		_NoiseFrequency("NoiseFrequency", float) = 1 //frequency of the noise volume
		_NoiseSpeed("NoiseSpeed", float) = 1 // speed at which the noise moves
		_PixelOffset("PixelOffset", float) = 0.005 //offset controls how it looks
		_ThresholdDivide("Division", float) = 4 // Divisionary Threshold
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
       
		Pass {
	  CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				#define M_TAU 6.28318530718 //Ref to 2PI 6.28318530718
		sampler2D _CameraDepthTexture; //depth of camera reference to camera


		sampler2D _NormalMap, _MainTex; //noise map && Ref to Camera

		uniform float _PixelOffset, _NoiseScale, _NoiseSpeed, _NoiseFrequency, _ThresholdDivide; //noise settings && Pixel Offset, && ThresholdDivide used for division 
		
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

		
		float3 Spos = float3 (i.scrPos.x, i.scrPos.y, 0) * _NoiseFrequency; 
		fixed4 noisetex =  tex2D(_NormalMap, i.uv + float2(_Time.y * (_NoiseSpeed / 100), _Time.x * (_NoiseSpeed / 100))); 
		Spos.x += _Time.x * _NoiseSpeed; 
		float noiseConv = _NoiseScale * ((noisetex * _Time.x * (Spos)) / _ThresholdDivide); 
		float4 noiseToDirection = float4(sin(noiseConv * (M_TAU)), cos(noiseConv * (M_TAU)), tan(noiseConv * (M_TAU)), 0);
	
		
		fixed4 col = tex2Dproj(_MainTex, i.scrPos + (normalize(noiseToDirection) * _PixelOffset)); // Texture look up for camera and placed on camera
		return col;
		//Is now replicating a SIN wave 
			}
		ENDCG
			}
    }
 
}
