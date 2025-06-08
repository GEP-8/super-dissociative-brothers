Shader "Custom/QuadrantSharedVision"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Quadrant ("Quadrant Index (1~4)", Float) = 1
        _CharPos ("Character Pos (UV)", Vector) = (0.5, 0.5, 0, 0)
        _SharedRadius ("Shared Radius", Float) = 0.15
        _SplitMode ("Split Mode (1~4)", Float) = 4
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Quadrant;
            float2 _CharPos;
            float _SharedRadius;
            float _SplitMode;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            bool InMyArea(float2 uv, float quadrant, float splitMode)
            {
                if (splitMode == 1) // 1인: 전체
                    return true;
                if (splitMode == 2) // 2인: 좌/우
                {
                    if (quadrant == 1) return uv.x <= 0.5;
                    if (quadrant == 2) return uv.x > 0.5;
                }
                if (splitMode == 3) // 3인: 좌상, 우상, 하단 전체
                {
                    if (quadrant == 1) return uv.x <= 0.5 && uv.y >= 0.5;
                    if (quadrant == 2) return uv.x > 0.5 && uv.y >= 0.5;
                    if (quadrant == 3) return uv.y < 0.5;
                }
                if (splitMode == 4) // 4인: 4분할
                {
                    if (quadrant == 1) return uv.x <= 0.5 && uv.y >= 0.5;
                    if (quadrant == 2) return uv.x > 0.5 && uv.y >= 0.5;
                    if (quadrant == 3) return uv.x <= 0.5 && uv.y < 0.5;
                    if (quadrant == 4) return uv.x > 0.5 && uv.y < 0.5;
                }
                return false;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float2 toChar = uv - _CharPos;
                float dist = length(toChar);

                // 원형 공유 시야 영역이면 항상 보임
                if (dist < _SharedRadius)
                {
                    return tex2D(_MainTex, uv);
                }

                // 자신의 분할 영역이 아니면 어둡게 처리
                if (!InMyArea(uv, _Quadrant, _SplitMode))
                {
                    return fixed4(0, 0, 0, 1); // 마스킹 처리
                }

                // 자신의 시야 영역은 정상 표시
                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
