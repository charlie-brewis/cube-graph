using UnityEngine;

public class Graph : MonoBehaviour {

    [SerializeField]
    Transform pointPrefab;

    void Awake() { 
        // Transform point = Instantiate(pointPrefab);
        // // This method transforms the Vector3 position by (1, 0, 0)
        // point.localPosition = Vector3.right;

        // // We don't need to redefine point's type as it has already been defined
        // point = Instantiate(pointPrefab);
        // point.localPosition = Vector3.right * 2f;

        // int i = 0;
        // while (i++ < 10) {
        var scale = Vector3.one / 5f;
        // Initialise the position as (0, 0, 0)
        var position = Vector3.zero;
        for (int i = 0; i < 10; i++) {
            // C# can tell this is inside a loop and so this definition is allowed
            Transform point = Instantiate(pointPrefab);
            position.x = (i + 0.5f) / 5f - 1f;
            point.localPosition = position;
            // rescales the points to fit within -1-1 range
            point.localScale = scale;
        }
    }
}