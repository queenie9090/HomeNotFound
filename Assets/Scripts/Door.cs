using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform door;
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;
    private bool canTriggerDog = false;

    public DogAiInLv3 dog;
    public Transform stayPoint2;

    void Update()
    {
        Quaternion targetRotation = isOpen ?
            Quaternion.Euler(0, openAngle, 0) :
            Quaternion.Euler(0, 0, 0);

        door.localRotation = Quaternion.Lerp(door.localRotation, targetRotation, Time.deltaTime * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            OpenDoor();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;
        Debug.Log("Door opened");

        // ONLY trigger dog AFTER shower
        if (canTriggerDog && dog != null && stayPoint2 != null)
        {
            dog.GoToStayPoint(stayPoint2);
        }
    }

    void CloseDoor()
    {
        if (!isOpen) return;

        isOpen = false;
        Debug.Log("Door closed");
    }

    public void EnableDogTrigger()
    {
        canTriggerDog = true;
        Debug.Log("Dog can now be triggered by door");
    }
}