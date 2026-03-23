using UnityEngine;

public class RecycleItem : MonoBehaviour
{
    public int value = 2;

    public void Collect()
    {
        Level2Manager.Instance.AddMoney(value);
        Destroy(gameObject);
    }
}
