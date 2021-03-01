Shader "Invertex/TriangleCalcExample2" {

	SubShader {
		Tags { "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom

			#include "UnityCG.cginc"

			struct v2g {
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
				float3 worldPos : TEXCOORD1;
				fixed4 color : COLOR;
			};

			struct g2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2g vert(appdata_full v) {
				v2g o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.color = v.color;

				return o;
			}

			[maxvertexcount(3)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> tristream) {
				g2f o;

				float2 edgeA = normalize(IN[1].worldPos.xy - IN[0].worldPos.xy);
				float2 uvEdgeA = normalize(IN[1].uv - IN[0].uv);
				float2 angle = normalize(edgeA - uvEdgeA);

				for (int i = 0; i < 3; i++) {
					o.pos = IN[i].vertex;
					o.uv = IN[i].uv;
					o.color = IN[i].color; // fixed4(angle, 1, 1);
					tristream.Append(o);
				}
			}

			fixed4 frag(g2f i) : SV_Target {
				return  i.color;
			}
			ENDCG
		}
	}
}