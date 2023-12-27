using UnityEngine;
using static UnityEngine.Mathf;

// Static as this class will work as a static library for mathematical functions, instead of an object type
public static class FunctionLibrary {

    // This is essentially an abstract method to define a shape/type (Function) for all our wave functions
    public delegate float Function (float x, float z, float y);

    // Enum FunctionName type defines the valid names allowed within functions
    // I.e., FunctionName is the key to functions value - Treated as strings
    public enum FunctionName {Wave, DoubleWave, Ripple}
    static Function[] functions = {Wave, DoubleWave, Ripple};

    public static Function GetFunction (FunctionName name) {
        return functions[(int)name];
    }

    // f(x, t) = sin(pi(x + t))
    public static float Wave(float x, float z, float t) {
        return Sin(PI * (x + z + t));
    }

    public static float DoubleWave(float x, float z, float t) {
        float y = Sin(PI * (x + t * 0.5f));
        y += 0.5f * Sin(2f * PI * (z + t));
        y += Sin(PI * (x + z + 0.25f * t));
        // Return garuntees wave stays within the -1-1 range
        // Also note using multiplication of constant fractional values over division for performance
        return y * (1f / 2.5f);
    }

    public static float Ripple (float x, float z, float t) {
        float distFromCent = Sqrt(x * x + z * z);
        float y = Sin(PI * (4f * distFromCent - t)) / (1f + 10f * distFromCent);
        return y;
    }

}
