Shader "Unlit/MooShader"
{
    Properties {
        _Color ("Base Color", Color) = (1, 1, 1, 1)
        _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
        _EmissionStrength ("Emission Strength", Float) = 1.0
        _LightPosition ("Light Position", Vector) = (0, 10, 0, 1) // init placeholder value this changes quickly..
        _LightColor ("Light Color", Color) = (1, 1, 1, 1)
        _LightStrength ("Light Strength", Float) = 1.0
        _Shininess ("Shininess", Float) = 100.0
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

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

            float4 _LightPosition;
            float4 _LightColor;
            float _LightStrength;
            float _Shininess;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            float3 ComputeLighting(float3 normal, float3 worldPos) {
                // ambient then diffuse and spec
                float3 ambient = 0.5 * _LightColor.rgb;
                float3 lightDir = normalize(_LightPosition.xyz - worldPos);
                float diff = max(dot(normal, lightDir), 0.0);
                float3 diffuse = _LightColor.rgb * _LightStrength * diff;
                float3 viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                float3 reflectDir = reflect(-lightDir, normal);
                float specular = pow(max(dot(viewDir, reflectDir), 0.0), _Shininess);
                float3 specularColor = _LightColor.rgb * _LightStrength * specular;
                return ambient + diffuse + specularColor;
            }

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target {
                float3 normal = normalize(i.normal);
                float3 finalColor = ComputeLighting(normal, i.worldPos);
                float4 texColor = tex2D(_MainTex, i.uv);
                return float4(finalColor, 1.0) * texColor;
            }
            ENDCG
        }
    }
}
