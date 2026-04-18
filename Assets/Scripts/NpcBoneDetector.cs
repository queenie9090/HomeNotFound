using UnityEngine;

public class NpcBoneDetector : MonoBehaviour
{
    public bool isTargetedByDog = false;

    [Header("Animation")]
    public Animator anim; 

    [Header("Running Settings")]
    public Transform[] waypoints;
    public float runSpeed = 0.5f;
    public float rotationSpeed = 2f;

    [Header("Audio")]
    public AudioSource mainAudio;
    public AudioSource sfxAudio;

    public AudioClip idleClip;
    public AudioClip warningClip;
    public AudioClip chaseClip;
    public AudioClip hitClip;

    public float attackCooldown = 1.2f;
    private float nextAttackTime = 0f;

    private bool isWarningPlaying = false;

    public static bool npcDistracted = false;

    private int currentWaypointIndex = 0;
    private bool isRunning = false;

    private bool hasCompletedNpcTask = false;

    private void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();

        PlayIdleSound();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bone"))
        {
            Destroy(collision.gameObject);
            StartRunning();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bone"))
        {
            Destroy(other.gameObject);
            StartRunning();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (isRunning) return;

        if (other.CompareTag("Player"))
        {
            if (Time.time >= nextAttackTime)
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopHitLoop();

            if (!isRunning)
            {
                PlayIdleSound();
            }

            if (anim != null)
            {
                anim.SetBool("isAttacking", false);
            }
        }
    }

    void AttackPlayer()
    {
        // animation
        if (anim != null)
        {
            anim.SetBool("isAttacking", true);
            anim.SetBool("isChasing", false);
        }

        // sound
        PlayMainSound(warningClip);
        PlayHitLoop();
    }

    void StartRunning()
    {
        if (waypoints.Length > 0 && !isRunning)
        {
            isTargetedByDog = true;
            isRunning = true;
            npcDistracted = true;

            isWarningPlaying = false;
            StopHitLoop();

            // COMPLETE TASK 2 HERE
            if (!hasCompletedNpcTask)
            {
                if (JournalInLv3.Instance != null)
                {
                    JournalInLv3.Instance.CompleteTask(1); // Task 2
                }

                hasCompletedNpcTask = true;
            }

            PlayChaseSound();

            if (anim != null)
            {
                anim.SetBool("isChasing", true);
                anim.SetBool("isAttacking", false);
            }
        }
    }

    void Update()
    {
        if (isRunning) RunThroughWaypoints();
    }

    void RunThroughWaypoints()
    {
        Transform target = waypoints[currentWaypointIndex];

        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotationSpeed);
        }

        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 1.0f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    void StopHitLoop()
    {
        if (sfxAudio.isPlaying)
        {
            sfxAudio.Stop();
        }
    }

    void PlayMainSound(AudioClip clip)
    {
        if (mainAudio.clip == clip && mainAudio.isPlaying) return;

        mainAudio.Stop();
        mainAudio.clip = clip;
        mainAudio.loop = true;
        mainAudio.Play();
    }

    void PlayHitLoop()
    {
        if (sfxAudio.clip == hitClip && sfxAudio.isPlaying) return;

        sfxAudio.Stop();
        sfxAudio.clip = hitClip;
        sfxAudio.loop = true;
        sfxAudio.Play();
    }

    public void PlayWarningSound()
    {
        if (isWarningPlaying) return;

        isWarningPlaying = true;

        PlayMainSound(warningClip);
        PlayHitLoop();

        Invoke(nameof(ResetWarning), 2f);
    }

    void ResetWarning()
    {
        isWarningPlaying = false;
        StopHitLoop();

        if (!isRunning)
        {
            PlayMainSound(idleClip);
        }
    }

    void PlayChaseSound()
    {
        isWarningPlaying = false;
        StopHitLoop();
        PlayMainSound(chaseClip);
    }

    void PlayIdleSound()
    {
        PlayMainSound(idleClip);
    }
}