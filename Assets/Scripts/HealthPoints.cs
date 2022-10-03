using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    //get collison between player and the health points
    public void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        //check if theres any health collected
        if(playerInventory != null)
        {
            //collect health point on collison and remove object
            playerInventory.HealthCollected();
            gameObject.SetActive(false);
            print("Health Collected");
        }
    }
}
