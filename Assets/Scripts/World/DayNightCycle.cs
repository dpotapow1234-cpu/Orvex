using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time")]
    [Range(0, 24)]
    public float timeOfDay = 12f;

    public float dayLengthInMinutes = 20f;
    public bool runTime = true;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Environment")]
    public Gradient ambientColor;
    public Gradient fogColor;
    public Material skybox;

    [Header("Stars")]
    public GameObject stars;
    [Range(0, 1)]
    public float nightStarsThreshold = 0.15f;

    private void Update()
    {
        if (runTime)
        {
            timeOfDay += 24f / (dayLengthInMinutes * 60f)
                        * Time.deltaTime;

            if (timeOfDay >= 24f)
                timeOfDay -= 24f;
        }

        UpdateLighting();
    }

    private void UpdateLighting()
    {
        float normalizedTime = timeOfDay / 24f;

        // Солнце
        float sunRotation = (normalizedTime * 360f) - 90f;

        if (sun != null)
        {
            sun.transform.rotation =
                Quaternion.Euler(sunRotation, 170f, 0f);

            sun.color = sunColor.Evaluate(normalizedTime);
            sun.intensity = sunIntensity.Evaluate(normalizedTime);
        }

        // Окружающее освещение
        RenderSettings.ambientLight =
            ambientColor.Evaluate(normalizedTime);

        RenderSettings.fogColor =
            fogColor.Evaluate(normalizedTime);

        // Звёзды
        if (stars != null)
        {
            float night = 1f - sunIntensity.Evaluate(normalizedTime);
            stars.SetActive(night > nightStarsThreshold);
        }

        // Небо
        if (skybox != null)
        {
            RenderSettings.skybox = skybox;
        }
    }
}

