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
        float xStep = 2f / resolution;
        var scale = Vector3.one * xStep;
        // Initialise the position as (0, 0, 0)
        var position = Vector3.zero;

        // Objects like arrays must be initialized
        points = new Transform[resolution];

        for (int i = 0; i < points.Length; i++) {
            // C# can tell this is inside a loop and so this definition is allowed
            Transform point = Instantiate(pointPrefab);
            // Adding the point to the array of points
            points[i] = point;
            position.x = (i + 0.5f) * xStep - 1f;

            point.localPosition = position;
            // rescales the points to fit within -1-1 range
            point.localScale = scale;

            //! I dont know what this is
            point.SetParent(transform, false);
        }
    }

    void Update() {
        float time = Time.time;
        FunctionLibrary.Function func = FunctionLibrary.GetFunction(functionKey);
        for (int i = 0; i < points.Length; i++) {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            position.y = func(position.x, time);
            point.localPosition = position;
        }
    }
}