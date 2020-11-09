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
				float3 normal = normalize(i.normal);

				fixed4 col = tex2D(_MainTex, i.uv);
				float4 ambient = unity_AmbientSky * 0.75;

				float d = dot(normal, normalize(_WorldSpaceLightPos0));
				float4 diffuse = saturate(normalize(d))* _LightColor0;

				float3 viewDirection = normalize(_WorldSpaceCameraPos - i.worldPos);
				float3 halfVec = viewDirection + _WorldSpaceLightPos0;
				float d2 = saturate(dot(normalize(halfVec), normal));
				d2 = pow(d2, 30);
				d2 /= 2.25;
				float4 specular = d * d2 * _LightColor0;

				return (specular + diffuse + ambient) * col;
			}
			ENDCG
		}
	}
}
