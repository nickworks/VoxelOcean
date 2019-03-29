Shader"CoralShaders/CoralGlow"
{
	//Properties we are recieving from our inspector
	Properties
	{

		//The amount of glow applied to the corals emission
		_Glow("How bright the coral should glow", Range(0, 1)) = 0
		//_Cutoff value if this is under the .r value of the _CutOffTexture then we should apply the _Secondary color
		_Cutoff("CutOff Range", Range(0, 1)) = 1
		//We create a primary color property that recieves color information from the inspector, it is a float4
		_PrimaryColor("Primary Coral Color", Color) = (1,1,1,1)
		//We create a secondary color property that recieves color information from the inspector, it is a float4
		_SecondaryColor("Secondary Coral Color", Color) = (1,1,1,1)
		//A CutOffTexture used to determine where primary and secondary colors are applied
		_CutOffTexture("Used to determine where primary and secondary colors are applied", 2D) = "white"
		
		
		

	}//End of properties block
	SubShader
	{
		//This coral has the ability to implement alpha and fade in or out //Not implemented in this shader
		Tags{"RenderType" = "AlphaTest"}

		CGPROGRAM //We start writing CG and HLSL here
#pragma surface surf Standard //A surface shader with a function called surf using the Standard PBR lighting model

		struct Input// we recieve UV info in our input struct
		{
			float2 uv_CutOffTexture; //The only texture we are using is our CutOffTexture so we get it's UV's
		};//End Input struct

		//We redeclare our properties

		//Both of our colors have RGBA values so they are defined as float4's for optimization we could change this to fixed4's at a later time if we wanted with little
		//to no difference
		float4 _PrimaryColor;
		float4 _SecondaryColor;
		//To Get access to our _CutOffTexture we declare it as a sampler2D because we are sampling a 2D texture
		sampler2D _CutOffTexture;
		//We declare glow and threshold as floats because they are just values from 0 to 1
		float _Cutoff;
		float _Glow;

		//Our Surf method we declared in our pragma up above
		//We pass in our Input struct as IN to get access to UV's
		//We pass in our lighting model(SurfaceOutputStandard) as o
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			//We create a new variable called C that is set to our _CutOffTexture using a tex2D() texture lookup
			fixed4 c = tex2D(_CutOffTexture, IN.uv_CutOffTexture);

			//We lerp between the two colors based on c.r's channel information
			o.Albedo = lerp(_PrimaryColor, _SecondaryColor, c.r);
			//We use a lerp to multiply our _Glow by our colors so that the glowing emissive effect takes into acount our colors
			float3 newGlow = lerp(_Glow * _PrimaryColor.rgb,_Glow * _SecondaryColor.rgb, c.r);
			//We add our glow to our emission
			o.Emission = newGlow;
		}//End of surf method

		ENDCG
	}
}//End of Shader