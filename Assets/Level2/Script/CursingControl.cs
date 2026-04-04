using UnityEngine;

public class CursingControl : MonoBehaviour
{
    public static CursingControl Instance;

    [Header("Content Settings")]
    public bool allowCursing = false;

    void Awake()
    {
        Instance = this;
    }

    public void SetCursing(bool value)
    {
        allowCursing = value;
        Debug.Log("Allow Cursing: " + value);
    }
}