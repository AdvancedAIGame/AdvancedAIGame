using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    //health of the player initial health is 50

    public int playerHealth;
    public UnityEvent<PlayerInventory> onHealthCollected;
    private void Start()
    {
        playerHealth = 50;
    }

    private void Update()
    {
        PlayerDied();
    }
    public void HealthCollected()
    {
        playerHealth += 5;
        onHealthCollected.Invoke(this);
    }

    public void HealthLoss()
    {
        playerHealth -= 3;
        onHealthCollected.Invoke(this);
    }

    public void PlayerDied()
    {
        if(playerHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

   
 
 
    

  
   
}
