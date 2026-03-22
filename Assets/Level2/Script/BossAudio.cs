using UnityEngine;
using System.Collections;

public class AudioBoss : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;

    private bool playerNearby = false;
    private Coroutine playRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            if (playRoutine == null)
                playRoutine = StartCoroutine(PlayRandomClips());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            if (playRoutine != null)
            {
                StopCoroutine(playRoutine);
                playRoutine = null;
            }

            audioSource.Stop();
        }
    }

    IEnumerator PlayRandomClips()
    {
        while (playerNearby)
        {
            if (clips.Length == 0) yield break;

            AudioClip clip = clips[Random.Range(0, clips.Length)];
            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length + Random.Range(2f, 6f));
        }
    }
}