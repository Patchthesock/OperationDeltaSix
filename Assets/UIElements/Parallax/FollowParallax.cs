using UnityEngine;

/// <summary>
/// This parallax is suitable for working with moving objects.
/// Inertia depends on the movement of the target.
/// </summary>
public class FollowParallax : UIParallax {

    // The object behind the movements of which will move parallax
    public GameObject Target;

    // Old position target object
    private Vector3 _targetOldPosition;

    void Start()
    {
        if (Target == null) Debug.LogWarning("Warning. Parallax will be static. Target object not found. Please make sure component UIParallax is configured correctly.");
        if (ParallaxLayers.Length > 0) return;
        Debug.LogWarning(" The problem of UI Parallax initialization. Parallax layers are not found. Please make sure component UIParallax is configured correctly.");
        _isInitialize = false;
    }

	void Update ()
    {
        if (Target.transform.position == _targetOldPosition) return; // Check move object
        Parallaxing(Target.transform.position - _targetOldPosition); // Get direction movement
        _targetOldPosition = Target.transform.position; // Update Old Target Position
    }
}
