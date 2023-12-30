using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour {

    [SerializeField]
    TextMeshProUGUI display;

    public enum DisplayMode { FPS, MS }
    [SerializeField]
    DisplayMode displayMode = DisplayMode.FPS;

    [SerializeField, Range(0.1f, 2f)]
    float sampleDuration = 1f;

    int numFrames;
    float duration, lowestDuration = float.MaxValue, highestDuration;

    void Update() {
        
        float frameDuration = Time.unscaledDeltaTime;
        numFrames++;
        duration += frameDuration;

        if (frameDuration < lowestDuration) {
            lowestDuration = frameDuration;
        }
        if (frameDuration > highestDuration) {
            highestDuration = frameDuration;
        }
        
        if (duration >= sampleDuration) {
            if (displayMode == DisplayMode.FPS) {
                display.SetText("FPS\n{0:0}\n{1:0}\n{2:0}", 1f / lowestDuration, numFrames / duration, 1f/ highestDuration);
            }
            else {
                display.SetText("MS\n{0:1}\n{1:1}\n{2:1}", 1000f * lowestDuration, 1000f * duration / numFrames , 1000f * highestDuration);
            }   
            numFrames = 0;
            duration = 0f;
            lowestDuration = float.MaxValue;
            highestDuration = 0;
        }

    }
}