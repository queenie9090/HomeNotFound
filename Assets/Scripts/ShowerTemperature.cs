using UnityEngine;

public class ShowerTemperature : MonoBehaviour
{
    [Header("Shower Settings")]
    public ParticleSystem waterParticles;

    private bool isWaterRunning = false;

    private float cooldownTime = 1.0f; // Wait 1 second between touches
    private float nextToggleTime = 0f;

    void Start()
    {
        if (waterParticles != null)
        {
            waterParticles.Stop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            // Check if enough time has passed since the last touch
            if (Time.time >= nextToggleTime)
            {
                ToggleWater();

                // Set the next allowed time to 1 second in the future
                nextToggleTime = Time.time + cooldownTime;
            }
        }
    }

    void ToggleWater()
    {
        isWaterRunning = !isWaterRunning;

        if (isWaterRunning)
        {
            waterParticles.Play();
            Debug.Log("Shower turned ON");
        }
        else
        {
            waterParticles.Stop();
            Debug.Log("Shower turned OFF");
        }
    }
}