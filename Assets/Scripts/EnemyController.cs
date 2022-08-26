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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        StartCoroutine(FoVRoutine());

        isIdle = true;
        canSeePlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //check fov
        FieldOfViewCheck();
        if (isIdle)
            //run patrol
            IdleAnimation();
        else
            //else chase
            RunAnimation();
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
                canSeePlayer = false;
            //fix this can see player thing
        }
    }

    private void IdleAnimation()
    {
        animator.SetLayerWeight(5, 10f);
    }


    private void RunAnimation()
    {
        animator.SetLayerWeight(1, 10f);
    }

    //create idle method (like patrolling) and chasing
    //isIdle isn't changing
    //canSeePlayer does nothing either
}
