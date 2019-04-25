Shader "Custom/CreatureLuminentPlankton"
{
    Properties
    {
        _ColorTint ("Color Tint", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Range(1.0,6.0)) = 3.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0


        struct Input
        {
        	float4 color : Color;
            float2 uv_MainTex;
            float3 viewDir;
        };

float4 _ColorTint;
sampler2D _MainTex;
float4 _RimColor;
float _RimPower;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
        	IN.color = _ColorTint;
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * IN.color;
            half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
            o.Emission =  abs(sin(_Time.y) / 2 + 1 / 2) + (_RimColor.rbg * pow(rim, _RimPower));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
