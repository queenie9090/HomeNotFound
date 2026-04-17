using UnityEngine;

public class ToiletDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform door; // Drag the physical door mesh here
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;

    void Update()
    {
        // Smoothly rotate between the open and closed angles
        Quaternion targetRotation = isOpen ?
            Quaternion.Euler(0, openAngle, 0) :
            Quaternion.Euler(0, 0, 0);

        door.localRotation = Quaternion.Lerp(door.localRotation, targetRotation, Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        // Detects either the whole Player body or just the Hand
        if (other.CompareTag("Player") || other.CompareTag("Hand"))
        {
            if (!isOpen)
            {
                isOpen = true;
                Debug.Log("Toilet Door Opened");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Hand"))
        {
            if (isOpen)
            {
                isOpen = false;
                Debug.Log("Toilet Door Closed");
            }
        }
    }
}