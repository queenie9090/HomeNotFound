using UnityEngine;

public class StoleDetector : MonoBehaviour
{

    [Header("Bagger")]
    public GameObject bagger1;
    public GameObject bagger2;

    public BaggerTrigger stoleDialogue;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deadSound;

    void Start()
    {
        gameObject.SetActive(false);
        bagger2.SetActive(false);
    }

    public void setActiveStole()
    {
        gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioSource != null && deadSound != null)
                audioSource.PlayOneShot(deadSound);

            stoleDialogue.moneyStole = true;
            DialogueManager.Instance.ShowDialogue("What's that sound?! Better go check on the guy just now!");

            if (bagger1 != null && bagger2 != null)
            {
                bagger1.SetActive(false);
                bagger2.SetActive(true);
            }

            Collider myCollider = GetComponent<Collider>();
            if (myCollider != null) myCollider.enabled = false;

            Invoke("DisableThisObject", 3f);
        }
    }

    void DisableThisObject()
    {
        gameObject.SetActive(false);

    }

}
