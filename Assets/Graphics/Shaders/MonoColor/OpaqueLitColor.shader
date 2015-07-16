Shader "MonoColor/OpaqueLitColor"
{
    Properties {
      _Color ("MainColor", Color) = (1,1,1,1)
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      
      CGPROGRAM
      #pragma surface surf WrapLambert
      
      float4 _Color;

      half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half atten) {
          half NdotL = dot (s.Normal, lightDir);
          half diff = NdotL * 0.5 + 0.5;
          half4 c;
          c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
          c.a = s.Alpha;
          return c;
      }

      struct Input {
          float2 uv_MainTex;
      };
      
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = _Color;
      }
      ENDCG
    }
    fallback "MonoColor/OpaqueNLitColor"
}