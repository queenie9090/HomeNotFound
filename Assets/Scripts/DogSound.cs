using UnityEngine;

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

        if (distance < detectionRange)
        {
            if (!isChasing)
            {
                PlaySound(barkingSound);
                isChasing = true;
            }
        }
        
        else if (distance > stopRange || isChasing == false)
        {
            if (isChasing || !audioSource.isPlaying || audioSource.clip != pantingSound)
            {
                PlaySound(pantingSound);
                isChasing = false;
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        if (audioSource.clip == clip && audioSource.isPlaying) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}