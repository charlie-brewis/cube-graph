using UnityEngine;

public class GPUGraph : MonoBehaviour {

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