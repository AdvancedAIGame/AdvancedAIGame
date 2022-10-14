using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawn;
    public GameObject bulletPrefab;
   
    
    public float bulletSpeed = 50;
    public int nBullets = 5;
    //disable shooting when there are no bullets
    public bool buttonEnabled;

   


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            
            buttonEnabled = true;
            if(nBullets >= 0 && buttonEnabled == true)
            {
                var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.forward * bulletSpeed;
                nBullets--;
                print("Bullets left" + nBullets);

            }
            else if(nBullets == 0)
            {
                buttonEnabled = false;
                //this.gameObject.SetActive(false);
                print("You are out of bullets collect them");
            }

        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        
        
            if (other.gameObject.tag == "Reload")
            {
                 nBullets += 10;
                this.gameObject.SetActive(true);
            }
        
       
    }*/
}
