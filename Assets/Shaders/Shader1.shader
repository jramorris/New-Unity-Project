Shader "Unlit/Shader1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StartColor ("Start Color", Color) = (1,1,1,1)
        _ColorA ("Y Color", Color) = (1,1,1,1)
        _ColorB ("X Color", Color) = (1,1,1,1)
        _Offset ("Grad Offset", Float) = 1
    }
    SubShader
    {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Pass
        {
            // Cull Back (default value)

            ZWrite Off // write to depth buffer (off for transparency) 
            ZTest LEqual // render pixels if </= (close to camera) than depth buffer

            Blend One One
            // multiply: Blend DstColor Zero
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _ColorA;
            float4 _ColorB;
            float _Offset;

            struct appdata // shit to grab from unity (Mesh Data)
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f // shit passed from vertex shader to fragment shader (fragment ~ pixel)
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float xOffset = cos(i.uv.y * 8 * 6.283185307179586) * .01;
                float lerpVal = cos( (i.uv.x + xOffset + _Time.y * .1) * 5 * 6.283185307179586) * 0.5 + .5;
                //lerpVal *= i.uv.x; 
                float4 outputColor = lerp(_ColorA, _ColorB, i.uv.x);

                //outputColor *= i.uv.x; 
                return outputColor * lerpVal;
            }
            ENDCG
        }
    }
}
