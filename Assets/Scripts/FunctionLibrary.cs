using UnityEngine;
using static UnityEngine.Mathf;

// Static as this class will work as a static library for mathematical functions, instead of an object type
public static class FunctionLibrary {

    // This is essentially an abstract method to define a shape/type (Function) for all our wave functions
    public delegate Vector3 Function (float u, float v, float y);

    // Enum FunctionName type defines the valid names allowed within functions
    // I.e., FunctionName is the key to functions value - Treated as strings
    public enum FunctionName {Wave, DoubleWave, Ripple, Sphere}
    static Function[] functions = {Wave, DoubleWave, Ripple, Sphere};

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

    public static Vector3 Sphere (float u, float v, float t) {
        float radius = Cos(0.5f * PI * v);
        Vector3 outPoint;
        outPoint.x = radius * Sin(PI * u);
        outPoint.y = Sin(PI * 0.5f * v);
        outPoint.z = radius * Cos(PI * u);
        return outPoint;
    } 

}
