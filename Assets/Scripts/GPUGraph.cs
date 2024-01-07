using UnityEngine;

public class GPUGraph : MonoBehaviour {

    [SerializeField]
    ComputeShader computeShader;
    static readonly int 
        positionsId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time");

    [SerializeField]
    Material material;
    [SerializeField]
    Mesh mesh;


    [SerializeField, Range(10, 1000)]
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
        } 
        
        UpdateFunctionOnGPU();
    }

    void PickNextFunction() {
        functionKey = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(functionKey) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(functionKey); }

    void UpdateFunctionOnGPU() {
        float step = 2f / resolution;
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);

        // the 0 is the index of our kernel as compute shaders can have multiple kernels
        computeShader.SetBuffer(0, positionsId, positionsBuffer);

        // This runs our kernel, again the first param is the kernel index while the other 3 is the amount of groups to run split into dimensions
        // Our group size is fixed to 8*8, we need 8 / resolution groups in the xy dimensions, rounded up
        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(0, groups, groups, 1);

        // Define the attributes in the material
        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);

        // the 0 is the sub-mesh index for when a mesh has multiple parts
        // Bounds lets Unity know the location of the object being rendered - in our case we are bounding the entire graph rather than a single point
        // This is what allows fulcrum culling on the GPU side 
        // The last parameter tells us how many instances should be drawn, which in our case is the number of elements in the positions buffer
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);
    }

}