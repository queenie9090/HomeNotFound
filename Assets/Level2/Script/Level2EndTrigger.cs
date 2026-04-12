using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2EndTrigger : MonoBehaviour
{
    public Level2Manager level2Manager;

    void OnTriggerEnter(Collider other)
    {
        var gm = Level2Manager.Instance;

    if (other.CompareTag("Player") && gm.currentState != Level2Manager.GameState.LevelComplete)
        {
            DialogueManager.Instance.ShowDialogue("Don't wanna go there yet...");
        }
    else if(other.CompareTag("Player") && gm.currentState == Level2Manager.GameState.LevelComplete)
        {
            //change scene to level 3
            SceneManager.LoadScene("Level 3");
        }
    }
}
