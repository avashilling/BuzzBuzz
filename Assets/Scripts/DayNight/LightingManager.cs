using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    [Header("Cycle Settings")]
    [SerializeField] public float TimeOfDay = 0f; // 0 = dawn
    public float maxTime = 180f; // total duration in seconds
    public bool loop = false;    // whether to loop or stop after sunset

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
            TimeOfDay += Time.deltaTime;

            // If we're done, stop or loop
            if (TimeOfDay >= maxTime)
            {
                if (loop)
                    TimeOfDay = 0f;
                else
                {
                    TimeOfDay = maxTime;
                    finished = true;
                }
            }
        }

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
