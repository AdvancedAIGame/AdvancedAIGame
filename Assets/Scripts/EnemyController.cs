using RandomStrings;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private float originalYRotation; 
    public string signature;
    public float affinity;
    public int damage;

    public TextMesh tm;
    GameObject sign;

    private Camera cam;

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
        originalYRotation = gameObject.transform.eulerAngles.y;

        //varying radius and angles so that the best are chosen for clonal expansion later
        //randomise fovAngle between 95 and 120
        fovAngle = Random.Range(80, 120);
        //randomise radius between 5 and 9
        fovRadius = Random.Range(4, 8);

        //randomise damage output
        damage = Random.Range(3, 8);

        sign = new GameObject("enemyAffinity");
        sign.transform.parent = transform;
        sign.transform.rotation = Camera.main.transform.rotation;

        tm = sign.AddComponent<TextMesh>();
        tm.text = "Affinity: ";
        tm.color = new Color(0.8f, 0.8f, 0.8f);
        tm.fontStyle = FontStyle.Bold;
        tm.alignment = TextAlignment.Center;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.characterSize = 0.065f;
        tm.fontSize = 60;

        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        sign.transform.position = transform.position + Vector3.up * 3f;
        sign.transform.rotation = Quaternion.LookRotation(sign.transform.position - cam.transform.position);

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

    public void clonalExpansion()
    {

        Antibody at = new Antibody(12);
        var playerSig = PlayerPrefs.GetString("playerSignature");
        var daffinity = at.ComputeAffinity(signature, playerSig);
        affinity = daffinity;
        tm.text = "Affinity: " + affinity;

        float affinty_perc = 100 - (affinity / 12)*100;

        float normalized_aff = affinty_perc / 100;

        fovAngle = (fovAngle * normalized_aff) + fovAngle;

        if(fovAngle > 160)
        {
            fovAngle = 160;
        }


        fovRadius = (fovRadius * normalized_aff) + fovRadius;

        if (fovRadius > 10)
        {
            fovRadius = 10;
        }

        int health = gameObject.GetComponent<MonsterHealth>().maxHealth;

        health = (int)((health * normalized_aff) + health);

        if (health > 150)
        {
            health = 150;
        }

        gameObject.GetComponent<MonsterHealth>().maxHealth = health;

        speedRun = ((speedRun * normalized_aff) + speedRun);

        if (speedRun > 5)
        {
            speedRun = 5;
        }

        damage = (int)((damage * normalized_aff) + damage);

        if (damage > 10)
        {
            damage = 10;
        }
    }

    public void enemyReset()
    {
        isIdle = true;
        canSeePlayer = false;
        navMeshAgent.enabled = false;
        transform.position = originalEnemyPos;//.position;
        //transform.rotation = originalEnemyPos.rotation;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, originalYRotation, transform.eulerAngles.z);

        
        navMeshAgent.enabled = true;

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        waitTime = 4;

        caughtPlayer = false;
        gameObject.GetComponent<MonsterHealth>().currentHealth = gameObject.GetComponent<MonsterHealth>().maxHealth;
    }
}
