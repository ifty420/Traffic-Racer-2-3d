Shader "LightShape/LightSpread Shader" {
	Properties {
		
		_MainTex ( "Main Texture (RGB) Emission (A)", 2D ) = "white" {}
		_DarkTex ( "Dark Texture (RGB)", 2D ) = "white" {}

		_BumpMap ( "Normal Map (RGB)", 2D ) = "bump" {}
		//_RimX  ( "Push X", float ) = 1
		
		_RimRamp ( "Rimlight Ramp (A)", 2D ) = "white" {}
		_SpecTex ( "Specular Color (RGB) Gloss (A)", 2D ) = "white" {}
		_MainLevel  ( "Main Brightness", float ) = 1
		_Brightness ( "Cubemap Brightness", float ) = 1
		_SpecLevel  ( "Specular Brightness", float ) = 1
		_LightSpread ( "Light Spread", float ) = 1		 
		_Cube ( "Reflection Map", CUBE ) = "" {}

	}
	
	SubShader {
		Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase"}
		
		CGPROGRAM

			#pragma target 3.0
			#pragma exclude_renderers flash
			#pragma surface surf CBlinnPhong fullforwardshadows
			#pragma only_renderers d3d9 opengl
			#pragma glsl
			
			struct CustomSurfaceOutput
			{
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half Specular;
				half3 GlossColor;
				half LightSpread;
				half Alpha;
				half3 darkTex;
				half3 mainTex;
			};
						
			struct Input
			{
				float2 uv_MainTex;
				float3 viewDir;
				float3 worldRefl;
				INTERNAL_DATA
			};
			
			
			sampler2D _MainTex;
			sampler2D _DarkTex;

			sampler2D _BumpMap;
			samplerCUBE _Cube;
			fixed _Brightness;
			fixed _SpecLevel;
			fixed _MainLevel;
			fixed _LightSpread;

			sampler2D _SpecTex;
			sampler2D _RimRamp;
			
			void surf(Input IN, inout CustomSurfaceOutput o)
			{
				fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 darkTex = tex2D(_DarkTex, IN.uv_MainTex);								
				_SpecColor = tex2D(_SpecTex, IN.uv_MainTex);
								
				o.Albedo = darkTex.rgb;
				o.mainTex = mainTex.rgb * 0.3 * _MainLevel;
				o.darkTex = darkTex.rgb * 0.7;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
				
				o.Specular = max(0.0, 1.0 - _SpecColor.a);
				o.GlossColor = _SpecColor.rgb * 0.5 * _SpecLevel;
				fixed4 reflcol = texCUBElod (_Cube, float4(WorldReflectionVector(IN, o.Normal), _SpecColor.a * 4));
								
				o.LightSpread = _LightSpread;
								
				o.Emission = reflcol  * _Brightness * 0.5 * tex2D(_RimRamp, fixed2(saturate(dot(normalize(IN.viewDir), o.Normal) / _SpecColor.a - .3), 0)).a * dot(saturate(_SpecColor.rgb), fixed3(0.3, 0.59, 0.11));
				o.Emission += mainTex.rgb * mainTex.a;							
			}

			inline fixed4 LightingCBlinnPhong (CustomSurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
			{
                lightDir = normalize(lightDir);

                viewDir = normalize(viewDir);

                half3 h = normalize (lightDir + viewDir);

                half diff = saturate(max (0, dot (s.Normal, lightDir)) * s.LightSpread) * 1.1;
                float nh = max (0, dot (s.Normal, h));
                fixed spec = pow (nh, s.Specular * 128.0);
                half4 c;

                c.rgb = saturate(lerp( s.darkTex, s.mainTex, diff) * max(float3(0.2, 0.2, 0.2), _LightColor0.rgb) * diff + _LightColor0.rgb * s.GlossColor * spec * diff) * (atten * 2) + s.Emission;

                //c.a = s.Alpha + _LightColor0.a * Luminance(s.GlossColor) * spec * atten;

                return c;
			}

		ENDCG
	}
	
	Fallback "Diffuse"
}