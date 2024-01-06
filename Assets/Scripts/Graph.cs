using UnityEngine;

public class Graph : MonoBehaviour {

    [SerializeField]
    Transform pointPrefab;

    // Range defines the range of values the field can take
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

    // Type transform is the position, rotation, and scale of the game object
    Transform[] points;

    float currDuration;

    bool transitioning;
    FunctionLibrary.FunctionName transitionFunctionFrom;

    void Awake() { 
        // Transform point = Instantiate(pointPrefab);
        // // This method transforms the Vector3 position by (1, 0, 0)
        // point.localPosition = Vector3.right;

        // // We don't need to redefine point's type as it has already been defined
        // point = Instantiate(pointPrefab);
        // point.localPosition = Vector3.right * 2f;

        // defines the space between points to keep graph within the domain -1-1
        // resolution = DEFAULT_RESOLUTION;
        float step = 2f / resolution;
        var scale = Vector3.one * step;

        // Objects like arrays must be initialized. Here we initialize a size of resolution squared to fill the x and z coordinates
        points = new Transform[resolution * resolution];

        for (int i = 0; i < points.Length; i++) {
            // C# can tell this is inside a loop and so this definition is allowed, also adds to index at points
            Transform point = points[i] = Instantiate(pointPrefab);
            // rescales the points to fit within -1-1 range
            point.localScale = scale;
            //! I dont know what this is
            point.SetParent(transform, false);
        }
    }

    void Update() {
        currDuration += Time.deltaTime;
        if (transitioning) {
            if (currDuration >= transitionDuration) {
                currDuration -= transitionDuration;
                transitioning = false;
            }
        }
        else if (currDuration >= functionDuration) {
            // Sub rather than reset to keep timing consistent even when desynced
            currDuration -= functionDuration;
            transitioning = true;
            transitionFunctionFrom = functionKey;
            PickNextFunction();
        }
        if (transitioning) {
            UpdateFunctionTransition();
        }
        else {
            UpdateFunction();
        }
    }

    void PickNextFunction() {
        functionKey = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(functionKey) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(functionKey);
    }

    void UpdateFunction() {
        FunctionLibrary.Function func = FunctionLibrary.GetFunction(functionKey);
        float time = Time.time;
        float step = 2f / resolution;
        //! Problem when drawing z = v as it relies on resolution incorrectly somewhere
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z += 1; 
                // v only needs to be recalculated when z changes, therefore it is defined here
                v = (z + 0.5f) * step - 1f;  
            } 
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = func(u, v, time);
        }
    }

    void UpdateFunctionTransition() {
        FunctionLibrary.Function 
            from = FunctionLibrary.GetFunction(transitionFunctionFrom),
            to = FunctionLibrary.GetFunction(functionKey);

        float progress = currDuration / transitionDuration;
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z += 1; 
                v = (z + 0.5f) * step - 1f;  
            } 
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
        }
    }
}