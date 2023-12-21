Shader "Graph/Point Surface" {

    SubShader {
        CGPROGRAM
        // Configures the shader compiler to generate a surface shader with standard lighting and full support for shadows
        #pragma surface ConfigureSurface Standard fullforwardshadows
        // Sets a minimum for the shaders target level and quality
        #pragma target 3.0

        struct Input {
            float3 worldPos;
        };

        // inout indicates the parameter is passed to the function and used in the output
        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface) {}
        ENDCG
    }

    FallBack "Diffuse"
}