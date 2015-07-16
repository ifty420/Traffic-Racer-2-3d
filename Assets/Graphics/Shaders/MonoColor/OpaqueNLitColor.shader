Shader "MonoColor/OpaqueNLitColor"
{
    Properties 
    {
        _Color ("Main Color",Color) = (1,1,1,1)
    }
    
    SubShader 
    {
        Pass { 
            CGPROGRAM
            #pragma vertex vert 
            #pragma fragment frag
            
            float4 _Color;

            float4 vert(float4 vertexPos : POSITION) : SV_POSITION 
            {
                return mul(UNITY_MATRIX_MVP, vertexPos); 
            }

            float4 frag(void) : COLOR 
            {
                return _Color; 
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}