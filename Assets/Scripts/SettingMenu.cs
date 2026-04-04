using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public void SetSnapTurn(bool isOn)
    {
        // Save the value (1 for true, 0 for false)
        PlayerPrefs.SetInt("SnapTurnEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save(); // Forces Unity to write to disk
    }

    public void SetTeleportation(bool isOn)
    {
        PlayerPrefs.SetInt("TeleportEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}