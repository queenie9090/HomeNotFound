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

    [Header("Text Display")]
    public MoneyTextDisplay textDisplay;

    [Header("Grid & Display Settings")]
    public Transform displayParent;
    [Tooltip("How many bills can fit in a single column before starting a new one")]
    public int maxItemsPerColumn = 4;
    public float ySpacing = 0.15f;   
    public float xSpacing = 0.15f;   

    [Header("Pooling Settings")]
    public int initialPoolSize = 5;
    public Vector3 moneyScale = new Vector3(1f, 1f, 1f);

    private List<GameObject> rm10Pool = new List<GameObject>();
    private List<GameObject> rm5Pool = new List<GameObject>();
    private List<GameObject> rm1Pool = new List<GameObject>();

    private void Awake()
    {
        PrewarmPool(rm10Pool, rm10Prefab, initialPoolSize);
        PrewarmPool(rm5Pool, rm5Prefab, initialPoolSize);
        PrewarmPool(rm1Pool, rm1Prefab, initialPoolSize);
    }

    private void PrewarmPool(List<GameObject> pool, GameObject prefab, int count)
    {
        if (prefab == null || displayParent == null) return;

        for (int i = 0; i < count; i++)
        {
            GameObject newObj = Instantiate(prefab, displayParent);
            newObj.SetActive(false);
            pool.Add(newObj);
        }
    }

    public void RefreshDisplay()
    {
        if (manager == null)
        {
            Debug.LogError("<color=red>[MoneyVis]</color> Level2Manager is NULL!");
            return;
        }

        int currentCash = manager.playerMoney;
        Debug.Log($"<color=cyan>[MoneyVis]</color> Menu opened. Player has: RM {currentCash}");

        UpdateVisuals(currentCash);

        if (textDisplay != null)
        {
            textDisplay.RefreshText();
        }
    }

    public void UpdateVisuals(int totalMoney)
    {
        if (displayParent == null || rm10Prefab == null || rm5Prefab == null || rm1Prefab == null) return;

        int numRM10 = totalMoney / 10;
        totalMoney %= 10;

        int numRM5 = totalMoney / 5;
        totalMoney %= 5;

        int numRM1 = totalMoney / 1;

     
        int currentItemIndex = 0;

        currentItemIndex = ManagePool(rm10Pool, rm10Prefab, numRM10, currentItemIndex);
        currentItemIndex = ManagePool(rm5Pool, rm5Prefab, numRM5, currentItemIndex);
        currentItemIndex = ManagePool(rm1Pool, rm1Prefab, numRM1, currentItemIndex);
    }

    private int ManagePool(List<GameObject> pool, GameObject prefab, int neededAmount, int startIndex)
    {
        int currentIndex = startIndex;
        int loopCount = Mathf.Max(pool.Count, neededAmount);

        for (int i = 0; i < loopCount; i++)
        {
            if (i < neededAmount)
            {
                if (i >= pool.Count)
                {
                    GameObject newObj = Instantiate(prefab, displayParent);
                    pool.Add(newObj);
                }

                GameObject obj = pool[i];
                obj.SetActive(true);

           
                int columnIndex = currentIndex / maxItemsPerColumn;

        
                int positionInColumn = currentIndex % maxItemsPerColumn;

                float targetX = columnIndex * xSpacing;
                float targetY = positionInColumn * ySpacing;

        
                obj.transform.localPosition = new Vector3(targetX, targetY, 0);

                obj.transform.localRotation = Quaternion.Euler(270f, 0f, 0f);
                obj.transform.localScale = moneyScale;

                currentIndex++; 
            }
            else
            {
                pool[i].SetActive(false);
            }
        }

        return currentIndex; 
    }
}