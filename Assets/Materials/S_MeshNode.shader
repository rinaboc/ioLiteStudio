Shader "Custom/S_MeshNode"
{
    Properties
    {
        [MainTexture] _MainTex("Texture", 2D) = "white" {}
        [TintColor] _Color("Tint Color", Color) = (1,1,1,1)

    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }

        Pass
        {
            Blend One OneMinusSrcAlpha
            ZWrite On

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                float4 color      : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float3 worldPos   : TEXCOORD1;
                float4 color      : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float mask =
                    step(0.0, IN.uv.x) *
                    step(IN.uv.x, 1.0) *
                    step(0.0, IN.uv.y) *
                    step(IN.uv.y, 1.0);

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                col.a *= mask;
                col *= IN.color;
                col.rgb *= col.a;

                return col;
            }
            ENDHLSL
        }
    }
}
