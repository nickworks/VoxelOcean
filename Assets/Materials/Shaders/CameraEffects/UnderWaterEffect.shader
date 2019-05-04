Shader "Custom/SinWaveUnderWater"
{
    Properties
    {
		_MainTex("Camera", 2D) = "white" {} // screen camera REFERENCE DO NOT GIVE ANYTHING TO THIS
		_NoiseMap("Noise Map", 2D) = "white" {} // Noise Map reference
		_NoiseScale("NoiseScale", float) = 1 //Scale of the noise 
		_NoiseSpeed("NoiseSpeedX", float) = 1 // speed at which the noise moves X 
		_NoiseSpeed("NoiseSpeedY", float) = 1 // speed at which the noise moves Y
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
       
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _NoiseMap, _MainTex; //noise map && Ref to Camera

			uniform float _NoiseScale, _NoiseSpeedX, _NoiseSpeedY; //noise settings && Pixel Offset, && ThresholdDivide used for division 
		
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			fixed4 frag(v2f i) : COLOR
			{

				float2 noiseUV = i.uv + float2(_NoiseSpeedX, _NoiseSpeedY) * _Time.y;
				fixed2 warpedUV =  i.uv + (tex2D(_NoiseMap, noiseUV).gb - fixed2(.5, .5)) * _NoiseScale;
				fixed4 col = tex2D(_MainTex, warpedUV);
				return col;
			}
			ENDCG
		}
    }
}
