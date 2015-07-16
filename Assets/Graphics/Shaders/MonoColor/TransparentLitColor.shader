Shader "MonoColor/TransparentLitColor"
{
    Properties {
      _Color ("MainColor", Color) = (1,1,1,0.5)
    }
    SubShader {
      Tags { "RenderType" = "Transparent"  "Queue" = "Transparent" }
      
      Cull Off
      
      CGPROGRAM
      #pragma surface surf Lambert alpha
      
      float4 _Color;

      struct Input {
          float2 uv_MainTex;
      };
      
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = _Color.rgb;
          o.Alpha = _Color.a;
      }
      
      ENDCG
    }
    fallback "MonoColor/OpaqueNLitColor"
}