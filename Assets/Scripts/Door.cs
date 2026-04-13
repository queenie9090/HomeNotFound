using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform door;
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;
    private bool playerHandInside = false;

    void Start()
    {
        Debug.Log("Script started");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("G pressed");
            ToggleDoor();
        }

        Quaternion targetRotation = isOpen ?
            Quaternion.Euler(0, openAngle, 0) :
            Quaternion.Euler(0, 0, 0);

        door.localRotation = Quaternion.Lerp(door.localRotation, targetRotation, Time.deltaTime * speed);
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            Debug.Log("Door is OPEN");
        }
        else
        {
            Debug.Log("Door is CLOSED");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            playerHandInside = true;
            Debug.Log("Hand touched the door handle");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            playerHandInside = false;
            Debug.Log("Hand left the door handle");
        }
    }
}