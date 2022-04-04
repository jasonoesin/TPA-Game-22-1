using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemyBullet : MonoBehaviour
{
    public int speed;
    public void Push()
    {
        this.GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    public void PushBoss()
    {

    }

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Player")
        {
            col.transform.GetComponent<PlayerController>().Hit();
        }
        Destroy(this.gameObject);
    }
}