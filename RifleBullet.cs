using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RifleBullet : MonoBehaviour
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

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Enemy")
        {
            col.transform.GetComponent<EnemyLogic>().Hit();
        }
        else if (col.gameObject.tag == "Boss")
        {
            col.transform.GetComponent<Boss>().Hit();
        }
        else if (col.gameObject.tag == "Target" && mission.index == 3 && !mission.next)
        {
            mission.missionText.SetText("Shoot 50 Bullets with the rifle! (" + mission.curr + "/50)");
            mission.curr++;
            if (mission.curr == 51) mission.nextM();
        }
        Destroy(this.gameObject);
    }
}