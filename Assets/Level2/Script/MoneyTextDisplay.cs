using UnityEngine;
using TMPro;

public class MoneyTextDisplay : MonoBehaviour
{
    [Header("Manager Reference")]
    public Level2Manager manager;

    [Header("Text Display")]
    public TMP_Text moneyText;

    private string prefix = "RM ";

    public void RefreshText()
    {
        if (manager == null)
        {
            Debug.LogError("<color=red>[MoneyText]</color> Level2Manager is missing! Please drag it into the inspector.");
            return;
        }

        if (moneyText == null)
        {
            Debug.LogWarning("<color=yellow>[MoneyText]</color> You forgot to assign the TextMeshPro object in the inspector!");
            return;
        }

        int currentCash = manager.playerMoney;
        moneyText.text = prefix + currentCash.ToString();

        Debug.Log($"<color=lime>[MoneyText]</color> Updated text to read: {moneyText.text}");
    }
}