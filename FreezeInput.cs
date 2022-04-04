using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeInput : MonoBehaviour
{
    // Start is called before the first frame update
    private static bool freezed;

    public void Freeze()
    {
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerController>().anim.SetFloat("FB", 0);
        GetComponent<PlayerController>().anim.SetFloat("LR", 0);
        GetComponent<Aim>().enabled = false;
        GetComponent<Scope>().enabled = false;
        GetComponent<Inventory>().enabled = false;
        GameObject.Find("Character Cam").GetComponent<Rifle>().enabled = false;
        GameObject.Find("Character Cam").GetComponent<Pistol>().enabled = false;
        freezed = true;
    }

    public void Release()
    {
        GetComponent<PlayerController>().enabled = true;
        GetComponent<Aim>().enabled = true;
        GetComponent<Scope>().enabled = true;
        GetComponent<Inventory>().enabled = true;
        GameObject.Find("Character Cam").GetComponent<Rifle>().enabled = true;
        GameObject.Find("Character Cam").GetComponent<Pistol>().enabled = true;
        freezed = false;
    }

    public static bool getFreeze()
    {
        return freezed;
    }
}
