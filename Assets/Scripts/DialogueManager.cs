using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    // Static instance so other scripts can find it easily
    public static DialogueManager Instance;

    [Header("UI References")]
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI dialogueText;

    [Header("Settings")]
    public float fadeSpeed = 2f;
    public float displayTime = 2.5f;

    void Awake()
    {
        // Simple Singleton for the current scene
        Instance = this;

        // Ensure it starts invisible
        if (canvasGroup != null) canvasGroup.alpha = 0;
    }

    // Call this for single lines (like the Clothing)
    public void ShowDialogue(string message)
    {
        StopAllCoroutines();
        StartCoroutine(FadeSequence(message));
    }

    // Call this for sequences (like the Dog)
    public void ShowDialogueSequence(string[] lines)
    {
        StopAllCoroutines();
        StartCoroutine(SequenceRoutine(lines));
    }

    private IEnumerator SequenceRoutine(string[] lines)
    {
        foreach (string line in lines)
        {
            yield return StartCoroutine(FadeSequence(line));
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator FadeSequence(string message)
    {
        dialogueText.text = message;

        // Fade In
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(displayTime);

        // Fade Out
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }
}