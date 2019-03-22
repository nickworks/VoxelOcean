Shader "Custom/WormWiggle"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
		_Wind("Current Strength", Float) = 1.0
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Noise;

        struct Input
        {
            float2 uv_MainTex;
			float4 color : COLOR;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		float _Wind;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		void vert(inout appdata_full IN) {

			float whtDist = ((IN.color.r + IN.color.g + IN.color.b) / 3);
			float redDist = fixed4 (1, 0, 0, 1) - ((IN.color.r + IN.color.g + IN.color.b) / 3);

			float colDist = (whtDist + redDist) / 2;

			float tx = IN.vertex.x + _Time.x;
			float tz = IN.vertex.z + _Time.x;

			fixed4 col = tex2Dlod(_Noise, float4(tx, tz, 0, 0));

			fixed4 offset = col.rgba * _Wind * ((colDist > .5) ? colDist : 0);

			IN.vertex.xyz += offset;
			IN.vertex.xyz -= .5 * offset;
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            //o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;

			o.Albedo = IN.color;

            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
