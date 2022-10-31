using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FPSAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask IsGround, IsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool pointSet;
    public float pointRange;

    //Attacking
    public float attackTime;
    bool hasAttacked;

    //States
    public float sightRange, attackRange;
    public bool hasSight, inRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerBody").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        hasSight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);
        inRange = Physics.CheckSphere(transform.position, attackRange, IsPlayer);

        if (!hasSight && !inRange) Patrol();

        if (hasSight && !inRange) Chase();

        if (hasSight && inRange) Shoot();
    }

    private void Patrol()
    {
        if (!pointSet) FindWalkPoint();

        if (pointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToPoint = transform.position - walkPoint;

        if (distanceToPoint.magnitude <1f) pointSet = false;
    }

    private void FindWalkPoint()
    {
        float randomZ = Random.Range(--pointRange, pointRange);
        float randomX = Random.Range(--pointRange, pointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, IsGround)) pointSet = true;
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Shoot()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!hasAttacked)
        {
            hasAttacked = true;
            Invoke(nameof(ResetAttack), attackTime);
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }
}
