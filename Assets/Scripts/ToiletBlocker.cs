using UnityEngine;

public class ToiletBlocker : MonoBehaviour
{
    public Animator npcAnim;

    public AudioSource idleSound;
    public AudioSource hitSound;

    private bool hasTriggered = false;

    void Start()
    {
        // Make sure idle sound is playing
        if (idleSound != null && !idleSound.isPlaying)
        {
            idleSound.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!NpcBoneDetector.npcDistracted && !hasTriggered)
        {
            hasTriggered = true;

            Debug.Log("NPC hits player!");

            // Stop idle sound
            if (idleSound != null)
            {
                idleSound.Stop();
            }

            // Play hit sound
            if (hitSound != null)
            {
                hitSound.Play();
            }

            // Play animation
            if (npcAnim != null)
            {
                npcAnim.SetTrigger("Hit");
            }
        }
    }
}