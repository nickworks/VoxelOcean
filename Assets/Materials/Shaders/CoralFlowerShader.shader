Shader "Custom/CoralFlower"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Noise("Noise", 2D) = "white" {}
		_Wind("Current Strength", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

		#pragma vertex vert 


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

			float redDist = IN.color.r - .5;
			if (redDist < 0)redDist = 0;

			float4 pos = mul(unity_ObjectToWorld, IN.vertex) / 10;

			pos.x += _Time.y * .05f;

			fixed4 col = tex2Dlod(_Noise, float4(pos.xy, 0, 0)) - .5;

			//fixed4 offset = col.rgba * redDist;
			//offset.a = 0;
			//IN.vertex.xyz -= redDist * 15;
			//IN.vertex.xyz += offset * 30;

			float3 offest = lerp(float3(0, 0, 0), col.rgb, IN.color.r);

			IN.vertex.xyz += col.rgb; 
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            
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
