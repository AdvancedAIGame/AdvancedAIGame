using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //radius around enemy
    public float fovRadius;
    //angle of enemy sight
    [Range(0, 360)]
    public float fovAngle;
    //layermask to look for
    public LayerMask targetMask;
    //layermask that obstructs the fov
    public LayerMask obstructionMask;
    //bool check if player is visible
    public bool canSeePlayer;
    //bool check if enemy is idle
    public bool isIdle;
    //player gameobject
    private GameObject player;
    //player position
    private Vector3 playerPosition;
    private Animator animator;
    //bool check if player is caught
    public bool caughtPlayer;
    //the navmeshagent
    private NavMeshAgent navMeshAgent;
    //speed variables
    public float speedWalk = 3;
    public float speedRun = 4;
    //wait time
    private float waitTime = 4;
    //original enemy position
    private Vector3 originalEnemyPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //animator = GetComponent<Animator>();

        StartCoroutine(FoVRoutine());

        isIdle = true;
        canSeePlayer = false;
        caughtPlayer = false;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;

        originalEnemyPos = gameObject.transform.position;

        //varying radius and angles so that the best are chosen for clonal expansion later
        //randomise fovAngle between 95 and 120
        fovAngle = Random.Range(95, 120);
        //randomise radius between 5 and 9
        fovRadius = Random.Range(5, 9);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //check fov
        //FieldOfViewCheck();
        if (isIdle)
            //run idle
            Idling();
        else
            //else chase
            Running();
    }

    private IEnumerator FoVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        //check for colliders
        //from the centre of the enemy with the radius
        //look for object with the specific layermask (the player)
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, fovRadius, targetMask);

        //if there exists a collider with player layermask
        if (rangeChecks.Length != 0)
        {
            //there is only the player there so just get the first one
            Transform target = rangeChecks[0].transform;
            //where the enemy is looking to where the player is
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
            {
                //is the player close enough 
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //a raycast from the centre of enemy aimed at the player, limited by distance and obstructions
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    isIdle = false;
                    playerPosition = player.transform.position;
                }
            }
            if (Vector3.Distance(transform.position, target.position) > fovRadius)
            { 
                canSeePlayer = false;
                isIdle = true;
            }
        }
    }

    private void Idling()
    {
        //animator.SetLayerWeight(5, 1);
        //animator.SetTrigger("Idle");
    }


    private void Running()
    {
        //if the player hasn't been caught yet
        if (!caughtPlayer)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speedRun;
            navMeshAgent.SetDestination(playerPosition);
            //animator.SetTrigger("Run");
        }

        //if the destination has been reached
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            //haven't caught player, can't see player, and wait time is over, back to idle
            if (waitTime <= 0 && !caughtPlayer && !canSeePlayer)
            {
                isIdle = true;
                navMeshAgent.isStopped = false;
                navMeshAgent.speed = speedWalk;
                waitTime = 4;
                navMeshAgent.SetDestination(originalEnemyPos);
            }
            else
            {
                //can't see the player so I'll stop and wait for a bit
                if (!canSeePlayer)
                {
                    navMeshAgent.isStopped = true;
                    navMeshAgent.speed = 0;
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }
}
