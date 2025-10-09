using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    [Header("Cycle Settings")]
    [Tooltip("Current time of day (0 = dawn)")]
    public float TimeOfDay = 0f;

    [Tooltip("Total duration of a full day in seconds")]
    public float maxTime = 180f;

    [Tooltip("Loop the day cycle or stop at sunset")]
    public bool loop = false;

    [Header("Optional Settings")]
    [Tooltip("Freeze time progression, but allow manual adjustment of TimeOfDay")]
    public bool freezeTime = false; // default false

    private bool finished = false;

    private void Start()
    {
        if (maxTime == 0)
            maxTime = 180f;
    }

    private void Update()
    {
        if (Preset == null || finished)
            return;

        if (Application.isPlaying)
        {
            // Only advance time automatically if freezeTime is false
            if (!freezeTime)
            {
                TimeOfDay += Time.deltaTime;

                if (TimeOfDay >= maxTime)
                {
                    if (loop)
                        TimeOfDay = 0f;
                    else
                    {
                        TimeOfDay = maxTime;
                        SceneManager.LoadScene("End");
                    }
                }
            }
        }

        // Clamp time and update lighting
        TimeOfDay = Mathf.Clamp(TimeOfDay, 0f, maxTime);
        float timePercent = Mathf.Clamp01(TimeOfDay / maxTime);
        UpdateLighting(timePercent);
    }

    private void UpdateLighting(float timePercent)
    {
        // Update environment colors
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        // Update sun rotation and color
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            float sunAngle = Mathf.Lerp(-10f, 190f, timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3(sunAngle, -170f, 0f));
        }
    }

    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
