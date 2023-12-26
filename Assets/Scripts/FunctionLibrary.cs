using UnityEngine;

// Static as this class will work as a static library for mathematical functions, instead of an object type
public static class FunctionLibrary {

    // f(x, t) = sin(pi(x + t))
    public static float Wave(float x, float t) {
        return (float)Mathf.Sin(Mathf.PI * (x + t));
    }

}
