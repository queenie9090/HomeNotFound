using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator anim;
    private bool isOpen = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        anim.SetBool("isOpen", true);
    }

    void CloseDoor()
    {
        isOpen = false;
        anim.SetBool("isOpen", false);
    }
}