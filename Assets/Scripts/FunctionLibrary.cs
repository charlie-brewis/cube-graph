using UnityEngine;
using static UnityEngine.Mathf;

// Static as this class will work as a static library for mathematical functions, instead of an object type
public static class FunctionLibrary {

    // f(x, t) = sin(pi(x + t))
    public static float Wave(float x, float t) {
        return Sin(PI * (x + t));
    }

    public static float DoubleWave(float x, float t) {
        float y = Sin(PI * (x + t * 0.5f));
        y += Sin(2f * PI * (x + t)) * 0.5f;
        // Return garuntees wave stays within the -1-1 range
        // Also note using multiplication of constant fractional values over division for performance
        return y * (2f / 3f);
    }

}
