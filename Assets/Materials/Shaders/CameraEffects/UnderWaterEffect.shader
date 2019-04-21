Shader "Custom/UnderWaterEffect"
{
    Properties
    {
	   _Noise("Noise", 2D) = "white" {} // screen
	_NormalMap("Normal Map", 2D) = "bump" {} // Normal Map 2d
		_NoiseScale("NoiseScale", float) = 1 //Scale of the noise 
		_NoiseFrequency("NoiseFrequency", float) = 1 //frequency of the noise volume
		_NoiseSpeed("NoiseSpeed", float) = 1 // speed at which the noise moves
		_PixelOffset("PixelOffset", float) = 0.005 //offset controls how it looks
		_DepthStart("Depth Start", float) = 1 //the depth at which the noise volume will start, recommended to put it inFRONT of the camera
		_DepthDist("Depth Distance", float) = 1 //Depth distance is how far it is that won't be affected by the distortion effect
		_Transparency("Transparency", Range(0.0,0.5)) = .25
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
       
		Pass {
	  CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				#define M_PI 3.1415926535897932384626433832795
		sampler2D _CameraDepthTexture;
		uniform float _PixelOffset, _NoiseScale, _NoiseSpeed, _NoiseFrequency;
		
		float _DepthStart;
		float _DepthDist;
		sampler2D _Noise;
		sampler2D _NormalMap;
		float _Transparency;
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


		sampler2D _MainTex;


			fixed4 frag(v2f i) : COLOR
		{

		float depthValue = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r * _ProjectionParams.z);
		
		depthValue = 1 - saturate((depthValue - _DepthStart) / _DepthDist);
		float3 spos = float3 (i.scrPos.x, i.scrPos.y, 0) * _NoiseFrequency;
		fixed4 noise1 = tex2D(_NormalMap, i.uv + float2(_Time.y * .05, _Time.y * .04));
		spos.z += _Time.x * _NoiseSpeed;
		float noise = _NoiseScale * ((noise1 * (spos) + 1) / 3);
		float4 noiseToDirection = float4(cos(noise * (M_PI * 2)), sin(noise*(M_PI*2)), 0, 0);
		fixed4 col = tex2Dproj(_MainTex, i.scrPos + (normalize(noiseToDirection) * _PixelOffset * depthValue));
		
		col.rgb += .1;
		return col;
		}
		ENDCG
			}
    }
 
}
