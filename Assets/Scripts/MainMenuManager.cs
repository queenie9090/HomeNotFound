using UnityEngine;
using UnityEngine.SceneManagement; // Required for switching scenes

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // You can use the scene name "Level1" or the build index 1
        SceneManager.LoadScene("Level 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}