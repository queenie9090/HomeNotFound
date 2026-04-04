using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;

    [Header("Real-Time Connection")]
    public MovementSettingApplier applier;

    [Header("UI References")]
    public TMP_Dropdown movementDropdown;
    public TMP_Dropdown turningDropdown;
    public Slider volumeSlider;
    public Toggle vignetteToggle;
    public Toggle seatedToggle;

    [Header("Audio")]
    public AudioSource moveSound;
    public AudioSource turnSound;

    private bool isPaused = false;
    private bool _isReady = false;

    System.Collections.IEnumerator EnableAudioAfterDelay()
    {
        yield return new UnityEngine.WaitForSeconds(0.1f);
        _isReady = true;
    }

    void Start()
    {
        Debug.Log("<color=cyan>[PauseManager]</color> Start: Loading Saved Preferences...");

        if (movementDropdown != null)
            movementDropdown.value = PlayerPrefs.GetInt("MovementType", 0);

        if (turningDropdown != null)
            turningDropdown.value = PlayerPrefs.GetInt("TurningType", 0);

        if (volumeSlider != null)
            volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);

        if (vignetteToggle != null)
            vignetteToggle.isOn = PlayerPrefs.GetInt("VignetteEnabled", 1) == 1;

        if (seatedToggle != null)
            seatedToggle.isOn = PlayerPrefs.GetInt("SeatedMode", 0) == 1;

        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);

        StartCoroutine(EnableAudioAfterDelay());
    }

    public void OpenSettings()
    {
        pauseMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
        PlayerPrefs.Save();
        Debug.Log("<color=green>[PauseManager]</color> Settings Saved to Disk.");
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (!isPaused)
        {
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
        }
        else
        {
            pauseMenuPanel.SetActive(true);
        }

        Time.timeScale = isPaused ? 0f : 1f;
        Debug.Log("<color=yellow>[PauseManager]</color> Pause Toggled. IsPaused: " + isPaused);
    }

    public void ResetLevel()
    {
        Time.timeScale = 1f;
        Debug.Log("<color=orange>[PauseManager]</color> Resetting Level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // --- SETTING FUNCTIONS WITH DEBUG LOGS ---

    public void OnMovementDropdownChanged(int index)
    {
        if (_isReady && moveSound != null) moveSound.Play();
        PlayerPrefs.SetInt("MovementType", index);
        Debug.Log("<color=white>[PauseManager]</color> Movement Changed to Index: " + index);
        if (applier != null) applier.ApplySettings();
    }

    public void OnTurningDropdownChanged(int index)
    {
        if (_isReady && turnSound != null) turnSound.Play();
        PlayerPrefs.SetInt("TurningType", index);
        Debug.Log("<color=white>[PauseManager]</color> Turning Changed to Index: " + index + " (0=Snap, 1=Smooth)");

        if (applier != null)
        {
            applier.ApplySettings();
        }
        else
        {
            Debug.LogWarning("<color=red>[PauseManager]</color> Applier is MISSING in the Inspector!");
        }
    }

    public void OnVignetteToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("VignetteEnabled", isOn ? 1 : 0);
        Debug.Log("<color=white>[PauseManager]</color> Vignette Toggle: " + isOn);
        if (applier != null) applier.ApplySettings();
    }

    public void OnVolumeSliderChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        // No log here to avoid spamming the console while sliding
    }

    public void OnSeatedModeToggle(bool isOn)
    {
        PlayerPrefs.SetInt("SeatedMode", isOn ? 1 : 0);
        Debug.Log("<color=white>[PauseManager]</color> Seated Mode Toggle: " + isOn);
        if (applier != null) applier.ApplySettings();
    }
}