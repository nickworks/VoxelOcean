Shader "ImageEffect/WaterDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // plug in a texture
		_NormalMap ("Normal Map", 2D) = "bump" {} // plug in a 2d normal Map
		_Noise ("Noise", 2D) = "white" {} // plug in noise data only 2d, trying to add 3d simplex noise
		_Transparency("Transparency", Range(0.0,0.5)) = .25 //sets the Transparency
		
    }
    SubShader
    {
		// sets queue to render Transparent, which renders anything after solid geometry and alpha test
		// aslo sets the render type to Transparent rather than opaque
		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        // No depth
        ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			

            #include "UnityCG.cginc"
			//#include "noiseSimplex.cginc"

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			sampler2D _Noise;
			sampler2D _NormalMap;
			fixed4 _Color;
			float _Transparency;


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				fixed4  noise1 = tex2D(_NormalMap, i.uv + float2(_Time.y * .06, _Time.y*.06)); // takes the normal texture and moves it based on time
				fixed4  noise2 = tex2D(_NormalMap, i.uv + float2(_Time.y * -.09, _Time.y * -.08)); // takes the normal texture and moves it based on time
                // just invert the colors
				
                
				col.a = _Transparency; // sets the alpha based on Transparency input
				col = col - (noise1 * (col.a * 0.15)); // takes col and subtracts it from ( noise1 mult (col's alpha by .15))
				col = col - (noise2 * (col.a * 0.15)); // takes col and subtracts it from ( noise2 mult (col's alpha by .15))
				col.rgb += .1 // add col's rgb by .1

                return col;
            }
            ENDCG
        }
    }
}
