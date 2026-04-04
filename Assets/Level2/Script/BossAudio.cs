using UnityEngine;
using System.Collections;

public class AudioBoss : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Dialogue Sets")]
    public AudioClip[] normalClips;
    public AudioClip[] cursingClips;

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
            AudioClip[] clipsToUse;

            if (CursingControl.Instance != null && CursingControl.Instance.allowCursing)
                clipsToUse = cursingClips;
            else
                clipsToUse = normalClips;

            if (clipsToUse == null || clipsToUse.Length == 0)
                yield break;

            AudioClip clip = clipsToUse[Random.Range(0, clipsToUse.Length)];

            audioSource.clip = clip;
            audioSource.volume = 1.0f;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length + Random.Range(4f, 6f));
        }
    }
}