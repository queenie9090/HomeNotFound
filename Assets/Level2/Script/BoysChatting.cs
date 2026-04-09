using UnityEngine;
using System.Collections;

public class BoysChatting : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Dialogue Sets")]
    public AudioClip[] normalClips;

    public bool loopSequence = true;

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        do
        {
            AudioClip[] clipsToUse;

            clipsToUse = normalClips;

            if (clipsToUse == null || clipsToUse.Length == 0)
                yield break;

            for (int i = 0; i < clipsToUse.Length; i++)
            {
                if (clipsToUse[i] == null) continue;

                audioSource.clip = clipsToUse[i];
                audioSource.volume = 1.0f;
                audioSource.Play();

                yield return new WaitForSeconds(clipsToUse[i].length + 4f);
            }

        } while (loopSequence);
    }
}