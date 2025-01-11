Shader "Custom/UniformLightingChladni"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (1, 1, 1, 1)
        _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
        _EmissionStrength ("Emission Strength", Float) = 1.0
        _LightPosition ("Light Position", Vector) = (0, 10, 0, 1) // init placeholder value this changes quickly..
        _LightColor ("Light Color", Color) = (1, 1, 1, 1)
        _LightStrength ("Light Strength", Float) = 1.0
        _Shininess ("Shininess", Float) = 32.0
        _A ("A", Float) = 0.5
        _B ("B", Float) = 0.5
        _N ("N", Float) = 0.5
        _M ("M", Float) = 0.5
        _DisplacementFactor ("DisplacementFactor", Float) = 15.0
        _LineThreshold ("Line Threshold", Float) = 3.0
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

            float4 _LightPosition;
            float4 _LightColor;
            float _LightStrength;
            float _Shininess;
            float _A, _B, _N, _M;
            float _DisplacementFactor;
            float _LineThreshold;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            float3 ComputeLighting(float3 normal, float3 worldPos) {
                // ambient then diffuse and spec
                float3 ambient = 0.1 * _LightColor.rgb;
                float3 lightDir = normalize(_LightPosition.xyz - worldPos);
                float diff = max(dot(normal, lightDir), 0.0);
                float3 diffuse = _LightColor.rgb * _LightStrength * diff;
                float3 viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                float3 reflectDir = reflect(-lightDir, normal);
                float specular = pow(max(dot(viewDir, reflectDir), 0.0), _Shininess);
                float3 specularColor = _LightColor.rgb * _LightStrength * specular;
                return ambient + diffuse + specularColor;
            }

            float ChladniEqn(float a, float b, float n, float m, float x, float y)
            {
                static const float PI = 3.14159265359f;
                return a * sin(PI * n * x) * sin(PI * m * y) + b * sin(PI * m * x) * sin(PI * n * y);
            }

            v2f vert(appdata v) {
                v2f o;
                float4 sumVec = v.vertex; // should push the vertex higher up
                float res = ChladniEqn(_A, _B, _N, _M, lerp(-3, 3, v.uv.x), lerp(-3, 3, v.uv.y));
                if (abs(res) < _LineThreshold) {
                    sumVec.y += _DisplacementFactor;
                    o.vertex = UnityObjectToClipPos(sumVec);
                }
                // o.vertex = UnityObjectToClipPos(sumVec);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                o.worldPos = mul(unity_ObjectToWorld, sumVec).xyz;
                return o;
            }

            float4 frag(v2f i) : SV_Target {
                float3 normal = normalize(i.normal);
                float3 finalColor = ComputeLighting(normal, i.worldPos);
                float res = ChladniEqn(_A, _B, _N, _M, lerp(-3, 3, i.uv.x), lerp(-3, 3, i.uv.y));
                if (abs(res) < _LineThreshold) {
                    return float4(1.0, 0.0, 0.0, 1.0);
                }
                float4 texColor = tex2D(_MainTex, i.uv);
                return float4(finalColor, 1.0) * texColor;//only the non covrd place will ahve a pattern.
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}