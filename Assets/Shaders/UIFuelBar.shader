Shader "Sprite Unlit Master"
{
    Properties
    {
        [NoScaleOffset]Texture2D_C9F1AED5("ColorText", 2D) = "white" {}
        _fuelFillAmount("FuelFillAmount", Range(0, 1)) = 1
        Vector1_2506B2E("BlinkBelowAmount", Range(0, 1)) = 0.25
        Vector1_2BA60ACE("BlinkRate", Float) = 5
        [NoScaleOffset]_MainTex("BaseTex", 2D) = "white" {}
        [HDR]_intensity("Intensity", Color) = (2, 2, 2, 1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent+0"
        }
        
        Pass
        {
            // Name: <None>
            Tags 
            { 
                // LightMode: <None>
            }
           
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Off
            ZTest [unity_GUIZTestMode]
            ZWrite Off
            // ColorMask: <None>

            Stencil

            {
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }
            ColorMask [_ColorMask]
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
        
            // Keywords
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            // GraphKeywords: <None>
            
            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define SHADERPASS_SPRITEUNLIT
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float _fuelFillAmount;
            float Vector1_2506B2E;
            float Vector1_2BA60ACE;
            float4 _intensity;
            CBUFFER_END
            TEXTURE2D(Texture2D_C9F1AED5); SAMPLER(samplerTexture2D_C9F1AED5); float4 Texture2D_C9F1AED5_TexelSize;
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            SAMPLER(_SampleTexture2D_799C9340_Sampler_3_Linear_Repeat);
            SAMPLER(_SampleTexture2D_A637FD17_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            
            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            void Unity_Comparison_Greater_float(float A, float B, out float Out)
            {
                Out = A > B ? 1 : 0;
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_Sine_float(float In, out float Out)
            {
                Out = sin(In);
            }
            
            void Unity_Absolute_float(float In, out float Out)
            {
                Out = abs(In);
            }
            
            void Unity_Branch_float(float Predicate, float True, float False, out float Out)
            {
                Out = Predicate ? True : False;
            }
        
            // Graph Vertex
            // GraphVertex: <None>
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
                float3 TimeParameters;
            };
            
            struct SurfaceDescription
            {
                float4 Color;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _SampleTexture2D_799C9340_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv0.xy);
                float _SampleTexture2D_799C9340_R_4 = _SampleTexture2D_799C9340_RGBA_0.r;
                float _SampleTexture2D_799C9340_G_5 = _SampleTexture2D_799C9340_RGBA_0.g;
                float _SampleTexture2D_799C9340_B_6 = _SampleTexture2D_799C9340_RGBA_0.b;
                float _SampleTexture2D_799C9340_A_7 = _SampleTexture2D_799C9340_RGBA_0.a;
                float4 _Property_A109A269_Out_0 = _intensity;
                float _Property_B73B248E_Out_0 = _fuelFillAmount;
                float4 _SampleTexture2D_A637FD17_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_C9F1AED5, samplerTexture2D_C9F1AED5, (_Property_B73B248E_Out_0.xx));
                float _SampleTexture2D_A637FD17_R_4 = _SampleTexture2D_A637FD17_RGBA_0.r;
                float _SampleTexture2D_A637FD17_G_5 = _SampleTexture2D_A637FD17_RGBA_0.g;
                float _SampleTexture2D_A637FD17_B_6 = _SampleTexture2D_A637FD17_RGBA_0.b;
                float _SampleTexture2D_A637FD17_A_7 = _SampleTexture2D_A637FD17_RGBA_0.a;
                float4 _Multiply_D7F288D6_Out_2;
                Unity_Multiply_float(_Property_A109A269_Out_0, _SampleTexture2D_A637FD17_RGBA_0, _Multiply_D7F288D6_Out_2);
                float _Property_D4591E86_Out_0 = Vector1_2506B2E;
                float _Comparison_443ADB20_Out_2;
                Unity_Comparison_Greater_float(_Property_B73B248E_Out_0, _Property_D4591E86_Out_0, _Comparison_443ADB20_Out_2);
                float _Property_1E2D1215_Out_0 = Vector1_2BA60ACE;
                float _Multiply_3D92AF05_Out_2;
                Unity_Multiply_float(IN.TimeParameters.x, _Property_1E2D1215_Out_0, _Multiply_3D92AF05_Out_2);
                float _Sine_7D6A6972_Out_1;
                Unity_Sine_float(_Multiply_3D92AF05_Out_2, _Sine_7D6A6972_Out_1);
                float _Absolute_2B957B33_Out_1;
                Unity_Absolute_float(_Sine_7D6A6972_Out_1, _Absolute_2B957B33_Out_1);
                float _Branch_27023BD3_Out_3;
                Unity_Branch_float(_Comparison_443ADB20_Out_2, 1, _Absolute_2B957B33_Out_1, _Branch_27023BD3_Out_3);
                float4 _Multiply_14EB5B9E_Out_2;
                Unity_Multiply_float(_Multiply_D7F288D6_Out_2, (_Branch_27023BD3_Out_3.xxxx), _Multiply_14EB5B9E_Out_2);
                float4 _Multiply_BF6B3611_Out_2;
                Unity_Multiply_float(_SampleTexture2D_799C9340_RGBA_0, _Multiply_14EB5B9E_Out_2, _Multiply_BF6B3611_Out_2);
                surface.Color = _Multiply_BF6B3611_Out_2;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 color : COLOR;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 texCoord0;
                float4 color;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float4 interp00 : TEXCOORD0;
                float4 interp01 : TEXCOORD1;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                output.interp01.xyzw = input.color;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                output.color = input.interp01.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            
            
            
            
                output.uv0 =                         input.texCoord0;
                output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteUnlitPass.hlsl"
        
            ENDHLSL
        }
        
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}
