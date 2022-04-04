using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PistolBullet : MonoBehaviour
{
    public Vector3 hitPoint;
    public int speed;
    Mission mission;
    //public AudioSource myShot;
    // Start is called before the first frame update
    void Start()
    {
        mission = GameObject.Find("Asuna").GetComponent<Mission>();
    }

    public void Push()
    {
        this.GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.transform.GetComponent<EnemyLogic>().pistolHit();
        }
        else if (col.gameObject.tag == "Boss")
        {
            col.transform.GetComponent<Boss>().pistolHit();
        }
        else if(col.gameObject.tag == "Target" && mission.index == 2 && !mission.next)
        {
            mission.missionText.SetText("Shoot 10 Rounds at the shooting target!("+mission.curr+"/10)");
            mission.curr++;
            if (mission.curr == 11) mission.nextM();
        }
        Destroy(this.gameObject);
    }
}