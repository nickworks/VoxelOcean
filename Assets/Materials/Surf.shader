Shader "Custom/Surf"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Noise("Noise", 2D) = "white"{}
		_NoiseStrength("Noise Strength", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 200

		Lighting Off

		GrabPass
		{
			"_BackgroundTexture"
		}

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Noise;
		sampler2D _BackgroundTexture;
		float _NoiseStrength;

        struct Input
        {
            float2 uv_MainTex;
			float screenPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full IN) {

			float tx = IN.vertex.x + sin(_Time) - .5;
			float ty = IN.vertex.y - _Time;

			fixed4 col = tex2Dlod(_Noise, float4(tx, ty, 0, 0));

			IN.vertex.xyz += col.rgb * _NoiseStrength;
			IN.vertex.xyz -= .5 * _NoiseStrength;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2Dproj(_BackgroundTexture, IN.screenPos) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
