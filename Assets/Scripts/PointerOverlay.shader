Shader "Hidden/PointerOverlay"
{
    Properties
    {
        [MainTexture] _MainTex ("Texture", 2D) = "white" {}
        _PointerTexture("Pointer Overlay Texture", 2D) = "white" {}
        _Center("Pointer Overlay Center", Vector) = (0.5, 0.5, 0, 0)
        _Scale("Pointer Overlay Scale", Vector) = (1, 1, 0, 0)
    }
    SubShader
    {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            sampler2D _PointerTexture;
            float4 _PointerTexture_TexelSize;
            float4 _Center;
            float4 _Scale;

            fixed2 toPointerUVs(fixed2 screenUV)
            {
                fixed2 normalizedUVStep = _MainTex_TexelSize.xy / max(_MainTex_TexelSize.x, _MainTex_TexelSize.y);
                fixed2 pointerUV = (screenUV - _Center.xy) / normalizedUVStep * _Scale;
                pointerUV = (pointerUV + 0.5f);
                return pointerUV;
            }

            fixed4 applyPointerFilter(fixed2 screenUV)
            {
                fixed4 colM = tex2D(_MainTex, screenUV);
                fixed2 pointerUV = toPointerUVs(screenUV);

                if (pointerUV.x < 0.0f || pointerUV.x > 1.0f ||
                    pointerUV.y < 0.0f || pointerUV.y > 1.0f)
                {
                    return colM;
                }

                fixed4 invertFilter = tex2D(_PointerTexture, pointerUV);

                return fixed4(lerp(colM.rgb, 1 - colM.rgb, invertFilter.r), 1);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return applyPointerFilter(i.uv);
            }
            ENDCG
        }
    }
}
