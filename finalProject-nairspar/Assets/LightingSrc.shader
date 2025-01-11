Shader "Custom/LightingSrc"
{
    Properties
    {
        _Color ("Base Color", Color) = (1, 1, 1, 1)
        _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
        _EmissionStrength ("Emission Strength", Float) = 1.0
        _LightPosition ("Light Position", Vector) = (0, 10, 0, 1)
        _LightColor ("Light Color", Color) = (1, 1, 1, 1)
        _LightStrength ("Light Strength", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            float4 _Color;
            float4 _EmissionColor;
            float _EmissionStrength;
            float4 _LightPosition;
            float4 _LightColor;
            float _LightStrength;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                return o;
            }

            float4 frag(v2f i) : SV_Target {
                float4 color = _Color;
                color.rgb += _EmissionColor.rgb * _EmissionStrength;
                float3 lightDir = normalize(_LightPosition.xyz - i.pos.xyz);
                float diff = max(dot(i.normal, lightDir), 0.0);
                color.rgb += _LightColor.rgb * _LightStrength * diff;
                return color;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
