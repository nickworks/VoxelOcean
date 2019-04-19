Shader "Custom/CoralFlowerShader"
{
    Properties
    {
		[PerRendererData]
        _Color ("Color", Color) = (1,1,1,1)	
		_MainTex("Main Texture", 2D) = "white"{}
		_DispTex("Displacement Texture", 2D) = "white"{}
		_DispPower("Displacement Power" , Float) = 0
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}

		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
			

		GrabPass{}

        
        
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
				float4 grabPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _DispTex;
			sampler2D _VinTex;
			sampler2D _GrabTexture;
			fixed4 _Color;
			float _DispPower;

			float _StartOpac = 1.0;

			v2f vert(appdata v) {
				v2f o;
				o.uv = v.uv;
				o.color = v.color;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.grabPos = ComputeScreenPos(o.vertex);
				o.grabPos /= o.grabPos.w;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				_StartOpac -= unity_DeltaTime.y;

				fixed4 srcCol = tex2D(_MainTex, i.uv);
				fixed4 dispPos = tex2D(_DispTex, float2(i.uv.x, i.uv.y - (_Time.y + i.color.a / 2)));
				fixed4 vinCol = tex2D(_VinTex, i.uv);
				float2 offset = (dispPos.xy * 2 - 1) * _DispPower * vinCol.r;

				fixed4 grabColor = tex2D(_GrabTexture, i.grabPos.xy + offset);
				fixed4 texColor = tex2D(_MainTex, i.uv)*i.color;

				fixed4 finalColor = lerp(fixed4(grabColor.rgb, 0.0), grabColor, vinCol.r * 2);
				finalColor.a *= i.color.a;

				return finalColor;
			}
			ENDCG
		}
    }
    
}
