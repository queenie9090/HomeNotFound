using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform door;
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;
    private bool handInside = false;

    public DogAiInLv3 dog;
    public Transform stayPoint2;

    void Update()
    {
        // Handle input ONLY here
        if (handInside && Input.GetKeyDown(KeyCode.G))
        {
            ToggleDoor();
        }

        // Smooth door rotation
        Quaternion targetRotation = isOpen ?
            Quaternion.Euler(0, openAngle, 0) :
            Quaternion.Euler(0, 0, 0);

        door.localRotation = Quaternion.Lerp(door.localRotation, targetRotation, Time.deltaTime * speed);
    }

    void ToggleDoor()
    {
        if (isOpen) return;

        isOpen = true;
        Debug.Log("Door opened");

        if (dog != null && stayPoint2 != null)
        {
            dog.GoToStayPoint(stayPoint2);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            handInside = true;
            Debug.Log("Hand entered");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            handInside = false;
            Debug.Log("Hand exited");
        }
    }
}