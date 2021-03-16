// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/DimensionCamera"
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
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityStandardUtils.cginc"

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 screen_uv : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                // get screen position from vertex world position
                float4 clipSpacePosition = UnityObjectToClipPos(v.vertex);
                o.vertex = clipSpacePosition;
                o.uv = v.uv;
                // Adding o.vertex.w as a plus 1, and scaling range 
                // back to 0-1 with 0.5
                o.screen_uv = float3((o.vertex.xy + o.vertex.w) * 0.5, o.vertex.w);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //divide out z component which is equivalent to o.vertex.w
                float2 uv = i.screen_uv.xy / i.screen_uv.z;

                //one minus flips image back upright
                #if UNITY_UV_STARTS_AT_TOP
                    uv.y = 1 - uv.y;
                #endif

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
