using UnityEngine;
using System.Collections.Generic;

public class MoneyVisualizer : MonoBehaviour
{
    [Header("Manager Reference")]
    public Level2Manager manager;

    [Header("Money Prefabs")]
    public GameObject rm10Prefab;
    public GameObject rm5Prefab;
    public GameObject rm1Prefab;

    [Header("Display Settings")]
    public Transform displayParent;
    public float spacing = 0.15f;

    [Header("Pooling Settings")]
    [Tooltip("How many of each bill to pre-spawn invisibly when the game starts")]
    public int initialPoolSize = 5;

    private List<GameObject> rm10Pool = new List<GameObject>();
    private List<GameObject> rm5Pool = new List<GameObject>();
    private List<GameObject> rm1Pool = new List<GameObject>();

    // Awake runs the very first time this GameObject is turned on
    private void Awake()
    {
        // 1. Pre-warm the pools before the menu even opens
        PrewarmPool(rm10Pool, rm10Prefab, initialPoolSize);
        PrewarmPool(rm5Pool, rm5Prefab, initialPoolSize);
        PrewarmPool(rm1Pool, rm1Prefab, initialPoolSize);

        Debug.Log($"<color=lime>[MoneyVis]</color> Pools pre-warmed with {initialPoolSize} of each prefab.");
    }

    private void PrewarmPool(List<GameObject> pool, GameObject prefab, int count)
    {
        if (prefab == null || displayParent == null) return;

        for (int i = 0; i < count; i++)
        {
            // Spawn it and immediately hide it
            GameObject newObj = Instantiate(prefab, displayParent);
            newObj.SetActive(false);
            pool.Add(newObj);
        }
    }

    // OnEnable runs every single time you press the 'N' button to show the menu
    private void OnEnable()
    {
        if (manager == null)
        {
            Debug.LogError("<color=red>[MoneyVis]</color> Level2Manager.Instance is NULL!");
            return;
        }

        int currentCash = manager.playerMoney;
        Debug.Log($"<color=cyan>[MoneyVis]</color> Menu opened. Player currently has: RM {currentCash}");

        UpdateVisuals(currentCash);
    }

    public void UpdateVisuals(int totalMoney)
    {
        if (displayParent == null || rm10Prefab == null || rm5Prefab == null || rm1Prefab == null) return;

        int numRM10 = totalMoney / 10;
        totalMoney %= 10;

        int numRM5 = totalMoney / 5;
        totalMoney %= 5;

        int numRM1 = totalMoney / 1;

        float currentXOffset = 0f;
        currentXOffset = ManagePool(rm10Pool, rm10Prefab, numRM10, currentXOffset);
        currentXOffset = ManagePool(rm5Pool, rm5Prefab, numRM5, currentXOffset);
        currentXOffset = ManagePool(rm1Pool, rm1Prefab, numRM1, currentXOffset);
    }

    private float ManagePool(List<GameObject> pool, GameObject prefab, int neededAmount, float startX)
    {
        float currentX = startX;
        int loopCount = Mathf.Max(pool.Count, neededAmount);

        for (int i = 0; i < loopCount; i++)
        {
            if (i < neededAmount)
            {
                // If the player somehow got rich and needs MORE than the 5 we pre-warmed, 
                // this safely spawns extras on the fly.
                if (i >= pool.Count)
                {
                    GameObject newObj = Instantiate(prefab, displayParent);
                    pool.Add(newObj);
                }

                // Activate and position the money
                GameObject obj = pool[i];
                obj.SetActive(true);
                obj.transform.localPosition = new Vector3(currentX, 0, 0);
                obj.transform.localRotation = prefab.transform.rotation;

                currentX += spacing;
            }
            else
            {
                // We have extra pre-warmed money sitting around, just make sure it's hidden
                pool[i].SetActive(false);
            }
        }

        return currentX;
    }
}