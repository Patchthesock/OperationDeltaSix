using UnityEngine;

/// <summary>
/// This parallax is suitable for the menu.
/// Specify the direction of movement and intensity.
/// </summary>
public class ConstantParallax : UIParallax {

    // The intensity with which all layers will move.
    public float TotalIntensity = 1;

    // Direction of movement
    public Vector2 Direction2D = Vector2.one;

    void Start() {

        if (ParallaxLayers.Length <= 0) {
            Debug.LogWarning(" The problem of UI Parallax initialization. Parallax layers are not found. Please make sure component UIParallax is configured correctly.");
            _isInitialize = false;
        }

    }

    void Update() {
        Parallaxing(Direction2D * TotalIntensity / 100);
    }

}
