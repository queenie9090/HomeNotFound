using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Mission Status")]
    private bool clothDone = false;
    private bool dogDone = false;

    [Header("Dialogue Messages")]
    [TextArea(2, 5)]
    public string warningMessage = "I can't go out yet... I still have things to do.";

    [TextArea(2, 5)]
    public string[] finalSequence = {
        "Maybe I should head to the street...",
        "I need to earn some money to buy more food."
    };

    void Awake()
    {
        Instance = this;
    }

    // --- TASK COMPLETION ---

    public void MarkClothComplete()
    {
        clothDone = true;
        CheckFinalDialogue();
    }

    public void MarkDogComplete()
    {
        dogDone = true;
        CheckFinalDialogue();
    }

    private void CheckFinalDialogue()
    {
        // Only play the "I should go to the street" lines 
        // the moment BOTH tasks are finished.
        if (clothDone && dogDone)
        {
            Invoke("PlayFinalSequence", 4.0f);
        }
    }

    // --- EXIT LOGIC ---

    private void OnTriggerEnter(Collider other)
    {
        // Check if Player enters the exit region
        if (other.CompareTag("MainCamera") || other.CompareTag("Player"))
        {
            if (clothDone && dogDone)
            {
                // Everything is done! Go to Level 2
                SceneManager.LoadScene("Level 2");
            }
            else
            {
                // Not done! Remind the player
                DialogueManager.Instance.ShowDialogue(warningMessage);
            }
        }
    }

    private void PlayFinalSequence()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.ShowDialogueSequence(finalSequence);
        }
    }
}