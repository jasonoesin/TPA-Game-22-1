using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtChar : MonoBehaviour
{
    public Transform target;
    void Update()
    {
        if(target != null)
        {
            Vector3 loc = new Vector3(target.position.x,this.transform.position.y,target.position.z);
            transform.LookAt(loc);
        }
    }
}
