using UnityEngine;
using static UnityEngine.Mathf;

// Static as this class will work as a static library for mathematical functions, instead of an object type
public static class FunctionLibrary {

    // This is essentially an abstract method to define a shape/type (Function) for all our wave functions
    public delegate float Function (float x, float y);

    public static Function GetFunction (int index) {
        if (index == 0) {
            return Wave;
        } 
        else if (index == 1) {
            return DoubleWave;
        } 
        else {
            return Ripple;
        }
    }

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

    public static float Ripple (float x, float t) {
        float distFromXCent = Abs(x);
        float y = Sin(PI * (4f * distFromXCent - t)) / (1f + 10f * distFromXCent);
        return y;
    }

}
