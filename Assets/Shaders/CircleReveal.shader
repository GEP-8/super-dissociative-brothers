Shader "UI/CircleReveal"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,1)
        _Center("Center", Vector) = (0.5, 0.5, 0, 0)
        _Cutoff("Cutoff", Range(0, 1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Overlay" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            float2 _Center;
            float _Cutoff;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = distance(i.uv, _Center);
                if (dist < _Cutoff)
                    discard;

                return _Color;
            }
            ENDCG
        }
    }
}
