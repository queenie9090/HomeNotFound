using UnityEngine;

public class ShowerTemperature : MonoBehaviour
{
    [Header("Shower Settings")]
    public ParticleSystem waterParticles;

    [Header("NPC Settings")]
    public GameObject npc;
    public DogAiInLv3 dog;
    public Transform stayPoint1;
    public Door door; 

    private bool isWaterRunning = false;

    private float cooldownTime = 1.0f; // Wait 1 second between touches
    private float nextToggleTime = 0f;

    private bool hasCompletedShowerTask = false;

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

                if (npc != null)
                {
                    npc.SetActive(false);
                }

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

            if (!hasCompletedShowerTask)
            {
                if (JournalInLv3.Instance != null)
                {
                    JournalInLv3.Instance.CompleteTask(2);
                }
                hasCompletedShowerTask = true;
            }

            if (dog != null)
            {
                dog.GoToStayPoint(stayPoint1);
            }

            if (door != null)
            {
                door.EnableDogTrigger();
            }
        }
        else
        {
            waterParticles.Stop();
            Debug.Log("Shower turned OFF");
        }

    }
}