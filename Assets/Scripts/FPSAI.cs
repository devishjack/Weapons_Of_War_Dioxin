using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FPSAI : MonoBehaviour
{
    public NavMeshAgent agent;

    private Transform player;
    private MouseLookSingle mls;

    public LayerMask IsGround, IsPlayer;

    private Vector3 Velocity;

    private float gravity = 9.81f;

    //Patroling
    public Vector3 walkPoint;
    bool pointSet;
    bool grounded;

    //Attacking
    public float attackTime;
    bool hasAttacked;

    //States
    public float sightRange, attackRange;
    public bool hasSight, inRange;
    public bool pinged;
    public float PingTimer;
    public float radarPing;

    private void Awake()
    {
        player = GameObject.Find("PlayerBody").transform;
        mls = player.GetComponent<MouseLookSingle>();
        agent = GetComponent<NavMeshAgent>();
        sightRange = 10;
        attackRange = 5;
        attackTime = 3;
        PingTimer = 10;
        radarPing = 10;
        pinged = true;
    }

    private void Update()
    {
        hasSight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);
        inRange = Physics.CheckSphere(transform.position, attackRange, IsPlayer);

        Velocity.y = gravity * Time.deltaTime;

        if (pinged == false)
        {
            if (PingTimer <= 0)
            {
                PingTimer = 10;
                radarPing = 10;
                pinged = true;
            }
            else
            {
                Chase();
                PingTimer -= Time.deltaTime;
            }
        }
        else
        {
            radarPing -= Time.deltaTime;
        }

        if(radarPing <= 0)
        {
            pinged = false;
        }

        if(radarPing > 0)
        {
            if (!hasSight && !inRange) Patrol();
        }

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
        float randomZ = Random.Range(1, 60);
        float randomX = Random.Range(1, 97);

        walkPoint = new Vector3(randomX, transform.position.y, randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, IsGround)) pointSet = true;
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
        pointSet = false;
    }

    private void Shoot()
    {
        pointSet = false;
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!hasAttacked)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.name == "PlayerBody")
                {
                    mls.PlayerDed();
                }
            }
            hasAttacked = true;
            Invoke(nameof(ResetAttack), attackTime);
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

    public void Ded()
    {
        Destroy(gameObject);
    }
}
