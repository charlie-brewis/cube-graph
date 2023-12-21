Shader "Graph/Point Surface" {

    Properties {
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
    }

    SubShader {
        CGPROGRAM
        // Configures the shader compiler to generate a surface shader with standard lighting and full support for shadows
        #pragma surface ConfigureSurface Standard fullforwardshadows
        // Sets a minimum for the shaders target level and quality
        #pragma target 3.0

        struct Input {
            float3 worldPos;
        };

        float _Smoothness;
        // inout indicates the parameter is passed to the function and used in the output
        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface) {
            surface.Smoothness = _Smoothness;
        }
        ENDCG
    }

    FallBack "Diffuse"
}