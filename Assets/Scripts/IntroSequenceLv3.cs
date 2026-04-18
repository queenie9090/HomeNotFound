using UnityEngine;
using System.Collections;

public class IntroSequenceLv3 : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ExecuteIntro());
    }

    IEnumerator ExecuteIntro()
    {
        yield return new WaitForSeconds(1.0f);
        DialogueManager.Instance.ShowDialogue("It's evening.");

        // 2. Misson 1
        yield return new WaitForSeconds(3.0f);
        DialogueManager.Instance.ShowDialogue("I think I should find a place to sleep.");

        yield return new WaitForSeconds(3.0f);
        DialogueManager.Instance.ShowDialogue("hmmmmm......");

        // 3. Misson 2
        yield return new WaitForSeconds(3f);
        DialogueManager.Instance.ShowDialogue("Toilet looks like a good place.");

        yield return new WaitForSeconds(3f);
        DialogueManager.Instance.ShowDialogue("But I think I need to get rid of that damn janitor first");

    }
}