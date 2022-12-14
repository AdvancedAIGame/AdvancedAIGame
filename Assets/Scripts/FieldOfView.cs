using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    //angle limited to 0 - 360
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    // Start is called before the first frame update
    void Start()
    {
        //get player
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FoVRoutine());

        //varying radius and angles so that the best are chosen for clonal expansion later
        //randomise fovAngle between 95 and 120
        angle = Random.Range(95, 120);
        //randomise radius between 5 and 9
        radius = Random.Range(5, 9);
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
        //look for object with the specific layerMask (the player)
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        //there exists a collider with player layerMask
        if (rangeChecks.Length != 0)
        {
            //there is only the player there so just get the first one
            Transform target = rangeChecks[0].transform;
            //where the enemy is looking to where the player is
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            //
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                //is the player close enough
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //a raycast from the centre of enemy aimed at the player, limited by distance and obstructions
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
