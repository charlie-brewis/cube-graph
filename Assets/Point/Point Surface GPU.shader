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
        // Sets a minimum for the shaders target level and quality
        #pragma target 4.5

        struct Input {
            float3 worldPos;
        };

        float _Smoothness;

        // Only do this for shader variants specifically compiled for procedural drawing
        #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
        // This one is of type StructuredBuffer as we only have to read from it
        StructuredBuffer<float3> _Positions;
        #endif

        float _Step;

        void ConfigureProcedural () {
            #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
            // Get the position at the index currently being drawn
            float3 position = _Positions[unity_InstanceID];
            // Insitialises a 4x4 object-world transformation matrix with all values set to 0
            // https://catlikecoding.com/unity/tutorials/basics/compute-shaders/procedural-drawing/transformation-matrix.png
            unity_ObjectToWorld = 0.0;
            // Assigns position column values to the position 
            unity_ObjectToWorld._m03_m13_m23_m33 = float4(position, 1.0);
            // Assigns the scale cells to step
            unity_ObjectToWorld._m00_m11_m22 = _Step;
            #endif
        }

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