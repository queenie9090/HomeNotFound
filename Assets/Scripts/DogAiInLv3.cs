using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class DogAiInLv3 : MonoBehaviour
{
    public Animator anim;

    [Header("References")]
    public Transform player;
    public Transform playerHand;
    public Transform npc;
    private NpcBoneDetector npcDetector;
    private GameObject cachedBone;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 8f;
    public float stopDistance = 1.5f;
    public LayerMask obstacleMask; 

    [Header("Wander")]
    public float wanderRadius = 5f;
    private Vector3 wanderTarget;
    private float wanderTimer;

    [Header("Dialogue UI")]
    public TMP_Text dialogueText;

    [Header("Custom Dialogue")]
    [TextArea(3, 10)]
    public string[] snatchDialogue;

    private bool dialogueShown = false;

    [Header("Stay Points")]
    public Transform stayPoint1;
    public Transform stayPoint2;
    private bool goToStayPoint = false;
    private Transform currentStayPoint;

    private bool hasCompletedDogTask = false;

    [Header("Ending Sequence")]
    public CanvasGroup fadeScreen; 
    public string endSceneName = "EndScene"; //Type the name
    void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();

        cachedBone = GameObject.FindGameObjectWithTag("Bone");
        if (npc != null) npcDetector = npc.GetComponent<NpcBoneDetector>();
        PickNewWanderTarget();

        if (fadeScreen != null) fadeScreen.alpha = 0f;
    }

    void Update()
    {
        if (goToStayPoint && currentStayPoint != null)
        {
            float dist = Vector3.Distance(transform.position, currentStayPoint.position);

            if (dist > stopDistance)
            {
                anim.SetBool("isRunning", true);
                anim.SetBool("isWaiting", false);

                PursueTarget(currentStayPoint.position);
            }
            else
            {
                anim.SetBool("isRunning", false);

                if (currentStayPoint == stayPoint1)
                {
                    anim.SetBool("isWaiting", true);
                    anim.SetBool("isSitting", false);
                }
                else if (currentStayPoint == stayPoint2)
                {
                    anim.SetBool("isWaiting", false);
                    anim.SetBool("isSitting", true); // trigger sitting animation
                }

                if (!dialogueShown)
                {
                    ShowDialogue();
                    dialogueShown = true;
                }
            }

            return;
        }


        bool blocked = IsPathBlocked();


        if (blocked)
        {
            // SPIN if hitting wall or edge
            transform.Rotate(Vector3.up * rotationSpeed * 15f * Time.deltaTime);
        }
        else
        {
            if (npcDetector != null && npcDetector.isTargetedByDog)
            {
                if (!hasCompletedDogTask)
                {
                    if (JournalInLv3.Instance != null)
                    {
                        JournalInLv3.Instance.CompleteTask(0);
                    }
                    hasCompletedDogTask = true; 
                }

                anim.SetBool("isRunning", true);
                anim.SetBool("isWaiting", false);
                PursueTarget(npc.position);
            }
            else if (IsHoldingBone())
            {
                anim.SetBool("isRunning", true);
                anim.SetBool("isWaiting", false);
                PursueTarget(player.position);
            }
            else
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isWaiting", false);
                Wander();
            }
        }
    }

    // Updated Block Check
    bool IsPathBlocked()
    {
        // Offset the start so it doesn't hit the dog's own model
        Vector3 forwardOffset = transform.forward * 0.6f;
        Vector3 rayOrigin = transform.position + Vector3.up + forwardOffset;

        // Wall Check
        if (Physics.Raycast(rayOrigin, transform.forward, 1.0f, obstacleMask)) return true;

        // Edge Check
        Vector3 edgePos = transform.position + (transform.forward * 1.5f) + Vector3.up;
        if (!Physics.Raycast(edgePos, Vector3.down, 2.5f, obstacleMask)) return true;

        return false;
    }

    void PursueTarget(Vector3 targetPos)
    {
        float distance = Vector3.Distance(transform.position, targetPos);

        // Rotate to face target
        Vector3 dir = (targetPos - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
        }

        // Move forward
        if (distance > stopDistance)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    bool IsHoldingBone()
    {
        if (cachedBone == null)
        {
            cachedBone = GameObject.FindGameObjectWithTag("Bone");
            return false;
        }
        return Vector3.Distance(playerHand.position, cachedBone.transform.position) < 0.6f;
    }

    void Wander()
    {
        wanderTimer += Time.deltaTime;

        // Move forward at half speed while wandering
        transform.Translate(Vector3.forward * (moveSpeed * 0.5f) * Time.deltaTime);

        // Look towards the wander target
        Vector3 dir = (wanderTarget - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
        }

        // Pick a new target if we reached the old one or time ran out
        if (wanderTimer > 4f || Vector3.Distance(transform.position, wanderTarget) < 1.5f)
        {
            PickNewWanderTarget();
        }
    }

    void PickNewWanderTarget()
    {
        wanderTarget = transform.position + (Random.insideUnitSphere * wanderRadius);
        wanderTarget.y = transform.position.y;
        wanderTimer = 0;
    }

    public void GoToStayPoint(Transform target)
    {
        currentStayPoint = target;
        goToStayPoint = true;

        anim.SetBool("isWaiting", false);
        anim.SetBool("isSitting", false);

        dialogueShown = false;
    }

    void ShowDialogue()
    {
        if (currentStayPoint == stayPoint1)
        {
            StartCoroutine(PlayDialogueSequence(0, 3)); // show element 0,1,2
        }
        else if (currentStayPoint == stayPoint2)
        {
            StartCoroutine(PlayDialogueSequence(3, 3));
        }
    }

    IEnumerator PlayDialogueSequence(int startIndex, int count)
    {
        for (int i = startIndex; i < startIndex + count && i < snatchDialogue.Length; i++)
        {
            DialogueManager.Instance.ShowDialogue(snatchDialogue[i]);
            yield return new WaitForSeconds(3f); // time between dialogue lines

            if (startIndex == 3)
            {
                StartCoroutine(EyeBlinkSequence());
            }
        }

        
    }
    IEnumerator EyeBlinkSequence()
    {
        if (fadeScreen == null)
        {
            Debug.LogWarning("Fade Screen is missing! Just loading scene.");
            SceneManager.LoadScene(endSceneName);
            yield break;
        }

        // Blink 1 (Fast)
        yield return StartCoroutine(FadeToBlack(1f, 0.5f)); // Eyes close
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(FadeToBlack(0f, 0.3f)); // Eyes open
        yield return new WaitForSeconds(0.3f);

        // Blink 2 (Slower)
        yield return StartCoroutine(FadeToBlack(1f, 0.8f)); // Eyes close
        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(FadeToBlack(0f, 0.5f)); // Eyes open
        yield return new WaitForSeconds(0.5f);

        // Final Close (Very slow, passing out)
        yield return StartCoroutine(FadeToBlack(1f, 2.5f)); // Eyes close for good
        yield return new WaitForSeconds(1f); // Wait in darkness

        // Load the End Scene
        SceneManager.LoadScene(endSceneName);
    }

    IEnumerator FadeToBlack(float targetAlpha, float duration)
    {
        float startAlpha = fadeScreen.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            fadeScreen.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null; // wait until next frame
        }

        fadeScreen.alpha = targetAlpha; // Ensure it perfectly hits the target
    }
}