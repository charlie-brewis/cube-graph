using UnityEngine;

public class GPUGraph : MonoBehaviour {
    
    [SerializeField]
    ComputeShader computeShader;

    [SerializeField, Range(10, 200)]
    int resolution;

    const int DEFAULT_RESOLUTION = 100;

    [SerializeField]    
    FunctionLibrary.FunctionName functionKey;

    public enum TransitionMode { Cycle, Random }
    [SerializeField]
    TransitionMode transitionMode;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    float currDuration;

    bool transitioning;
    FunctionLibrary.FunctionName transitionFunctionFrom;

    // used to allocate GPU space for the positions
    ComputeBuffer positionsBuffer;

    // We use OnEnable instead of Awake as computeBuffers do not survive hot reloads, therefore we must call it each time the object is enabled
    void OnEnable() {
        // Constructor method of the compute buffer
        // param 2 is the size of each datum to be stored, we are storing Vector3 objects which are sets of 3 floats (floats are 4 bytes)
        positionsBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
    }

    void OnDisable() {
        positionsBuffer.Release(); 
        // As we wont use this instance again, we release it manually instead of waiting for the garbage collector
        positionsBuffer = null; }

    void Update() {
        currDuration += Time.deltaTime;
        if (transitioning) {
            if (currDuration >= transitionDuration) {
                currDuration -= transitionDuration;
                transitioning = false;
            }
        }
        else if (currDuration >= functionDuration) {
            currDuration -= functionDuration;
            transitioning = true;
            transitionFunctionFrom = functionKey;
            PickNextFunction();
        } }

    void PickNextFunction() {
        functionKey = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(functionKey) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(functionKey); }

}