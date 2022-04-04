using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signals : MonoBehaviour
{

    public FreezeInput inp;
    // Start is called before the first frame update
    public void Start()
    {
        inp = GameObject.FindGameObjectWithTag("Player").GetComponent<FreezeInput>();
    }
    public void Freeze()
    {
        inp.Freeze();
    }
    public void Release()
    {
        inp.Release();
    }
}
