using UnityEngine;
using TMPro;
using System.Collections;

public class Level2Dialogue : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ExecuteIntro());
    }

    IEnumerator ExecuteIntro()
    {
        yield return new WaitForSeconds(1.0f);
        DialogueManager.Instance.ShowDialogue("Street full of people...");

        yield return new WaitForSeconds(5.5f);
        DialogueManager.Instance.ShowDialogue("Great, just what I need right now...");

        yield return new WaitForSeconds(5.8f);
        DialogueManager.Instance.ShowDialogue("Find some food to eat first, I'm starving...");

    }
}