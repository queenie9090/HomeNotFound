using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineOfSight : MonoBehaviour
{
    public Transform target; //the enemy
    float seeRange = 12.0f; //maximum attack distance –
                            //will attack if closer than
                            //this to the enemy
    float sightAngle = 60f;

    Animator anim;
    Pursue pursue;
    Wandering wander;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        pursue = GetComponent<Pursue>();
        wander = GetComponent<Wandering>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        if (CanSeeTarget())
        {
            anim.SetBool("isRunning", true);

            pursue.target = target;
            pursue.DoPursue();
        }
        else
        {
            anim.SetBool("isRunning", false);

            wander.DoWander();
        }
    }
    bool CanSeeTarget()
    {
        Vector3 directionToTarget = target.position - transform.position;
        float angle = Vector3.Angle(directionToTarget, transform.forward);

        if (Vector3.Distance(transform.position, target.position) > seeRange || angle > sightAngle)
            return false;

        return true;
    }

}