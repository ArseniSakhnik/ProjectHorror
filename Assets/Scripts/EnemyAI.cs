using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public LayerMask whatIsPlayer;



    public NavMeshAgent agent;
    public Transform[] waypoints;
    public GameObject player;
    int waypointIndex;
    Vector3 target;
    public int health = 100;
    public Animator anim;




    public float cooldownTime = 0, sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, onCooldown = false;



    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        UpdateDestination();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        if (cooldownTime < 10 && !playerInSightRange)
        {
            cooldownTime += Time.deltaTime;
        }
        else onCooldown = false;

        if (!playerInSightRange) Patroling();
        else if (playerInSightRange && !onCooldown) Chase();
    }


    private void Chase()
    {
        agent.SetDestination(player.transform.position);
        if (cooldownTime > 0)
        {
            cooldownTime = cooldownTime - Time.deltaTime;
        }
        if (cooldownTime <= 5)
        {
            onCooldown = true;
            IteratorWaypointIndex();
            UpdateDestination();
        }

    }

    private void Patroling()
    {
       anim.SetBool("Run", true);
        if (Vector3.Distance(transform.position, target) < 1)
        {
            IteratorWaypointIndex();
            UpdateDestination();
        }
    }

    void UpdateDestination()
    {
        anim.SetBool("Run", false);
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    void IteratorWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
