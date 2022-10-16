using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    // Start is called before the first frame update
    private void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            //check for collison with bullet 
            currentHealth -= 50;
            print("Bullet Collision");
            Destroy(other.gameObject);
        }
    }
    void Update()
    {
        //destroy monster if health is zero
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
