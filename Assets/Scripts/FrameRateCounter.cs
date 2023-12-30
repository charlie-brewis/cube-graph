using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI display;

    [SerializeField, Range(0.1f, 2f)]
    float sampleDuration = 1f;

    int numFrames;
    float duration;

    void Update() {
        
        float frameDuration = Time.unscaledDeltaTime;
        numFrames++;
        duration += frameDuration;

        if (duration >= sampleDuration) {
            display.SetText("FPS\n{0:0}\n000\n000", numFrames / duration);
            numFrames = 0;
            duration = 0f;
        }

    }
}