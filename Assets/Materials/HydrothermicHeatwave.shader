Shader "Custom/HydrothermicHeatwave"
{
    Properties
    {
        _MainTex ("Texture1", 2D) = "white" {}
		_Noise("Noise", 2D) = "white"{}
		_NoiseStrength("Noise Strength", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100

		GrabPass
		{}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 norm: NORMAL;
            };

            struct v2f
            {
                float2 uv1 : TEXCOORD0; //_MainTex
                float2 uv2 : TEXCOORD1; //_Noise
                float4 grabPos : TEXCOORD2; //grabPos
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _Noise;
			float4 _Noise_ST;
			float _NoiseStrength;

			sampler2D _GrabTexture;

            v2f vert (appdata v)
            {
                v2f o;

				fixed4 ogVect;
				ogVect.rgba = v.vertex.rgba;
				//v.vertex.xyz += sin(_Time.y) * v.norm * .1;

				float tx = v.vertex.x + sin(_Time) - .5;
				float ty = v.vertex.y - _Time;

				fixed4 col = tex2Dlod(_Noise, float4(tx, ty, 0, 0));

				v.vertex.xyz += col.rgb * _NoiseStrength;
				v.vertex.xyz -= .5 * _NoiseStrength;

				//
				o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv1 = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv, _Noise);
                UNITY_TRANSFER_FOG(o,o.vertex);


				o.grabPos = ComputeGrabScreenPos(ogVect);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				half4 bgcolor = tex2Dproj(_GrabTexture , i.grabPos);
				return bgcolor;
            }
            ENDCG
        }
    }
}
