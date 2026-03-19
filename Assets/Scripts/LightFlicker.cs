using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    public Light lightSource;
    public float minIntensity = 0.2f;
    public float maxIntensity = 1.0f;

    void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light>();

        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // Randomize the light intensity
            lightSource.intensity = Random.Range(minIntensity, maxIntensity);

            // Wait for a short, random burst of time
            yield return new WaitForSeconds(Random.Range(1.0f, 10.0f));
        }
    }
}