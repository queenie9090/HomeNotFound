using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPressButton : MonoBehaviour
{
    public XRWeighingScale scale;

    [Header("Button Animation")]
    public float pressDepth = 0.02f;
    public float returnSpeed = 5f;

    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            initialPos,
            Time.deltaTime * returnSpeed
        );
    }

    // Called by XR Interaction
    public void OnRaySelect()
    {
        // Press animation
        transform.localPosition = initialPos - new Vector3(0, 0, pressDepth);

        // SELL ITEMS
        scale.SellItems();
    }
}