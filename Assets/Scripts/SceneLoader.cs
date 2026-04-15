using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Type the exact name of your main menu scene in the Inspector
    public string mainMenuSceneName = "MainMenu";

    // Public function that the button will look for
    public void LoadMainMenu()
    {
        Debug.Log("Returning to main menu...");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
