using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2EndTrigger : MonoBehaviour
{
    public Level2Manager level2Manager;

    [Header("Save Settings")]
    [Tooltip("The level number the player JUST unlocked. If this is the end of Level 1, type 2 here.")]
    public int levelToUnlock = 3;

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

    void OnTriggerEnter(Collider other)
    {
        var gm = Level2Manager.Instance;

    if (other.CompareTag("Player") && gm.currentState != Level2Manager.GameState.LevelComplete)
        {
            DialogueManager.Instance.ShowDialogue("Don't wanna go there yet...");
        }
    else if(other.CompareTag("Player") && gm.currentState == Level2Manager.GameState.LevelComplete)
        {
            //change scene to level 3 - main menu
            //SceneManager.LoadScene("Level 3");
            SaveAndExitToMenu();
        }
    }
}
