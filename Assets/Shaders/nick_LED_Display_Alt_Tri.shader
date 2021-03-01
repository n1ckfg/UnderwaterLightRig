Shader "Nick/LED_Display_Alt_Tri" {

	Properties{
		_SpriteTex("Base (RGB)", 2D) = "white" {}
		_Size("Size", Range(0, 3)) = 0.5
		_Brightness("Brightness", Range(1, 200)) = 10.0
	}

	SubShader {
		Tags {
			"IgnoreProjector" = "True"
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Back

			CGPROGRAM
			#pragma require geometry
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom

			#include "UnityCG.cginc"

			float _Size;
			float _Brightness;
			sampler2D _SpriteTex;
			float4 _SpriteTex_ST;

			struct appdata {
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2g {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			struct g2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2g vert(appdata_full v) {
				v2g o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = mul(unity_ObjectToWorld, v.vertex);
				//o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.color = v.color;

				return o;
			}

			[maxvertexcount(3)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> tristream) {
				g2f o;

				float3 up = float3(0, 1, 0);
				float3 look = _WorldSpaceCameraPos - IN[0].vertex;
				look.y = 0;
				look = normalize(look);
				float3 right = cross(up, look);

				float halfS = 0.5f * _Size;

				float4 v[4];
				v[0] = float4(IN[0].vertex + halfS * right - halfS * up, 1.0f);
				v[1] = float4(IN[0].vertex + halfS * right + halfS * up, 1.0f);
				v[2] = float4(IN[0].vertex - halfS * right - halfS * up, 1.0f);
				v[3] = float4(IN[0].vertex - halfS * right + halfS * up, 1.0f);

				float4x4 vp;
				#if UNITY_VERSION >= 560 
				vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);
				#else 
				#if UNITY_SHADER_NO_UPGRADE 
				vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);
				#endif
				#endif

				o.pos = mul(vp, v[0]);
				o.uv = float2(1.0f, 0.0f);
				o.color = IN[0].color; 
				tristream.Append(o);

				o.pos = mul(vp, v[1]);
				o.uv = float2(1.0f, 1.0f);
				o.color = IN[0].color;
				tristream.Append(o);

				o.pos = mul(vp, v[2]);
				o.uv = float2(0.0f, 0.0f);
				o.color = IN[0].color;
				tristream.Append(o);

				o.pos = mul(vp, v[3]);
				o.uv = float2(0.0f, 1.0f);
				o.color = IN[0].color;
				tristream.Append(o);
			}

			fixed4 frag(g2f i) : SV_Target {
				return  i.color * tex2D(_SpriteTex, i.uv) * _Brightness;
			}
			ENDCG
		}
	}
}