using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI; // Needed for Slider and Toggle

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("UI References")]
    public TMP_Dropdown movementDropdown;
    public TMP_Dropdown turningDropdown;
    public Slider volumeSlider;       // Drag your Volume Slider here
    public Toggle vignetteToggle;     // Drag your Vignette Toggle here
    public Toggle seatedToggle;       // Drag your Seated Mode Toggle here

    [Header("Audio")]
    public AudioSource moveSound;
    public AudioSource turnSound;

    private bool _isReady = false;

    System.Collections.IEnumerator EnableAudioAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        _isReady = true;
    }

    void Start()
    {
        // --- LOAD SAVED DATA INTO UI ---

        if (movementDropdown != null)
            movementDropdown.value = PlayerPrefs.GetInt("MovementType", 0);

        if (turningDropdown != null)
            turningDropdown.value = PlayerPrefs.GetInt("TurningType", 0);

        // Load Volume (Default to 0.75f)
        if (volumeSlider != null)
        {
            float vol = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
            volumeSlider.value = vol;
            AudioListener.volume = vol; // Apply volume to menu music/sounds
        }

        // Load Vignette (Default to 1/On)
        if (vignetteToggle != null)
            vignetteToggle.isOn = PlayerPrefs.GetInt("VignetteEnabled", 1) == 1;

        // Load Seated Mode (Default to 0/Off)
        if (seatedToggle != null)
            seatedToggle.isOn = PlayerPrefs.GetInt("SeatedMode", 0) == 1;

        StartCoroutine(EnableAudioAfterDelay());
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        PlayerPrefs.Save(); // Final hard save when closing
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // --- SAVING LOGIC (No Applier needed in Main Menu) ---

    public void OnMovementDropdownChanged(int index)
    {
        if (_isReady && moveSound != null) moveSound.Play();
        PlayerPrefs.SetInt("MovementType", index);
    }

    public void OnTurningDropdownChanged(int index)
    {
        if (_isReady && turnSound != null) turnSound.Play();
        PlayerPrefs.SetInt("TurningType", index);
    }

    public void OnVignetteToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("VignetteEnabled", isOn ? 1 : 0);
    }

    public void OnVolumeSliderChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void OnSeatedModeToggle(bool isOn)
    {
        PlayerPrefs.SetInt("SeatedMode", isOn ? 1 : 0);
    }
}