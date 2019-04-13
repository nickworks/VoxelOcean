Shader"CoralShaders/CoralGlow"
{
	//Properties we are recieving from our inspector
	Properties
	{

		//The amount of glow applied to the corals emission
		_Glow("How bright the coral should glow", Range(0, 1)) = 0
		
		//We create a primary color property that recieves color information from the inspector, it is a float4
		_PrimaryColor("Primary Coral Color", Color) = (1,1,1,1)
		
	
		
		
		

	}//End of properties block
	SubShader
	{
		//This coral has the ability to implement alpha and fade in or out //Not implemented in this shader
		Tags{"RenderType" = "AlphaTest"}

		CGPROGRAM //We start writing CG and HLSL here
#pragma surface surf Standard //A surface shader with a function called surf using the Standard PBR lighting model

		
		struct Input
		{
			float2 uv_MainTex;
		};
		//We redeclare our properties

		//Both of our colors have RGBA values so they are defined as float4's for optimization we could change this to fixed4's at a later time if we wanted with little
		//to no difference
		float4 _PrimaryColor;
		
		//The glow amount our coral is outputting through the emission
		float _Glow;

		//Our Surf method we declared in our pragma up above
		//We pass in our Input struct as IN to get access to UV's
		//We pass in our lighting model(SurfaceOutputStandard) as o
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			

			o.Albedo = _PrimaryColor.rgb;
			
			//We add our glow to our emission
			o.Emission = _Glow * _PrimaryColor.rgb;
		}//End of surf method

		ENDCG
	}
}//End of Shader