using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class MainMenuSaveManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Drag your Resume Button here")]
    public Button resumeButton;

    void Start()
    {
        int highestLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        Debug.Log($"[Save System] Save file loaded. Highest level is: {highestLevel}");

        if (highestLevel == 1)
        {
            resumeButton.interactable = false;
        }
        else
        {
            resumeButton.interactable = true; 
        }
    }

    public void StartNewGame()
    {
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Level 1");
    }

    public void ResumeGame()
    {
        int highestLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        SceneManager.LoadScene("Level " + highestLevel);
    }
}