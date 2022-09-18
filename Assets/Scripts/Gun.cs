using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //variables to controll the speed of shooting
    public float coolDownSpeed;
    public float fireRate = 6f;

    //countdown from the last time the gun was fired
    public float recoilCoolDown;
    //reset accuracy from the last time fired
    public float accuracy;

    //make the gun shot not to be accurate everytime
    public float maximumSpreadAngle = 3f;

    public float timeTillMaximumSpread = 10f;

    //Objects used in the gun
    public GameObject bullet;
    public GameObject shootPoint;
    public AudioSource gunshot;
    public AudioClip singleShot;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //increasing cooldown speed every frame, so that after a certain amount of time pass
        //we can check for the fire button
        coolDownSpeed += Time.deltaTime * 60f;

        //check for fire button
        if (Input.GetButton("Fire1"))
        {
            //increase accuracy while firering
            accuracy += Time.deltaTime * 4f;

            //check if cooldown is low enough
            if(coolDownSpeed >= fireRate)
            {
                Shoot();
                gunshot.PlayOneShot(singleShot); //once trigger fired play sound
                coolDownSpeed = 0;
                recoilCoolDown = 1;
            }
        }
        else
        {
            recoilCoolDown -= Time.deltaTime;
            if(recoilCoolDown <= 1)
            {
                //reset the shooting accuracy to normal
                accuracy = 0.0f;
            }
        }

    }

    public void Shoot()
    {
        RaycastHit hit;
        Quaternion fireRotation = Quaternion.LookRotation(transform.forward);

        //make the spread
        float currentSpread = Mathf.Lerp(0.0f, maximumSpreadAngle, accuracy / timeTillMaximumSpread);
        fireRotation = Quaternion.RotateTowards(fireRotation, Random.rotation, Random.Range(0.0f, currentSpread));

        //use the raycast to see if we have hit the monster
        if(Physics.Raycast(transform.position, fireRotation*Vector3.forward,out hit, Mathf.Infinity))
        {
            //fire the bullet
            GameObject temBullet = Instantiate(bullet, shootPoint.transform.position, fireRotation);
            temBullet.GetComponent<MoveBullet>().hitPoint = hit.point;
        }

    }

}
