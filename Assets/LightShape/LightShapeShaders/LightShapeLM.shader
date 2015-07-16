// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "LightShape/LightMap Shader" {
Properties {
		_MainTex ( "Main Texture (RGB) Emission (A)", 2D ) = "white" {}
		_DarkTex ( "Dark Texture (RGB)", 2D ) = "white" {}
		_BumpMap ( "Normal Map (RGB)", 2D ) = "bump" {}
		_RimRamp ( "Rimlight Ramp (A)", 2D ) = "white" {}
		_SpecTex ( "Specular Color (RGB) Gloss (A)", 2D ) = "white" {}
		_Brightness ( "Cubemap Brightness", float ) = 1
		_SpecLevel  ( "Specular Brightness", float ) = 1
		_Cube ( "Reflection Map", CUBE ) = "" {}
		_Dummy ("DO NOT TOUCH", 2D) = "white" {}
	}
	
	SubShader {
		Tags { "RenderType" = "Opaque" }
		
		CGPROGRAM
			#pragma target 3.0
			#pragma exclude_renderers flash
			#pragma surface surf CBlinnPhong nolightmap fullforwardshadows exclude_path:prepass
			#pragma only_renderers d3d9 opengl
			#pragma glsl
			
			struct CustomSurfaceOutput
			{
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half Specular;
				half3 GlossColor;				
				half Alpha;
				half3 darkTex;
				half3 mainTex;
			};
			
			struct Input
			{
				float2 uv_MainTex;
				float2 uv2_Dummy;
				float3 viewDir;
				float3 worldRefl;
				INTERNAL_DATA
			};
			
			sampler2D _MainTex;
			sampler2D _DarkTex;
			sampler2D _BumpMap;
			samplerCUBE _Cube;
			float _Brightness;
			sampler2D _SpecTex;
			fixed _SpecLevel;
			sampler2D _RimRamp;
			
			// sampler2D unity_Lightmap;
			// float4 unity_LightmapST;
			
			void surf(Input IN, inout CustomSurfaceOutput o)
			{
				fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 darkTex = tex2D(_DarkTex, IN.uv_MainTex);
				
				float2 lmuv = IN.uv2_Dummy.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				fixed3 lightmapInfo = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, lmuv));
				_SpecColor = tex2D(_SpecTex, IN.uv_MainTex);
				float des = lightmapInfo.rgb;
				fixed4 finalColor = lerp(darkTex, mainTex , des);
				
				o.Albedo = darkTex.rgb;
				o.mainTex = (des * 2 - 1) * mainTex.rgb * 0.3;
				o.darkTex = (des * 2 - 1) * darkTex.rgb * 1.1;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
				o.Specular = max(0.0, 1.0 - _SpecColor.a);
				o.GlossColor = (des * 2 - 1) * _SpecColor.rgb  * 0.6 * _SpecLevel;
				
				fixed4 reflcol = (des * 0.25 + 0.75) * texCUBElod (_Cube, float4(WorldReflectionVector(IN, o.Normal), _SpecColor.a * 4));

				//o.Emission = reflcol * (_SpecColor.rgb * 2) * _Brightness * tex2D(_RimRamp, fixed2(saturate(dot(normalize(IN.viewDir), o.Normal * 1.3) / _SpecColor.a - .4), 0)).a * dot(saturate(_SpecColor.rgb), fixed3(0.3, 0.59, 0.11));
				o.Emission = reflcol * _Brightness * 0.8 * tex2D(_RimRamp, fixed2(saturate(dot(normalize(IN.viewDir), o.Normal * 1.0) / _SpecColor.a - .4), 0)).a * dot(saturate(_SpecColor.rgb), fixed3(0.3, 0.59, 0.11));				
				o.Emission += mainTex.rgb  *  mainTex.a;
			}
			
			inline fixed4 LightingCBlinnPhong (CustomSurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
			{
                lightDir = normalize(lightDir);

                viewDir = normalize(viewDir);
                
                half3 h = normalize (lightDir + viewDir);
                half diff = max (0, dot (s.Normal, lightDir));
                float nh = max (0, dot (s.Normal, h));
                float spec = pow (nh, s.Specular*128.0);

                half4 c;
				
				c.rgb = saturate(lerp( s.darkTex, s.mainTex, diff) * max(float3(0.2, 0.2, 0.2), _LightColor0.rgb)+ _LightColor0.rgb * s.GlossColor * spec) * atten  + s.Emission;

                //c.a = s.Alpha + _LightColor0.a * Luminance(s.GlossColor) * spec * atten;
                return c;
			}
			
		ENDCG
	}
	
	Fallback "Diffuse"
}