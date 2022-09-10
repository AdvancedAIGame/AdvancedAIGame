using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //health of the player initial health is 50
    public int playerHealth { get; private set; } = 50;


    public void HealthCollected()
    {
        //increment health by 5
        playerHealth = playerHealth + 5;
    }
}
