using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class contains information about the parallax layer
/// </summary>

[System.Serializable]
public class ParallaxElement {

    // Image source for the parallax layer
    public RawImage Image;

    // You can select the required range
    [Range(0, 100)]
    public float Intensity;

}
