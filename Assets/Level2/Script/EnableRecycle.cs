using UnityEngine;

public class EnableRecycle : MonoBehaviour
{

    public GameObject recycleLight;
    public GameObject recycleTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        recycleLight.SetActive(false);
        recycleTrigger.SetActive(false);
    }

    // Update is called once per frame
    public void SetActiveRecycle()
    {
        recycleLight.SetActive(true);
        recycleTrigger.SetActive(true);
    }
}
