Shader "Custom/DeepBlue"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cube ("Environment Map", Cube) = "white" {}
      	_ColorTop ("Top Color", Color) = (1,.5,.5,1)
      	_ColorBottom ("Bottom Color", Color) = (1,.5,.5,1)
    }

  SubShader {
      Tags { "Queue"="Background"  }

      Pass {
         ZWrite Off 
         Cull Off

         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag

         #include "UnityCG.cginc"
         #include "UnityShaderVariables.cginc"

         // User-specified uniforms
         samplerCUBE _Cube;
         fixed4 _ColorTop;
         fixed4 _ColorBottom;

/*
         struct Input{
         	float3 viewDir;
         	float p;//
         	fixed4 ScreenColor;// = lerp(_ColorTop,_BottomTop,p);

         };
*/
         struct vertexInput {
            float4 vertex : POSITION;
            float3 texcoord : TEXCOORD0;
         };

         struct vertexOutput {
            float4 vertex : SV_POSITION;
            float3 texcoord : TEXCOORD0;
            fixed4 screenColor : COLOR;
         };

         vertexOutput vert(vertexInput input)
         {
         	
            vertexOutput output;
            output.vertex = UnityObjectToClipPos(input.vertex);
            
            //float3 viewDir = WorldSpaceViewDir(input.vertex);

        	float p = input.texcoord.y;

			output.screenColor  = lerp(_ColorBottom,_ColorTop,p);
            
            output.texcoord = input.texcoord;

            return output;
         }

         fixed4 frag (vertexOutput input) : COLOR
         {
         	fixed4 col = input.screenColor;
            return col;
         }
         ENDCG 
      }
   } 	
/*
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
    */
}

 
