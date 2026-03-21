using UnityEngine;

public class PlaySoundOnHit : MonoBehaviour
{
    public AudioSource hitSound;
    public float velocityThreshold = 0.5f;

    private float startTime;
    private float ignoreDuration = 2.0f; // Seconds to stay quiet at start

    void Start()
    {
        startTime = Time.time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if enough time has passed since the game started
        if (Time.time - startTime < ignoreDuration) return;

        if (collision.relativeVelocity.magnitude > velocityThreshold)
        {
            hitSound.Play();
        }
    }
}