Shader "Graph/Point Surface GPU" {

    Properties {
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
    }

    SubShader {
        CGPROGRAM
        // Configures the shader compiler to generate a surface shader with standard lighting and full support for shadows, as well as a custom shadow pass
        #pragma surface ConfigureSurface Standard fullforwardshadows addshadow
        // Indicates that the shader has to invoke a ConfigureProcedural per vertex, and that all scaling will be uniform
        #pragma instancing_options assumeuniformscaling  procedural:ConfigureProcedural
        // forces the editor to buffer and compile this shader ahead of time asynchronously, instead of using a default shader while rendering
        #pragma editor_sync_compilation
        // Sets a minimum for the shaders target level and quality
        #pragma target 4.5

        #include "PointGPU.hlsl"

        struct Input {
            float3 worldPos;
        };

        float _Smoothness;

        // inout indicates the parameter is passed to the function and used in the output
        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface) {
            // This works as both Albedo and worldPos have the shape of (a, b, c)
            // i.e., (x, y, z) maps directly to (r, g, b)
            // Note that we have to * 0.5 + 0.5 to fit within the range 0-1 as negative color isnt real
            // Saturate simply clamps it between 0 and 1
            surface.Albedo = saturate(input.worldPos * 0.5 + 0.5);
            surface.Smoothness = _Smoothness;
        }
        ENDCG
    }

    FallBack "Diffuse"
}