using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCol : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wall")
        {
            print("Wall collision");
            Destroy(this.gameObject);
        }
    }
}
