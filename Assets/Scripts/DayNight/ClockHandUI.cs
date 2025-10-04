using UnityEngine;

public class ClockHandUI : MonoBehaviour
{
    [Header("References")]
    public LightingManager lightingManager; // assign your LightingManager

    [Header("Rotation Settings")]
    public float rotationAmount = 180f; // degrees to rotate over the day

    private RectTransform rectTransform;
    private float initialZRotation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
            Debug.LogError("ClockHandUI must be on a UI element with RectTransform!");

        // Record initial rotation on awake
        initialZRotation = rectTransform.localEulerAngles.z;
    }

    private void Update()
    {
        if (lightingManager == null) return;

        // Calculate the proportion of the day completed
        float timePercent = Mathf.Clamp01(lightingManager.TimeOfDay / lightingManager.maxTime);

        // Add rotationAmount relative to the initial rotation
        float currentZ = initialZRotation - rotationAmount * timePercent;

        rectTransform.localRotation = Quaternion.Euler(0f, 0f, currentZ);
    }
}
