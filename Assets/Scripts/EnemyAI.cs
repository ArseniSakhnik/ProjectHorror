using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public LayerMask whatIsPlayer;

    public bool isDeath;

    public NavMeshAgent agent;
    public Transform[] waypoints;
    public GameObject player;
    int waypointIndex;
    Vector3 target;
    public int health;
    public Animator anim;
    



    public float cooldownTime = 0, sightRange, attackRange;
    public bool duringAttack, playerInSightRange, playerInAttackRange, onCooldown = false;


    IEnumerator Attack()
    {
        //anim.SetBool("Run", false);
       // anim.SetBool("Attack", true);
        anim.Play("Attack");
        yield return new WaitForSeconds(0.5f);
        if (playerInAttackRange)
        {
            player.GetComponent<PlayerController>().GetDamage(30);
            cooldownTime = 0;
        }
        duringAttack = false;
    }


    public void TakeDamage(int damage)
    {
        health = health - damage;
        if (health<=0)
        {
            isDeath = true;
            Death();
        }
    }

    public void Death()
    {
        anim.SetBool("Run", false);
        anim.SetBool("Attack", false);
        //anim.SetBool("Dies", true);
        anim.Play("Death");
    }


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
        if (duringAttack) return;
        if (isDeath)
        {
            return;
        }
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);



        if (cooldownTime < 20 && !playerInSightRange)
        {
            cooldownTime += Time.deltaTime;
        }
        else onCooldown = false;

        if (playerInAttackRange)
        {
            StartCoroutine(Attack());
            duringAttack = true;
            return;
        }


        if (!playerInSightRange) Patroling();
        else if (playerInSightRange && !onCooldown) Chase();

    }


    private void Chase()
    {
        anim.SetBool("Run", true);
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
