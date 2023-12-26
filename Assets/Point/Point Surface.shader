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
            // This works as both Albedo and worldPos have the shape of (a, b, c)
            // i.e., (x, y, z) maps directly to (r, g, b)
            // Note that we have to * 0.5 + 0.5 to fit within the range 0-1 as negative color isnt real
            // surface.Albedo = input.worldPos * 0.5 + 0.5;
            // Z (b) is always the same here so we can discard it
            surface.Albedo.xy = input.worldPos.rg * 0.5 + 0.5;
            surface.Smoothness = _Smoothness;
        }
        ENDCG
    }

    FallBack "Diffuse"
}