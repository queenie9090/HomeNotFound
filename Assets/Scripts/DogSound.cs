using UnityEngine;
using UnityEngine.AI;

public class DogSound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip barkingSound;
    public AudioClip pantingSound;

    public Transform player;
    public float detectionRange = 10f;
    public float stopRange = 15f;

    private bool isChasing = false;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // CHASE
        if (distance < detectionRange)
        {
            if (!isChasing)
            {
                PlayBark();
                isChasing = true;
            }
        }
        // STOP CHASE
        else if (distance > stopRange)
        {
            if (isChasing)
            {
                PlayPanting();
                isChasing = false;
            }
        }
    }

    void PlayBark()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = barkingSound;
        audioSource.Play();
    }

    void PlayPanting()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = pantingSound;
        audioSource.Play();
    }
}