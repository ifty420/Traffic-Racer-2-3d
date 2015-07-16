Shader "Crossfade" { 
Properties
{
_tex0 ("Texture1", 2D) = "white" {}
_tex1 ("Texture2", 2D) = "white" {}
}
 
SubShader
{
Pass
{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
 
sampler2D _tex0;
sampler2D _tex1;
 
struct v2f {
float4 pos : POSITION;
float4 color : COLOR0;
float4 fragPos : COLOR1;
float2  uv : TEXCOORD0;
};
 
float4 _tex0_ST;
 
v2f vert (appdata_base v)
{
v2f o;
o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
o.fragPos = o.pos;
o.uv = TRANSFORM_TEX (v.texcoord, _tex0);
o.color = float4 (1.0, 1.0, 1.0, 1);
return o;
}

// I dont really have idea whats happening here..its a bit modified version from the original shader
half4 frag (v2f i) : COLOR
{
float2 q = i.uv / float2(1,1);
float3 oricol = tex2D (_tex0,float2(q.y,q.x)).xyz;
float3 col = tex2D (_tex1,float2(q.y,q.x)).xyz;
col = lerp(col,oricol,  clamp(q.x,0.0,1.0)); // ??
return float4(col,1);
}
ENDCG
}
}
FallBack "VertexLit"
}