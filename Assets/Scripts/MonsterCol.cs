using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCol : MonoBehaviour
{
    //get collision between player and monster
    public void OnTriggerEnter(Collider other)
    {
        PlayerInventory player = other.GetComponent<PlayerInventory>();

        if(player != null)
        {
            player.HealthLoss(gameObject.GetComponent<EnemyController>().damage);
            print("Monster Took away Health");
        }
    }

}
