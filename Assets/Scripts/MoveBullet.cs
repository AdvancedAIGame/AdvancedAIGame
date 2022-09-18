using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    public Vector3 hitPoint;

    //hitting something thats not the monster
    public GameObject dirt;

    //show blood when moster get hit
    public GameObject blood;

    //bullet speed
    public int speed = 10000;
    void Start()
    {
        
    }

    //when bullet is fired and it collides witht the monster
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Monsters")
        {
            //decrease monster health
            collision.gameObject.GetComponent<MonsterHealth>().currentHealth -= 10;
            //make blood visible
            GameObject newBlood = Instantiate(blood, this.transform.position, this.transform.rotation);
            newBlood.transform.parent = collision.transform;
            Destroy(this.gameObject);

        }
        //if we are not shooting anywhere else add dirt
        else
        {
            Instantiate(dirt, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }


        Destroy(this.gameObject);
    }
}
