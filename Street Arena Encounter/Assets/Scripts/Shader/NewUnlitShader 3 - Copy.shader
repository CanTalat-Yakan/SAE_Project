Shader "Unlit/NewUnlitShader 3"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};
			
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float3 worldPos : POSITION1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = mul(unity_ObjectToWorld, float4(v.normal, 0)).xyz;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				i.normal = normalize(i.normal);

				fixed4 col = tex2D(_MainTex, i.uv);

				//float3 viewDirection = normalize(_WorldSpaceCameraPos - input.Pos);
				//float4 fresnel = 2 * pow(1 + dot(viewDirection, i.normal), 10);

				return col;
			}
			ENDCG
		}
	}
}
