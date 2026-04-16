using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform door;
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;

    public DogAiInLv3 dog;
    public Transform stayPoint2;

    void Update()
    {
        Quaternion targetRotation = isOpen ?
            Quaternion.Euler(0, openAngle, 0) :
            Quaternion.Euler(0, 0, 0);

        door.localRotation = Quaternion.Lerp(door.localRotation, targetRotation, Time.deltaTime * speed);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Hand") && Input.GetKeyDown(KeyCode.G))
        {
            ToggleDoor();
        }
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
}