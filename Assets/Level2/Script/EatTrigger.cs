using UnityEngine;

public class EatTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip eatSound;
    public Level2Manager level2Manager;
    public GameObject endPath;
    public GameObject food;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            if (audioSource != null && eatSound != null)
                audioSource.PlayOneShot(eatSound);
            DialogueManager.Instance.ShowDialogue("Taste like crap... let's move on to find a place for the night...");
            level2Manager.SetState(Level2Manager.GameState.LevelComplete);
            endPath.SetActive(false);
            food.SetActive(false);
        }
    }


}
