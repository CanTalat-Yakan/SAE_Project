Shader "AAAAAA"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex VS
            #pragma fragment PS
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct VS_OUT
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            VS_OUT VS (appdata v)
            {
                VS_OUT o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.pos);

                return o;
            }

            fixed4 PS (VS_OUT i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
