using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class tempScript : MonoBehaviour
{

    NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
