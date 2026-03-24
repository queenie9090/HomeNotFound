using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class XRWeighingScale : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI displayText;

    [Header("Settings")]
    public string itemTag = "RecycleItem";

    private List<RecycleItem> items = new List<RecycleItem>();
    private int totalValue = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(itemTag))
        {
            RecycleItem item = other.GetComponent<RecycleItem>();

            if (item != null && !items.Contains(item))
            {
                items.Add(item);
                UpdateTotal();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(itemTag))
        {
            RecycleItem item = other.GetComponent<RecycleItem>();

            if (item != null && items.Contains(item))
            {
                items.Remove(item);
                UpdateTotal();
            }
        }
    }

    void UpdateTotal()
    {
        totalValue = 0;

        foreach (var item in items)
        {
            if (item != null)
                totalValue += item.value;
        }

        displayText.text = "RM " + totalValue;
    }

    // Called by VR button
    public void SellItems()
    {
        if (totalValue <= 0) return;

        Level2Manager.Instance.AddMoney(totalValue);

        foreach (var item in items)
        {
            if (item != null)
                Destroy(item.gameObject);
        }

        items.Clear();
        totalValue = 0;

        UpdateTotal();
    }
}