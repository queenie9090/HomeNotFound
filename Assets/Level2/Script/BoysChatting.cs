using UnityEngine;
using System.Collections;

public class BoysChatting : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;

    public bool loopSequence = true;

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        do
        {
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i] == null) continue;

                audioSource.clip = clips[i];
                audioSource.Play();

                yield return new WaitForSeconds(clips[i].length + 4f);
            }

        } while (loopSequence);
    }
}