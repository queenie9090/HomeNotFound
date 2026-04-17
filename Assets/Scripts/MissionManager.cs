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

    [Header("Save Settings")]
    [Tooltip("The level number the player JUST unlocked. If this is the end of Level 1, type 2 here.")]
    public int levelToUnlock = 2;

    [Tooltip("The exact name of your Main Menu scene")]
    public string mainMenuSceneName = "MainMenu";

    public void SaveAndExitToMenu()
    {
        int currentSavedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // 2. Only overwrite the save if this new level is HIGHER.
        // (This stops a player who is re-playing Level 1 from accidentally deleting their Level 3 save!)
        if (levelToUnlock > currentSavedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", levelToUnlock);
            PlayerPrefs.Save();

            Debug.Log($"<color=green>Progress Saved! Player unlocked Level {levelToUnlock}</color>");
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }

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
            Invoke("PlayFinalSequence", 6.0f);
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
                SaveAndExitToMenu();

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