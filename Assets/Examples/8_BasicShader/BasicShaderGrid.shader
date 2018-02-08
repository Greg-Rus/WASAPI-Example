// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/BasicShaderGrid"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SpectrumTexture ("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			sampler2D _SpectrumTexture;

			// http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
			float3 HSVToRGB(float h, float s, float v)
			{
				return lerp(	float3( 1.0,1,1 ), clamp(( abs( frac(h + float3( 3.0, 2.0, 1.0 ) / 3.0 )
								* 6.0 - 3.0 ) - 1.0 ), 0.0, 1.0 ), s ) * v;
			}

			float SpectrumValue(int spectrumIndex) {
				int x = spectrumIndex % 64;
				int y = spectrumIndex / 64;
				return tex2D(_SpectrumTexture, float2(x/64.0, y/64.0)).r;
			}

			fixed4 frag (v2f i) : COLOR
			{
				int step = i.uv[0] * 50;
				step = step % 100;
				float brightness = SpectrumValue(step) * ((1.2 - sin(i.uv[1])) % 10);
				int shouldShine = (brightness + 0.8);
				return fixed4(HSVToRGB(i.uv[0], 0.0, brightness), 1.0);
			}
			ENDCG
		}
	}
}
