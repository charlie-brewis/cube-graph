using UnityEngine;

public class Graph : MonoBehaviour {

    [SerializeField]
    Transform pointPrefab;

    // Range defines the range of values the field can take
    [SerializeField, Range(10, 100)]
    int resolution = 10;

    [SerializeField]    
    FunctionLibrary.FunctionName functionKey;

    // Type transform is the position, rotation, and scale of the game object
    Transform[] points;

    void Awake() { 
        // Transform point = Instantiate(pointPrefab);
        // // This method transforms the Vector3 position by (1, 0, 0)
        // point.localPosition = Vector3.right;

        // // We don't need to redefine point's type as it has already been defined
        // point = Instantiate(pointPrefab);
        // point.localPosition = Vector3.right * 2f;

        // defines the space between points to keep graph within the domain -1-1
        float step = 2f / resolution;
        var scale = Vector3.one * step;

        // Objects like arrays must be initialized. Here we initialize a size of resolution squared to fill the x and z coordinates
        points = new Transform[resolution * resolution];

        for (int i = 0; i < points.Length; i++) {
            // C# can tell this is inside a loop and so this definition is allowed
            Transform point = Instantiate(pointPrefab);
            // Adding the point to the array of points
            points[i] = point;
            // rescales the points to fit within -1-1 range
            point.localScale = scale;
            //! I dont know what this is
            point.SetParent(transform, false);
        }
    }

    void Update() {
        FunctionLibrary.Function func = FunctionLibrary.GetFunction(functionKey);
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z++; 
                // v only needs to be recalculated when z changes, therefore it is defined here
                v = (z + 0.5f) * step - 1f;  
            } 
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = func(u, v, time);
        }

    }
}