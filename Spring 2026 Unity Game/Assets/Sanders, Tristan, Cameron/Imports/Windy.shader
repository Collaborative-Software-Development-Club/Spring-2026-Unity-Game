Shader "Spring2026/Particles/Windy"
{
    Properties
    {
        [MainTexture] _BaseMap("Particle Texture", 2D) = "white" {}
        [MainColor] _BaseColor("Tint", Color) = (0.85, 0.95, 1.0, 0.7)

        [Header(Wind Motion)]
        _WindDirection("Wind Direction (XYZ)", Vector) = (1, 0, 0, 0)
        _WindStrength("Wind Strength", Range(0.0, 3.0)) = 0.35
        _WindSpeed("Wind Speed", Range(0.0, 10.0)) = 2.0
        _GustStrength("Gust Strength", Range(0.0, 2.0)) = 0.25
        _GustFrequency("Gust Frequency", Range(0.0, 10.0)) = 1.2
        _VerticalInfluence("Vertical Influence", Range(0.0, 3.0)) = 0.75

        [Header(Particle Controls)]
        _SoftAlpha("Soft Alpha", Range(0.0, 2.0)) = 1.0
        [Toggle] _UseVertexColor("Use Particle Vertex Color", Float) = 1

        [Header(Trail Support)]
        [Toggle] _UseTrailMode("Enable Trail Mode", Float) = 0
        _TrailFadePower("Trail Fade Power", Range(0.1, 8.0)) = 1.5
        _TrailUVScrollX("Trail UV Scroll X", Range(-5.0, 5.0)) = 0.0
        _TrailUVScrollY("Trail UV Scroll Y", Range(-5.0, 5.0)) = 0.0
        _TrailWindInfluence("Trail Wind Influence", Range(0.0, 2.0)) = 1.0

        [HideInInspector] _MainTex("Particle Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderPipeline" = "UniversalPipeline"
            "UniversalMaterialType" = "Unlit"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                half4 color : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _BaseMap_ST;
                float4 _WindDirection;
                float _WindStrength;
                float _WindSpeed;
                float _GustStrength;
                float _GustFrequency;
                float _VerticalInfluence;
                float _SoftAlpha;
                float _UseVertexColor;
                float _UseTrailMode;
                float _TrailFadePower;
                float _TrailUVScrollX;
                float _TrailUVScrollY;
                float _TrailWindInfluence;
            CBUFFER_END

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                float3 windDir = _WindDirection.xyz;
                float windLen = length(windDir);
                windDir = (windLen > 0.0001) ? (windDir / windLen) : float3(1.0, 0.0, 0.0);
                float timePhase = _Time.y * _WindSpeed;
                float gust = sin(timePhase * _GustFrequency + input.positionOS.y * 2.1 + input.positionOS.x * 1.7) * _GustStrength;
                float wave = sin(timePhase + input.positionOS.y * 3.0 + input.positionOS.x * 2.0);
                float sway = (wave * _WindStrength + gust) * saturate(abs(input.positionOS.y) * _VerticalInfluence);
                float trailMask = lerp(1.0, saturate(input.uv.x) * _TrailWindInfluence, saturate(_UseTrailMode));
                sway *= trailMask;

                float3 displacedOS = input.positionOS.xyz + windDir * sway;
                output.positionCS = TransformObjectToHClip(displacedOS);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.color = input.color;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;
                uv += float2(_TrailUVScrollX, _TrailUVScrollY) * _Time.y * saturate(_UseTrailMode);

                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
                half4 particleColor = lerp(half4(1, 1, 1, 1), input.color, _UseVertexColor);
                half4 color = texColor * _BaseColor * particleColor;
                float trailFade = lerp(1.0, pow(saturate(1.0 - input.uv.x), _TrailFadePower), saturate(_UseTrailMode));
                color.a *= trailFade;
                color.a = saturate(color.a * _SoftAlpha);
                return color;
            }
            ENDHLSL
        }
    }
}
