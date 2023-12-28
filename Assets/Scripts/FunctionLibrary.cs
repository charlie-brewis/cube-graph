using UnityEngine;
using static UnityEngine.Mathf;

// Static as this class will work as a static library for mathematical functions, instead of an object type
public static class FunctionLibrary {

    // This is essentially an abstract method to define a shape/type (Function) for all our wave functions
    public delegate Vector3 Function (float u, float v, float y);

    // Enum FunctionName type defines the valid names allowed within functions
    // I.e., FunctionName is the key to functions value - Treated as strings
    public enum FunctionName {Wave, DoubleWave, Ripple, Sphere, Torus, FuzzyWave}
    static Function[] functions = {Wave, DoubleWave, Ripple, Sphere, Torus, FuzzyWave};

    public static Function GetFunction (FunctionName name) {
        return functions[(int)name];
    }

    // f(x, t) = sin(pi(x + t))
    public static Vector3 Wave(float u, float v, float t) {
        Vector3 outPoint;
        outPoint.x = u;
        outPoint.y = Sin(PI * (u + v + t));
        outPoint.z = v;
        return outPoint;
    }

    public static Vector3 DoubleWave(float u, float v, float t) {
        Vector3 outPoint;
        outPoint.x = u;
        outPoint.y = Sin(PI * (u + t * 0.5f));
        outPoint.y += 0.5f * Sin(2f * PI * (v + t));
        outPoint.y += Sin(PI * (u + v + 0.25f * t));
        // Garuntees wave stays within the -1-1 range
        // Also note using multiplication of constant fractional values over division for performance
        outPoint.y *= (1f / 2.5f);
        outPoint.z = v;
        return outPoint;
    }

    public static Vector3 Ripple (float u, float v, float t) {
        float distFromCent = Sqrt(u * u + v * v);
        Vector3 outPoint;
        outPoint.x = u;
        outPoint.y = Sin(PI * (4f * distFromCent - t)) / (1f + 10f * distFromCent);
        outPoint.z = v;
        return outPoint;
    }

    // Think of this sphere as multiple semi-circles rotated around the y-axis
    public static Vector3 Sphere (float u, float v, float t) {
        // Scale r by time
        // float r = 0.5f + 0.5f * Sin(PI * t);
        // Verical Bands
        // float r = 0.9f + .1f * Sin(8f * PI * u);
        // Horizontal Bands
        // float r = 0.9f + .1f * Sin(8f * PI * v);
        // Twisting Bands (both)
        float r = .9f + .1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);

        Vector3 outPoint;
        outPoint.x = s * Sin(PI * u);
        outPoint.y = r * Sin(PI * 0.5f * v);
        outPoint.z = s * Cos(PI * u);
        return outPoint;
    } 

    public static Vector3 Torus(float u, float v, float t) {
        // float torusRadius = .75f;
        // float ringRadius = .25f;
        float torusRadius = .7f + .1f * Sin(PI * (6f * u + .5f * t));
        float ringRadius = .15f + .05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        float s = torusRadius + ringRadius * Cos(PI * v);

        Vector3 outPoint;
        outPoint.x = s * Sin(PI * u);
        outPoint.y = ringRadius * Sin(PI * v);
        outPoint.z = s * Cos(PI * u);
        return outPoint;
    }

    public static Vector3 FuzzyWave(float u, float v, float t) {
        // int waveCount = 5;
        // Random rand = new Random();
        Vector3 outPoint;
        outPoint.x = u; 
        outPoint.z = v;
        // outPoint.y = 0;
        // for (int i = 0; i < waveCount; i++){
        //     outPoint.y += Sin(PI * (u + t * Random.Range(0f, 1f))) * Random.Range(0f, 1f);
        // }
        outPoint.y = Sin(PI * (u + t )) * Random.Range(.5f, 1f) + Random.Range(.5f, 1f);

        return outPoint;
    }

}
